using System.Collections;
using System;
using UnityEngine;
using Ehd.Launcher.Common;
using Ehd.Launcher.SDK;
using Ehd.Launcher.SDK.Json;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [Header("서버에 로그 전송")]
    private Ehd.Launcher.SDK.ContentsAPI m_contensAPI = new ContentsAPI();

    private string[] arguments = new string[3];
    private string sessionID;
    private string authCode;
    private string hash;
    public Text mytext;
    private string serverAuthReturn;

    private void Awake()
    {
#if !UNITY_EDITOR
        arguments = Environment.GetCommandLineArgs();

        if (arguments.Length > 3)
        {
            sessionID = ExtractValue(arguments[1]);
            authCode = ExtractValue(arguments[2]);
            hash = ExtractValue(arguments[3]);
        }
#else
        arguments[0] = "session_id:70ea5c8e757d11ebaff8005056af049a";
        arguments[1] = "auth_code:cnt000000001";
        arguments[2] = "hash:bf85b2d5ad33e360485bbabdcad1683d";

        sessionID = ExtractValue(arguments[0]);
        authCode = ExtractValue(arguments[1]);
        hash = ExtractValue(arguments[2]);
#endif

    }

    private void Start()
    {
        //StartCoroutine(TestLaucherSDK());
        StartCoroutine(s02());
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            StartCoroutine(s01());
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            StartCoroutine(s02());
        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            StartCoroutine(s03());
        }
        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            StartCoroutine(s07());
        }
    }
    private void OnApplicationQuit()
    {
        StartCoroutine(s07());
    }
    private void OnApplicationPause(bool pause)
    {

    }

    public IEnumerator s01() //컨텐츠 시작
    {
        yield return StartCoroutine(AuthContents());
    }
    public IEnumerator s02() //컨텐츠 시작
    {
        yield return StartCoroutine(AuthContents());
        yield return StartCoroutine(LogStartContents());
    }
    public IEnumerator s03() //종료
    {
        yield return StartCoroutine(AuthContents());
        yield return StartCoroutine(LogStopContents());
    }
    public void s04() //중지
    {

    }
    public void s05() //재시작
    {

    }
    public void s06() //플레이초기화
    {

    }
    public IEnumerator s07() //플레이오류
    {
        yield return StartCoroutine(AuthContents());
        yield return StartCoroutine(LogErrorContents("error_s", "error_d"));
    }





    private IEnumerator TestLaucherSDK()
    {
        yield return StartCoroutine(AuthContents());

        yield return StartCoroutine(LogStartContents());

        yield return StartCoroutine(LogStopContents());
    }
    private string ExtractValue(string _argVal)
    {
        string result = string.Empty;

        string[] tempStr = _argVal.Split(':');

        result = tempStr[1];

        return result;
    }

    public IEnumerator AuthContents()
    {
        string jsonVal = string.Empty;

        serverAuthReturn = m_contensAPI.S0001(authCode, sessionID, hash);

        while (serverAuthReturn == string.Empty)
        {
            yield return null;
        }

        S0000RS srVal = JsonUtil.Deserialize<S0000RS>(serverAuthReturn);

        if (srVal.code == "0000")
        {
            // 인증 성공
            Debug.Log("인증 성공입니다.");
            mytext.text = "인증 성공입니다." + Time.time;
        }
        else
        {
            if (arguments.Length > 3)
            {
                string log = string.Format("authCode: {0}, sessionID: {1}, hash: {2}", arguments[1], arguments[2], arguments[3]);
                Debug.Log("인증 오류 입니다.: " + Time.time);
            }
        }
    }

    public IEnumerator LogStartContents()
    {
        string jsonVal = string.Empty;
        jsonVal = m_contensAPI.S0002(serverAuthReturn);
        while (jsonVal == string.Empty)
        {
            yield return null;
        }

        S0000RS srVal = JsonUtil.Deserialize<S0000RS>(jsonVal);

        if (srVal.code != "0000")
        {
            Debug.LogError("시작 로그 오류");
        }
        else
        {
            Debug.Log("시작 로그 보내기 성공입니다.");
            mytext.text = "시작 로그 보내기 성공입니다." + Time.time;
        }
    }

    public IEnumerator LogStopContents()
    {
        string jsonVal = string.Empty;
        jsonVal = m_contensAPI.S0003(serverAuthReturn);
        while (jsonVal == string.Empty)
        {
            yield return null;
        }

        S0000RS srVal = JsonUtil.Deserialize<S0000RS>(jsonVal);

        if (srVal.code != "0000")
        {
            Debug.LogError("종료 로그 오류");
        }
        else
        {
            Debug.Log("종료 로그 보내기 성공입니다.");
            mytext.text = "종료 로그 보내기 성공입니다." + Time.time;
        }
    }

    public IEnumerator LogErrorContents(string _error_s, string _error_d)
    {
        //auth_code
        //expc_id
        //expc_seq
        //error_s
        //error_d

        string jsonVal = string.Empty;
        jsonVal = m_contensAPI.S0007(serverAuthReturn, _error_s, _error_d);
        while (jsonVal == string.Empty)
        {
            yield return null;
        }

        S0000RS srVal = JsonUtil.Deserialize<S0000RS>(jsonVal);

        if (srVal.code != "0000")
        {
            Debug.LogError("시작 로그 오류");
        }
        else
        {
            Debug.Log("에러 보내기 성공입니다.");
            mytext.text = "에러 보내기 성공입니다." + Time.time;
        }
    }
}
