using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class lerp : MonoBehaviour
{
    public Transform t;
    public bool isTalking;
    public bool isLoud;
    public float zerotime = 0;
    public float uptime = 0;
    public float zeroTolerance = 1f;
    public float upTolerance = 1f;
    public MicInput micInput;
    public GameObject micObj;
    
    public Text questionText;
    public float interval;
    public float last_change_time;
    public go_data gd;
    public Text logtext;
    public Text StateAmount;
    public float level ;
    float logtime; //화면에 볼륨로그 표시
    float logtime_delay = 3; //화면에 볼륨로그 사라지는 딜레이
    [SerializeField]
    FileDataManager fileDataManager;
     
    private void Start()
    {
        //gd.QuestionRef();
        level = fileDataManager.LoadLevel( );
    }
     

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            micInput.amount++;
            StateAmount.text = "level = " + micInput.amount.ToString();
            fileDataManager.SaveLevel(level);
            PlayerPrefs.SetFloat("amount", micInput.amount);
        }

        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (micInput.amount == 1)
                return;
            micInput.amount--;
            StateAmount.text = "level = " + micInput.amount.ToString();
            fileDataManager.SaveLevel(level);
            PlayerPrefs.SetFloat("amount", micInput.amount);
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            level = level + .01f;
            logtext.text = "level = " + level.ToString();
            logtime = Time.time;
             
            fileDataManager.SaveLevel(level);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            level = level - .01f;
            logtext.text = "level = " + level.ToString();
            logtime = Time.time;
             
            fileDataManager.SaveLevel(level);
        }
        if (logtime_delay + logtime < Time.time)
        {
            logtext.enabled = false;
        }
        else
        {
            logtext.enabled = true;
        }
        if (interval + last_change_time < Time.time && micInput._isInitialized)
        {
            //if (!isLoud) gd.QuestionRef();
            last_change_time = Time.time;
        }
        transform.position = Vector3.Lerp(transform.position, t.position, .1f);
        if (transform.position.y > level)
        {
            //micObj.transform.localScale = Vector3.one * 1.2f;
            if (!isLoud) //소리 들어올때
            {
                isLoud = true;
                
                if (!isTalking) //말안할때
                {
                    isTalking = true;
                    
                    if (micInput)
                    {
                        uptime = Time.time;
                        micInput.StartRecord();
                        Debug.Log("start talking StartRecord");
                    }
                }
                else
                {
                    //Debug.Log("continue talking");
                }   
            }
            
        }
        else
        {
            //if (micObj) micObj.transform.localScale = Vector3.one  ;
            if (isLoud)
            {
                isLoud = false;
                zerotime = Time.time;
            }
            else
            {
                if (Time.time - zerotime > zeroTolerance && isTalking)
                {
                    Debug.Log("stop talking");
                    isTalking = false;
                    if (Time.time - uptime > upTolerance)
                    {
                        if (micInput) micInput.StopRecord();
                        //Debug.Log("recorded    lerp    " + (Time.time - uptime));
                    }
                    else
                    {

                    }
                }
            }          
            
        }
    }
}
