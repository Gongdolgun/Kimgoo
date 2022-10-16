using System;
using System.Collections.Generic;
using UnityEngine;
//using Newtonsoft.Json;
using Syrus.Plugins.DFV2Client;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DF2ClientAudioTester : MonoBehaviour
{
	public InputField session, content;
	public Text chatbotText;
	private DialogFlowV2Client client;
	public AudioClip testClip;
	public AudioSource audioPlayer;
	public Text LangueButtonText;
	private string languageCode = "ko-KR";
	private bool isEnglish = false;
	public GameObject WaitingPanel;
	public GameObject WaitingRecord;
	public go_controller go_Controller;
	public UIOnOff uIOnOff;

	void Start()
    {
		sceneStartTime = Time.time;
		client = GetComponent<DialogFlowV2Client>();

		audioPlayer = GetComponent<AudioSource>();
		
        // Adjustes session name if it is blank.
        string sessionName = GetSessionName();

        client.ChatbotResponded += LogResponseText;
		client.DetectIntentError += LogError;
		client.ReactToContext("DefaultWelcomeIntent-followup", 
			context => Debug.Log("Reacting to welcome followup"));
		client.SessionCleared += sess => Debug.Log("Cleared session " + session);
		client.AddInputContext(new DF2Context("userdata", 1, ("name", "George")), sessionName);

		Dictionary<string, object> parameters = new Dictionary<string, object>()
		{
			{ "name", "George" }
		};
		//client.DetectIntentFromEvent("test-inputcontexts", parameters, sessionName);
		if(animatorOverrideController) animator.runtimeAnimatorController = animatorOverrideController;
		//WaitingPanel.SetActive(false);
		//WaitingRecord.SetActive(false);
		//StartRecord();

	}

    public void OnChangeLanguageButton()
    {
	    isEnglish = !isEnglish;

	    if (isEnglish)
	    {
		    languageCode = "en-US";
		    LangueButtonText.text = "English";
	    }
	    else
	    {
		    languageCode = "ko-KR";
		    LangueButtonText.text = "Korean";
	    }
    }
 

    private void LogResponseText(DF2Response response)
	{
		//WaitingPanel.SetActive(false);
		//Debug.Log(JsonConvert.SerializeObject(response, Formatting.Indented));
		
		//Debug.Log("Audio " + response.OutputAudio);
		chatbotText.text = response.queryResult.queryText +"\n";
		chatbotText.text += response.queryResult.fulfillmentText;
		StartCoroutine(SetFinish(response.queryResult.queryText, response.queryResult.fulfillmentText, "1001"));
		//go_Controller.PlayAnswer( int.Parse(response.queryResult.fulfillmentText));
		if (response.queryResult.fulfillmentText != null)
		{
			Debug.Log(response.queryResult.fulfillmentText);
			if (response.queryResult.fulfillmentText == "10002")
			{
				Debug.Log(GetSessionName() + " said: \"" + response.queryResult.fulfillmentText + "\"");
				uIOnOff.SetAxtive();
				micInput.InitMic();
				Debug.Log(" null");
			}//메뉴열기
			else
			{
				//Debug.Log(GetSessionName() + " said: \"" + response.queryResult.fulfillmentText + "\"");
				micInput.ResponseControl(int.Parse(response.queryResult.fulfillmentText));
				//playAnimation(int.Parse(response.queryResult.fulfillmentText));
			}
		}
		else
		{
			micInput.ResponseControl();
			//micInput.InitMic();
			Debug.Log(" null");
		}
	}
	IEnumerator SetFinish(string _s, string _s2, string _s3)
	{
		yield return new WaitForEndOfFrame();
		WWWForm form = new WWWForm();
		form.AddField("name", "1"); // no
		if (_s == null) _s = "";
		form.AddField("email", _s); // type
		form.AddField("pass", "1234");
		form.AddField("title", "symId");
		form.AddField("content", "");
		if (_s2 == null) _s2 = "";
		form.AddField("score", _s2);
		form.AddField("avg", "");
		form.AddField("catg", _s3);
		UnityWebRequest uur = UnityWebRequest.Post("http://lkiop124.cafe24.com/board_kimkoo/insert.php", form);


		yield return uur.SendWebRequest();

		if (uur.isNetworkError)
		{
			Debug.Log(uur.error);
		}
		else
		{
			Debug.Log(uur.responseCode);
			Debug.Log(uur.downloadHandler.text);
			//myRank = uur.downloadHandler.text;
			if (uur.responseCode == 200)
			{
			}

		}
	}
	public Animator animator;
	public AnimatorOverrideController animatorOverrideController;
	public float delay;
	public AudioSource audioSource;
	public int frame;
	public float a;
	public go_data gd;
	public float offset;
	public MicInput micInput;
	public lerp _lerp;
	public bool test = false;
	private float sceneStartTime = 0;
	public Text timetext;
	public void PlayNumber()
	{
		playAnimation(int.Parse(content.text));
	}
	public void playAnimation(int _i)
	{

		StartCoroutine(micInput.SetMic(3));
		micInput.StartTalkingAni();
		string s = "k" + _i.ToString("0000");
		a = gd.getFrame(_i.ToString()) + offset;
		Debug.Log("a1   " + Time.time);
		int f = (int)(_i / 100f);
		Debug.Log("ani/" + f + "/" + _i);

		ResourceRequest request = Resources.LoadAsync("ani/" +f+"/"+ _i);
		AnimationClip animClip = request.asset as AnimationClip;

		AudioClip ac = Resources.Load("mp3/" + s) as AudioClip;

		if (animClip != null && ac != null)
		{
			Debug.Log("a2   " + Time.time);
			animatorOverrideController["A"] = animClip;
			audioSource.clip = ac;
			audioSource.time = a / 24f;
			Debug.Log("a   " + a);

			StartCoroutine(startMic(ac.length - (a / 24f)));
		}
		else
		{
			micInput.InitMic();
			Debug.Log(" null, ac , ani");
		}
			
		
	}
	IEnumerator startMic(float _t)
	{
		//yield return new WaitForSeconds(1f);
		animator.SetTrigger("shot");
		audioSource.Play();
		//Debug.Log("recordingNew.length   " + _t);
		yield return new WaitForSeconds(_t);
		micInput.InitMic();

	}
	 
	public void playAnimation_conv(int _i)
	{

		StartCoroutine(micInput.SetMic(3));
		micInput.StartTalkingAni();
		string s = "k" + _i.ToString("0000");
		a = gd.getFrame(_i.ToString()) + offset;
		//Debug.Log("a1   " + Time.time);
		int f = (int)(_i / 100f);
		Debug.Log("ani/" + f + "/" + _i + "   " + s);

		ResourceRequest request = Resources.LoadAsync("ani/" + f + "/" + _i);
		AnimationClip animClip = request.asset as AnimationClip;

		AudioClip ac = Resources.Load("mp3/" + s) as AudioClip;

		if (animClip != null && ac != null)
		{
			//Debug.Log("a2   " + Time.time);
			animatorOverrideController["A"] = animClip;
			audioSource.clip = ac;
			audioSource.time = a / 24f;
			//Debug.Log("a   " + a);
			if(micInput.state != -1) StartCoroutine(startMic_conv(ac.length - (a / 24f)));
		}
		else
		{
			//micInput.InitMic();
			//Debug.Log(" null, ac , ani");
		}


	}
	IEnumerator startMic_conv(float _t)
	{
		//yield return new WaitForSeconds(1f);
		animator.SetTrigger("shot");
		audioSource.Play();
		//Debug.Log("recordingNew.length   " + _t);
		yield return new WaitForSeconds(_t);

		if (micInput.state != -1)
		{
			if(micInput.state ==7 && micInput.loop1 == 4)
            {
				Invoke("mSetstate", 1f);
            }				
			else if(micInput.state == 10)
            {
				Invoke("mSetstate", 3f);	//state = 10(순회가 끝나면) 3초 후에 재시작
            }

			else micInput.SetState();
		}
		//micInput.InitMic();

	}

	public void mSetstate()
    {
		micInput.SetState();
    }


	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Y))
		{
			micInput.StartTestAction();
		}
		
		timetext.text = Time.time.ToString();
		 
	}

	private void LogError(DF2ErrorResponse errorResponse)
	{
		//WaitingPanel.SetActive(false);
		Debug.LogError(string.Format("Error {0}: {1}", errorResponse.error.code.ToString(), 
			errorResponse.error.message));
	}

	//@hoatong
	public void SendAudio(string audioString)
	{
		//WaitingPanel.SetActive(true);
		string sessionName = GetSessionName();
		client.DetectIntentFromAudio(audioString, sessionName , languageCode);
	}

    
	public void SendText()
	{
        string sessionName = GetSessionName();
		client.DetectIntentFromText(content.text, sessionName, languageCode);
	}


	public void SendEvent()
	{
        client.DetectIntentFromEvent(content.text,
			new Dictionary<string, object>(), GetSessionName());
	}

	public void Clear()
	{
        client.ClearSession(GetSessionName());
	}


    private string GetSessionName(string defaultFallback = "DefaultSession")
    {
        string sessionName = session.text;
        if (sessionName.Trim().Length == 0)
            sessionName = defaultFallback;
        return sessionName;
    }

    #region AUDIO RECORD

    AudioClip recordedAudioClip;

    //Keep this one as a global variable (outside the functions) too and use GetComponent during start to save resources
    //AudioSource audioSource;
    
    private float startRecordingTime;

    private bool isRecording = false;

    public Text recordButtonText;
    
    public void OnButtonRecord()
    {
	    if (!isRecording)
	    {
		    StartRecord();
		    isRecording = true;
		    recordButtonText.text = "Stop Record";
	    }
	    else
	    {
		    isRecording = false;
		    recordButtonText.text = "Start Record";
		    AudioClip recorded =  StopRecord();
		    
		    byte[] audioBytes = WavUtility.FromAudioClip(recorded);
		    string audioString = Convert.ToBase64String (audioBytes);
		    SendAudio(audioString);
	    }
    }
	public void SendAudio( AudioClip _clip)
	{
		 

			byte[] audioBytes = WavUtility.FromAudioClip(_clip);
			string audioString = Convert.ToBase64String(audioBytes);
			SendAudio(audioString);
		 
	}

	public int devices = 0;
	public string _device;
	 
	public void StartRecord()
    {
		if (_device == "")
		{
			_device = Microphone.devices[devices];
			Debug.Log("StartRecord  df2 " + _device);
		}
		//WaitingRecord.SetActive(true);
	    //Get the max frequency of a microphone, if it's less than 44100 record at the max frequency, else record at 44100
	    int minFreq;
	    int maxFreq;
	    int freq = 44100;
	    Microphone.GetDeviceCaps(_device, out minFreq, out maxFreq);
	    if (maxFreq < 44100)
		    freq = maxFreq;

		

		//Start the recording, the length of 300 gives it a cap of 5 minutes
		recordedAudioClip = Microphone.Start(_device, false, 300, 44100);
	    startRecordingTime = Time.time;
		isRecording = true;
	}
	public AudioClip StopRecord()
    {
		isRecording = false;
		//WaitingRecord.SetActive(false);
		//End the recording when the mouse comes back up, then play it
		Microphone.End(_device);
		Debug.Log("StopRecord  df2 " + _device);
		//Trim the audioclip by the length of the recording
		AudioClip recordingNew = AudioClip.Create(recordedAudioClip.name,
		    (int) ((Time.time - startRecordingTime) * recordedAudioClip.frequency), recordedAudioClip.channels,
		    recordedAudioClip.frequency, false);
	    float[] data = new float[(int) ((Time.time - startRecordingTime + 1) * recordedAudioClip.frequency)];
	    recordedAudioClip.GetData(data, 0);
	    recordingNew.SetData(data, 0);
	    this.recordedAudioClip = recordingNew;

		return recordedAudioClip;
    }
    #endregion
}
