using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class log : MonoBehaviour
{
    public float v;
    public float d;
    public float r;
    public Transform t;
    public Transform cube;
    public Material _light;
    public Image gage1;
    public Image gage2;
    public lerp myLerp;
    private Vector3 oldPos;
    private Vector3 newPos;
    void Update()
    {
        v = MicInput.MicLoudness  ;
        d =  Mathf.Clamp( MicInput.MicLoudnessinDecibels  + 100, 0, 1000) * 0.01f;
         
        t.localPosition = new Vector3(0,   d    , 0);
        //cube.localScale = Vector3.one * 7 * (d+1);
        if(myLerp.isLoud) _light.SetFloat("_EmissiveExposureWeight",   0);
        else _light.SetFloat("_EmissiveExposureWeight", 1);
        newPos = Vector3.Lerp(t.localPosition, oldPos, .5f);
        gage1.fillAmount = newPos.y / 2;
        gage2.fillAmount = newPos.y / 2;
        oldPos = newPos;

    }
}
