using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    RectTransform cameraRectTransform;

    [SerializeField]
    GameObject guideLineGo;


    [SerializeField]
    Text targetCameraNumText;

    public bool isKiosk;
    
    public void Update()
    {
        
    }
    public void VisibleGuideLine()
    {
        guideLineGo.SetActive(!guideLineGo.activeSelf);
        Cursor.visible = !Cursor.visible;
    }


    public void SetTargetCameraNum(string camNum)
    {
        targetCameraNumText.text = camNum;
    }

    public void RotationAngle()
    {
        if (cameraRectTransform.rotation.z == 180)
        {

        }
        else if (cameraRectTransform.rotation.z == 0)
        {

        }
        //if (isKiosk)
        //    cameraRectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        //else 
        //    cameraRectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));


    }

}
