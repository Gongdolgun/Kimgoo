using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllScript : MonoBehaviour
{

    public int setCameraNum { get; set; }

    [SerializeField]
    Transform[] cameraTf;
    [SerializeField]
    Camera[] contentCamera;
    public FileDataManager fileDataManager;
    public void CameraMovePos(Vector3 mPos)
    {
        cameraTf[setCameraNum - 1].position += mPos;
        cameraTf[2].position = new Vector3(-cameraTf[1].position.x, cameraTf[1].position.y, cameraTf[1].position.z);
        fileDataManager.SaveData();
    }

    public void CameraSize(float camSize)
    {
        contentCamera[setCameraNum - 1].orthographicSize += camSize;
        contentCamera[2].orthographicSize = contentCamera[1].orthographicSize;
        fileDataManager.SaveData();
    }

    public void ChangeCameraDisplay5(int num)
    {
        for (int i = 0; i < cameraTf.Length; i++)
        {
            Camera cam = cameraTf[i].GetComponent<Camera>();
            cam.targetDisplay = num;
        }
    }


}
