using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AppManager : MonoBehaviour
{
    [SerializeField]
    CameraControllScript cameraControllScript;

    [SerializeField]
    FileDataManager fileDataManager;

    [SerializeField]
    UiManager uiManager;

    

    private void Awake()
    {
        fileDataManager.LoadData();
        Debug.Log(this.name);
    }

    void Start()
    {
        uiManager.VisibleGuideLine();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))cameraControllScript.setCameraNum = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2))cameraControllScript.setCameraNum = 2;
        if (Input.GetKey(KeyCode.DownArrow)) cameraControllScript.CameraMovePos(new Vector3(0, 0.01f, 0.0f));
        if(Input.GetKey(KeyCode.UpArrow))cameraControllScript.CameraMovePos(new Vector3(0, -0.01f, 0.0f));
        if(Input.GetKey(KeyCode.LeftArrow))cameraControllScript.CameraMovePos(new Vector3(0.0f, 0.0f, 0.01f));
        if(Input.GetKey(KeyCode.RightArrow))cameraControllScript.CameraMovePos(new Vector3(0.0f, 0.0f, -0.01f));
        if(Input.GetKey(KeyCode.O)) cameraControllScript.CameraMovePos(new Vector3( 0.01f, 0.0f, 0.0f)); 
        if (Input.GetKey(KeyCode.P))cameraControllScript.CameraMovePos(new Vector3(-0.01f, 0.0f, 0.0f));
        if(Input.GetKeyDown(KeyCode.R))uiManager.RotationAngle();//화면 회전하기.
        if(Input.GetKeyDown(KeyCode.M))Cursor.visible = !Cursor.visible;//마우스 커서 보이고 숨기기.
        if(Input.GetKeyDown(KeyCode.G))uiManager.VisibleGuideLine();//그리드 라인 보이고 숨기기.
        if(Input.GetKeyDown(KeyCode.S))fileDataManager.SaveData();//파일 데이터 저장하기.
        if(Input.GetKeyDown(KeyCode.L))fileDataManager.LoadData();//파일 데이터 불러오기.
        if (Input.GetKeyDown(KeyCode.X))Application.Quit();//종료.
        if (Input.GetKeyDown(KeyCode.H)){
             
        }
        
    }
}
