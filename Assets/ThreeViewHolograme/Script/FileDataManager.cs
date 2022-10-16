using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;
using System;


[Serializable]
public class GameObjectData
{
    [SerializeField]
    Transform cTf;

    public float camSize;

    public float positionX;
    public float positionY;
    public float positionZ;

    

    public void Setup()
    {
        SetPositionData();        
    }



    public void SetPositionData()
    {
        if (cTf) {
            float posX = cTf.position.x;
            float posY = cTf.position.y;
            float posZ = cTf.position.z;
        

        //posX = (float)(Mathf.Floor(posX * 10.0f)) / 10.0f;
        //posY = (float)(Mathf.Floor(posY * 10.0f)) / 10.0f;
        //posZ = (float)(Mathf.Floor(posZ * 10.0f)) / 10.0f;

        positionX = posX;
        positionY = posY;
        positionZ = posZ;

            //Debug.Log(posX + " " + posY + " " + posZ);
        }
    }

    public void SaveCameraSize()
    {
        if (cTf)  camSize = cTf.GetComponent<Camera>().orthographicSize;
    }


    public void SetCameraSize(float sizeValue)
    {
        if (cTf) cTf.GetComponent<Camera>().orthographicSize = sizeValue;
    }


    public void SetPosition(Vector3 dPos)
    {
        if(cTf) cTf.transform.position = dPos;
    }
    
}



[Serializable]
public class CanvasView
{
    [SerializeField]
    RectTransform canvasRectTf;

    public float viewAngleZ;

    public void SetAngleValueZ()
    {
        viewAngleZ = canvasRectTf.rotation.eulerAngles.z;
    }

    public void SetAngleZ(float valueZ)
    {
        canvasRectTf.eulerAngles = new Vector3(canvasRectTf.rotation.eulerAngles.x, canvasRectTf.rotation.eulerAngles.y, valueZ);
    }
}



[Serializable]
public class ObjectData
{
    public CanvasView canvasView;

    public GameObjectData[] cameraDatas;

    public float level;

    public int campos;

    public void PrintData()
    {
        for (int i = 0; i < cameraDatas.Length; i++)
        {
            Debug.Log(cameraDatas[i].positionX + " " + cameraDatas[i].positionY + " " + cameraDatas[i].positionZ);
        }
    }


    public void SetAngleValueZ()
    {
        canvasView.SetAngleValueZ();
    }


    public void SetCameraSizeData()
    {
        
    }


    public void SetPositionData(Vector3 pos)
    {
        for (int i = 0; i < cameraDatas.Length; i++)
        {
            cameraDatas[i].SetPosition(pos);
        }
    }
}




