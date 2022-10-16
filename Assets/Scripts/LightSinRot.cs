using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSinRot : MonoBehaviour
{
    public float min;
    public float max;
    public float speed;
    public bool isRotate = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            isRotate = !isRotate;
        }
        if (isRotate)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Sin(Time.time * speed) * max, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, min  , 0));
        }
    }


}
