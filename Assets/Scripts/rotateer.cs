using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateer : MonoBehaviour
{
    public GameObject kim;
    bool isRotate = false;
    public float speed = 1;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            isRotate = !isRotate;

        }
        if (isRotate)
        {
            kim.transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
        }
        else
        {
            kim.transform.localRotation = Quaternion.Euler(new Vector3(0,180,0)); 
        }
    }
}