public class FileDataManager : MonoBehaviour
{
    [Header("카메라 위치 이동 저장")]
    [SerializeField]
    ObjectData oData;
    [SerializeField]
    UiManager uiManager;
    [SerializeField]
    Transform[] cameraTf;
    public int setCameraNum { get; set; }
    public Camera _CAM;
    public GameObject _CAMtransform;
    public GameObject helpUI;
    public float campos;
    private void Awake()
    {
        
        LoadData();
    }
    public  void SaveLevel(float _l)
    {
        oData.level = _l;
        SaveData();
    }
    public float LoadLevel()
    {
        return oData.level;
    }
    private void Start()
    {
        if(helpUI) helpUI.SetActive(false);
        if (PlayerPrefs.HasKey("campos"))
        {
            campos = PlayerPrefs.GetFloat("campos");
        }
        else
        {
            campos = 42;
        }
        if (campos == 42)
        {
            _CAMtransform.transform.localPosition = new Vector3(0, 1.6f, -3.014f);
            _CAM.fieldOfView = 42;
        }
        else
        {
            _CAMtransform.transform.localPosition = new Vector3(0, 1.75f, -3.014f);
            _CAM.fieldOfView = 29;
        }
        //uiManager.VisibleGuideLine(); 
    }
    void SetCampos()
    {
        Debug.Log(this.name);
        if (campos == 42)
        {
            _CAMtransform.transform.localPosition = new Vector3(0, 1.75f, -3.014f);
            _CAM.fieldOfView = 29;
            campos = 29;
        }
        else
        {
            _CAMtransform.transform.localPosition = new Vector3(0, 1.6f, -3.014f);
            _CAM.fieldOfView = 42;
            campos = 42;
        }
        PlayerPrefs.SetFloat("campos", campos);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) setCameraNum = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) setCameraNum = 1;
        if (Input.GetKey(KeyCode.DownArrow)) CameraMovePos(new Vector3(0, 0.001f, 0.0f));
        if (Input.GetKey(KeyCode.UpArrow)) CameraMovePos(new Vector3(0, -0.001f, 0.0f));
        if (Input.GetKey(KeyCode.LeftArrow)) CameraMovePos(new Vector3(0.0f, 0.0f, 0.001f));
        if (Input.GetKey(KeyCode.RightArrow)) CameraMovePos(new Vector3(0.0f, 0.0f, -0.001f));
        if (Input.GetKey(KeyCode.O)) CameraMovePos(new Vector3(0.001f, 0.0f, 0.0f));
        if (Input.GetKey(KeyCode.P)) CameraMovePos(new Vector3(-0.001f, 0.0f, 0.0f));
        if (Input.GetKeyDown(KeyCode.R)) uiManager.RotationAngle();//화면 회전하기.
        if (Input.GetKeyDown(KeyCode.M)) Cursor.visible = !Cursor.visible;//마우스 커서 보이고 숨기기.
        if (Input.GetKeyDown(KeyCode.G)) uiManager.VisibleGuideLine();//그리드 라인 보이고 숨기기.
        if (Input.GetKeyDown(KeyCode.S)) SaveData();//파일 데이터 저장하기.
        if (Input.GetKeyDown(KeyCode.L)) LoadData();//파일 데이터 불러오기.
        if (Input.GetKeyDown(KeyCode.X)) Application.Quit();//종료.
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetCampos();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (helpUI.activeSelf)
            {
                helpUI.SetActive(false);
            }
            else
            {
                helpUI.SetActive(true);
            }
        }
    }
    public void CameraMovePos(Vector3 mPos)
    {
        cameraTf[setCameraNum].position += mPos;
        if (setCameraNum == 1)
        {
            cameraTf[2].position = new Vector3(-cameraTf[1].position.x, cameraTf[1].position.y, cameraTf[1].position.z);
        }
        SaveData();
    }

    public void LoadData()
    {
        ObjectData loadData = LoadJsonFile<ObjectData>(Application.streamingAssetsPath, "ObjectData");

        for (int i = 0; i < loadData.cameraDatas.Length; i++)
        {
            float posX = loadData.cameraDatas[i].positionX;
            float posY = loadData.cameraDatas[i].positionY;
            float posZ = loadData.cameraDatas[i].positionZ;

            float camSize = loadData.cameraDatas[i].camSize;

            oData.cameraDatas[i].SetPosition(new Vector3(posX, posY, posZ));
            oData.cameraDatas[i].SetCameraSize(camSize);
        }

        //oData.canvasView.SetAngleZ(loadData.canvasView.viewAngleZ);
        oData.level = loadData.level;
        oData.campos = loadData.campos;
    }



    public void SaveData()
    {
        oData.SetAngleValueZ();

        for (int i = 0; i < oData.cameraDatas.Length; i++)
        {
            oData.cameraDatas[i].SetPositionData();
            oData.cameraDatas[i].SaveCameraSize();
        }

        CreateOnSaveData(oData);

    }



    /// <summary>
    /// 데이터를 저장한다.
    /// </summary>
    /// <param name="oData"></param>
    private void CreateOnSaveData(ObjectData oData)
    {
        CreateJsonFile(Application.streamingAssetsPath, "ObjectData", objectToJson(oData));
    }

    T JsonToObject<T>(string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }

    string objectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }


    T LoadJsonFile<T>(string loadPath, string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }
}
