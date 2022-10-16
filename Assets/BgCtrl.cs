using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgCtrl : MonoBehaviour
{
    public GameObject front;
    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(600);
            if (front.activeSelf) front.SetActive(false);
            else front.SetActive(true);
        }
    }

     
}
