using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class MicInput : MonoBehaviour
{

    #region SingleTon

    public static MicInput Inctance { set; get; }

    #endregion

    public static float MicLoudness;
    public static float MicLoudnessinDecibels;
    public int device;
    public DF2ClientAudioTester clientAudioTester;
    public string _device;
    public Text logtext;
    public GameObject canQ;
    public GameObject waitQ;
    //mic initialization
    

    AudioClip _clipRecord;
    AudioClip recordedAudioClip;
    AudioClip _recordedClip  ;
    public AudioSource audioPlayer;
    int _sampleWindow = 128;


    public int devices;
    public float startRecordingTime;
    public float startTalkingTime;
    public int offset;
    public float talkingTime;
    public int duration;
    public Material micObj;
    public MeshRenderer cuberendrer;

    public GameObject mic;
    public GameObject cube;
    public GameObject speaker;
    public GameObject haf;
    public Indicator indicator;

    public bool _isInitialized;

    public Animator animator;
    public float intervalRecord = 240;
    public float intervalTime = 0;
    public lerp _lerp;
    public bool micBlock = false;
     
    public GameObject people;
    public string stateText = "";
    float lastTime = 0;
    float lostTime = 0;
    public GameObject question_back;
    public Text exampleText;
    public Text canQText;
    public Text waitQText;
    public Text questionText;
    public Text doquestionText;
    public int state = 0; // 상태

    public int loop1 = 0;
    public int loop2 = 0;
    public int j = 0;
    public bool isResponse;
    public float amount = 3;
    public GameObject text_add;
    public void Start()
    {
        Application.runInBackground = true;
        RotateHaf();
        waitQ.SetActive(false);
        canQ.SetActive(false);
        if (!PlayerPrefs.HasKey("amount"))      //amount 기본값 = 3f, 저장되어 있는 amount 값이 있을시 그 값을 불러오고, null값이면 Default값 적용 (제어하는 곳은 lerp 스크립트에 있음)
            amount = 3f;
        else
            amount = PlayerPrefs.GetFloat("amount");
    }
    
    void Update()
    {
        CheckPeople();
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetMicBlock();
        }
        if (!(_lerp.isLoud) || !(_lerp.isTalking))  //오디오 데이타 리셋위해 마이크 다시켬
        {
            if (Time.time > intervalTime + intervalRecord)
            {
                Debug.Log("녹음 중지 후 다시 시작");
                StopMicrophone();
                InitMic();
            }
        }
        if (_isInitialized)
        {
            if (micObj) micObj.SetColor("_BaseColor", Color.white);

            cuberendrer.enabled = false;
        }
        else
        {
            if (micObj) micObj.SetColor("_BaseColor", Color.red);
            cuberendrer.enabled = true;
        }

        MicLoudness = MicrophoneLevelMax();
        MicLoudnessinDecibels = MicrophoneLevelMaxDecibels();
    }
    int ResponseNumber = 0;
    int QuestionNumber = 0;
    public void ResponseControl(int _r) //서버에서 온 응답
    {
        ResponseNumber = _r;
        switch (state)
        {
            case 3:
                if(_r == 2001)
                {
                    Debug.Log("4_____________"   + _r);
                    SetState(5);
                }
                else
                {
                    Debug.Log("2_____________" + _r);
                    SetState(2);
                }
                break;
            case 6:
                if (_r >= 1048 && _r <= 1082)
                {
                    SetState(7);
                }

                else if (_r >= 1 && _r <= 500)
                {
                    SetState(7);
                }

                else if (_r >= 1143 && _r <= 1182)
                {
                    SetState(7);
                }

                else
                    ResponseControl();
                //SetState(7);

                break;
            /*case 8:
                if (_r == 2001 || _r == 2002 || _r == 2003)
                {
                    SetState(100);
                }
                SetState(9);

                break;*/
            /*default:
                if (_r == 2001 || _r == 2002 || _r == 2003)
                {
                    SetState(100);
                }
                SetState(9);
                break;*/

        }
    }

    public void ResponseControl()   //이상한 질문 했을때 처리
    {
        clientAudioTester.playAnimation_conv(R(1037, 1044));
        SetState(state - 1);
    }

    public void SetState() //상태변경
    {
        if (state == -1) return;
        if (state == 0) state = 1;
        else if (state == 1) state = 2;
        else if (state == 2) state = 3;
        else if (state == 4) state = 5;
        else if (state == 5) state = 6;
        //else if (state == 6) state = 7;
        else if (state == 7)    //제대로된 질문을 하면 loop1++ 아니면 ResponseControl(), 5번 모두 질문 완료시 종료멘트
        {
            if (loop1 == 4)
            {
                state = 10;
            }
            else
            {
                state = 6;
                loop1++;
            }
        }
        //else if (state == 8) state = 9;
        /*else if (state == 9)
        {
            if (loop2 == 1)
            {
                state = 10;
            }
            else
            {
                state = 8;
                loop2++;
            }
        }*/

        else if (state == 10)
        {
            state = 0;
        }
        else if (state == 100) state = 8;
        SetState(state);
    }

    public void SetState(int _i) //조작
    {
        question_back.SetActive(false);
        questionText.text = "";
        exampleText.text = "";
        switch (_i)
        {
            case -1:
                Debug.Log("state  -1");
                state = -1;
                questionText.text = "가까이와서 질문해보세요";
                canQText.text = " ";
                waitQText.text = " ";
                exampleText.text = "";
                break;
            case 0:
                state = 0;
                Debug.Log("state  0");
                questionText.text = "";
                canQText.text = "대답해보세요.";
                waitQText.text = "말씀을 잘 들어보세요.";
                clientAudioTester.playAnimation_conv(R(1001,1003)); //1001,1002,1003
                break;
            case 1:
                clientAudioTester.playAnimation_conv(R(1004, 1006)); //1004 1005 1006
                break;
            case 2:         //1차 질문 
                state = 2;
                questionText.text = "";
                Debug.Log("state 2");
                switch (j)
                {
                    case 0:
                        QuestionNumber = R(1007, 1009);
                        Debug.Log(QuestionNumber);
                        clientAudioTester.playAnimation_conv(QuestionNumber); //1007 - 1014
                        j++;
                        break;

                    case 1:
                        QuestionNumber = R(1010, 1011);
                        Debug.Log(QuestionNumber);
                        clientAudioTester.playAnimation_conv(QuestionNumber); //1010 - 1011
                        j++;
                        break;

                    case 2:
                        QuestionNumber = R(1012, 1014);
                        Debug.Log(QuestionNumber);
                        clientAudioTester.playAnimation_conv(QuestionNumber); //치하포
                        j++;
                        break;

                    case 3:
                        QuestionNumber = R(1183, 1185);
                        Debug.Log(QuestionNumber);
                        clientAudioTester.playAnimation_conv(QuestionNumber); //독립운동
                        j++;
                        break;

                    case 4:
                        QuestionNumber = R(1186, 1188);
                        Debug.Log(QuestionNumber);
                        clientAudioTester.playAnimation_conv(QuestionNumber); //백범일지
                        j++;
                        break;

                    case 5:
                        QuestionNumber = R(1189, 1191);
                        Debug.Log(QuestionNumber);
                        clientAudioTester.playAnimation_conv(QuestionNumber); //통일운동
                        j++;
                        break;

                    case 6:
                        QuestionNumber = R(1192, 1194);
                        Debug.Log(QuestionNumber);
                        clientAudioTester.playAnimation_conv(QuestionNumber); //서거
                        j = 0;
                        break;
                }
                break;
                
            case 3:
                state = 3;
                Debug.Log("state 3"); //예 아니오 표시 
                InitMic();
                question_back.SetActive(true);
                exampleText.text = "답변 예시";
                questionText.text = "예 좋아요 \n아니오";
                break;
            case 4:       //2차질문
                state = 4;
                Debug.Log("state 4");
                clientAudioTester.playAnimation_conv(R(1021, 1023));  
                break;
            case 5:       //
                state = 5;
                text_add.SetActive(false);
                questionText.text = "";
                canQText.text = "질문해보세요.";
                waitQText.text = "말씀을 잘 들어보세요.";
                
                Debug.Log("state 5");
                switch (QuestionNumber)
                {
                    case 1007: //어린시절
                    case 1008:
                    case 1009:
                        QuestionNumber = R(1024, 1026);
                        clientAudioTester.playAnimation_conv(QuestionNumber);
                        break;
                    case 1010: //동학 
                    case 1011:
                        QuestionNumber = R(1027, 1028);
                        clientAudioTester.playAnimation_conv(QuestionNumber);
                        break;
                    case 1012: //치하포 
                    case 1013:
                    case 1014:
                        QuestionNumber = R(1029, 1030);
                        clientAudioTester.playAnimation_conv(QuestionNumber);
                        break;
                    case 1183: //독립운동
                    case 1184:
                    case 1185:
                        QuestionNumber = R(1195, 1197);
                        clientAudioTester.playAnimation_conv(QuestionNumber);
                        break;

                    case 1186: //백범일지
                    case 1187:
                    case 1188:
                        QuestionNumber = R(1198, 1200);
                        clientAudioTester.playAnimation_conv(QuestionNumber);
                        break;

                    case 1189: //통일운동
                    case 1190:
                    case 1191:
                        QuestionNumber = R(1201, 1203);
                        clientAudioTester.playAnimation_conv(QuestionNumber);
                        break;

                    case 1192: //서거
                    case 1193:
                    case 1194:
                        QuestionNumber = R(1204, 1206);
                        clientAudioTester.playAnimation_conv(QuestionNumber);
                        break;
                }
                
                break;
            case 6:       //질문 예시 
                state = 6;
                text_add.SetActive(true);
                questionText.text = "";
                question_back.SetActive(true);
                exampleText.text = "질문 예시";
                Debug.Log("state 6");
                if (QuestionNumber == 1024) gd.QuestionRef(1048, 1062);
                else if (QuestionNumber == 1025) gd.QuestionRef(1048, 1062);//출생
                else if (QuestionNumber == 1026) gd.QuestionRef(1048, 1062);//가족
                else if (QuestionNumber == 1027) gd.QuestionRef(1063, 1073);//공부
                else if (QuestionNumber == 1028) gd.QuestionRef(1063, 1073);//동학
                else if (QuestionNumber == 1029) gd.QuestionRef(1074, 1082);//치하포
                else if (QuestionNumber == 1030) gd.QuestionRef(1074, 1082);//치하포의거후
                else if (QuestionNumber == 1195) gd.QuestionRef(1143, 1157);//독립운동
                else if (QuestionNumber == 1196) gd.QuestionRef(1143, 1157);//독립운동
                else if (QuestionNumber == 1197) gd.QuestionRef(1143, 1157);//독립운동
                else if (QuestionNumber == 1198) gd.QuestionRef(1158, 1164);//백범일지
                else if (QuestionNumber == 1199) gd.QuestionRef(1158, 1164);//백범일지
                else if (QuestionNumber == 1200) gd.QuestionRef(1158, 1164);//백범일지
                else if (QuestionNumber == 1201) gd.QuestionRef(1165, 1173);//통일운동
                else if (QuestionNumber == 1202) gd.QuestionRef(1165, 1173);//통일운동
                else if (QuestionNumber == 1203) gd.QuestionRef(1165, 1173);//통일운동
                else if (QuestionNumber == 1204) gd.QuestionRef(1174, 1182);//서거
                else if (QuestionNumber == 1205) gd.QuestionRef(1174, 1182);//서거
                else if (QuestionNumber == 1206) gd.QuestionRef(1174, 1182);//서거
                InitMic();
                break;
            case 7:       //
                state = 7;
                text_add.SetActive(false);
                Debug.Log("state 7");
                Debug.Log(ResponseNumber);
                clientAudioTester.playAnimation_conv(ResponseNumber);

                /*for(int i = 1048; i <= 1142; i++)
                {
                    if (i == ResponseNumber)
                        isResponse = true;
                }
                if (isResponse == true) 
                { 
                    isResponse = false;
                }
                else 
                { 
                    clientAudioTester.playAnimation_conv(R(1037, 1044));
                    state--;
                }*/
                break;
            case 8:       //질문 루프
                state = 8;
                questionText.text = "";
                question_back.SetActive(true);
                exampleText.text = "질문 예시";
                //QuestionNumber = R(1034, 1036);
                gd.QuestionRef2(1,500);
                Debug.Log("state 8");
                //clientAudioTester.playAnimation_conv(R(1045, 1047));
                InitMic();
                break;
            case 9:       //답변루프
                state = 9;
                Debug.Log("state 9");
                clientAudioTester.playAnimation_conv(ResponseNumber);
                break;

            case 10:     //종료, 초기화
                state = 10;
                Debug.Log("state 10");
                clientAudioTester.playAnimation_conv(R(1045, 1047));
                loop1 = 0;
                loop2 = 0;
                break;

            case 100:       //답변루프
                state = 100;
                Debug.Log("state 100");
                clientAudioTester.playAnimation_conv(R(1037, 1044));
                break;
            default:
                break;
        }
    }
    public go_data gd;
     
    int R(int _s, int _e)
    {
        int i = UnityEngine.Random.Range(_s, _e + 1);
        Debug.Log(state + "    " + i);
        return i;
    }
     
    public void SetConvQuestion(int _i)
    {
        if (_i == 1)
        {

        }
        
    }
    void SetMicBlock()
    {
        if (micBlock)
        {
            micBlock = false;
            
        }
        else
        {
            //SetState(-1);
            micBlock = true;
            StopMicrophone();
        }
    }
    void CheckPeople()
    {
        if (Time.time > lastTime + amount) //1초마다 파일 체크
        {
            /*string localpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string path = localpath + "/state.txt";

            StreamReader reader = new StreamReader(path);
            string aa = reader.ReadLine();*/
            string aa = "1";
            //reader.Close();
            if (stateText != aa)
            {
                if (aa == "1")
                {
                    Debug.Log("사람 등장");
                    //micBlock = false;
                    //InitMic();
                    //SetMicBlock();
                    micBlock = false;
                    SetState(0);
                    people.SetActive(true);
                }
                else
                {
                    Debug.Log("퇴장");
                    //micBlock = true;
                    //SetMicBlock();
                    SetState(-1);
                    GameObject tmp = GameObject.Find("DF-Client");
                    tmp.GetComponent<DF2ClientAudioTester>().StopAllCoroutines();
                    tmp.GetComponent<DF2ClientAudioTester>().CancelInvoke();
                    loop1 = 0;
                    micBlock = true;
                    StopMicrophone();
                    canQText.text = " ";
                    waitQText.text = " ";
                    people.SetActive(false);
                }
                stateText = aa;
            }

            lastTime = Time.time;
        }
    }
    public void InitMic()
    {
        
            _device = Microphone.devices[0];

            _clipRecord = Microphone.Start(_device, false, 300, 44100);
            startRecordingTime = Time.time;
            Debug.Log("InitMic");
            animator.SetTrigger("stop");

            if (logtext) logtext.text = _device;
            _isInitialized = true;
            intervalTime = Time.time;
            micObj.SetColor("_BaseColor", Color.white);
            StartCoroutine(SetMic(1));
        
        waitQ.SetActive(false);
        canQ.SetActive(true);

    }

    public void StopMicrophone()
    {
        //Debug.Log("StopMicrophone");
        Microphone.End(_device);
        _isInitialized = false;

    }

    public void StartRecord()
    {
        //Debug.Log("StartRecord startTalkingTime");
        startTalkingTime = Time.time;
    }
    public string[] trigger;
    public string[] triggerN;
    public string[] triggerTalk;
    public void StopRecord()
    {
        //WaitingRecord.SetActive(false);
        //End the recording when the mouse comes back up, then play it
        StopMicrophone();
        _isInitialized = false;
        Debug.Log("StopRecord");
        offset = (int)((startTalkingTime - startRecordingTime - 1) * _clipRecord.frequency);
        if (offset < 0) offset = 0;
        duration = (int)((Time.time - startTalkingTime + 1) * _clipRecord.frequency);
        talkingTime = Time.time - startTalkingTime;
        //Trim the audioclip by the length of the recording
        AudioClip recordingNew = AudioClip.Create(_clipRecord.name + "_new", duration, _clipRecord.channels, _clipRecord.frequency, false);
        float[] data = new float[duration]; //버퍼
        _clipRecord.GetData(data, offset); //가져오기

        recordingNew.SetData(data, 0);   //저장
        StartCoroutine(SetMic(2));
        int r = UnityEngine.Random.Range(0, triggerN.Length);
        animator.SetTrigger(triggerN[r]);
        waitQ.SetActive(true);
        canQ.SetActive(false);
        clientAudioTester.SendAudio(recordingNew);

    }
    public void StartTalkingAni() //말할때 애니메이션 플레이
    {
        int r = UnityEngine.Random.Range(0, triggerTalk.Length);
        animator.SetTrigger(triggerTalk[r]);
    }
    public void StartTestAction() //테스트용 애니메이션 시작
    {
         
        animator.SetTrigger("test");
    }
    public IEnumerator SetMic(int _i)
    {
        iTween.Stop(gameObject);
        RotateHaf();
        yield return new WaitForSeconds(1f);
        mic.SetActive(false);
        cube.SetActive(false);
        speaker.SetActive(false);
        if (_i == 1)
        {
            mic.SetActive(true);
            indicator.rotateRootSpeed = 100;
        }
        else if (_i == 2)
        {
            cube.SetActive(true);
            indicator.rotateRootSpeed = 150;
        }
        else if (_i == 3)
        {
            speaker.SetActive(true);
            indicator.rotateRootSpeed = 50;
        }
    }
    public void RotateHaf()
    {
        haf.transform.rotation = Quaternion.identity;
        iTween.RotateAdd(haf, iTween.Hash("x", 360, "time", 1f, "easetype", "linear"   ));
    }

    float MicrophoneLevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(_device) - (_sampleWindow + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        if(waveData == null) return 0;
        _clipRecord.GetData(waveData, micPosition);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }
    
    //get data from microphone into audioclip
    float MicrophoneLevelMaxDecibels()
    {

        float db = 20 * Mathf.Log10(Mathf.Abs(MicLoudness)) ;

        return db;
    }

    public float FloatLinearOfClip(AudioClip clip)
    {
        StopMicrophone();

        _recordedClip = clip;

        float levelMax = 0;
        float[] waveData = new float[_recordedClip.samples];

        _recordedClip.GetData(waveData, 0);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _recordedClip.samples; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }

    public float DecibelsOfClip(AudioClip clip)
    {
        StopMicrophone();

        _recordedClip = clip;

        float levelMax = 0;
        float[] waveData = new float[_recordedClip.samples];

        _recordedClip.GetData(waveData, 0);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _recordedClip.samples; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }

        float db = 20 * Mathf.Log10(Mathf.Abs(levelMax));

        return db;
    }

    void OnEnable()
    {
        //InitMic();
        //_isInitialized = true;
        //Inctance = this;
    }

    //stop mic when loading a new level or quit application
    void OnDisable()
    {
        //StopMicrophone();
    }

    void OnDestroy()
    {
        StopMicrophone();
    }


    // make sure the mic gets started & stopped when application gets focused
    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            //Debug.Log("Focus");

            if (!_isInitialized)
            {
                //Debug.Log("Init Mic");
                //InitMic();
            }
        }
        if (!focus)
        {
            //Debug.Log("Pause");
            //StopMicrophone();
            //Debug.Log("Stop Mic");

        }
    }
}
