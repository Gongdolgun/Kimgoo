using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOnOff : MonoBehaviour
{
    public GameObject catlist;
    public GameObject[] catlistItem;
    public bool Active = false;
    public void SetAxtive()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        if (!Active)
        {
            iTween.MoveTo(catlist, iTween.Hash("x",  0, "islocal", true));

            Active = true;
        }
        else {
            
            iTween.MoveTo(catlist, iTween.Hash("x", 300, "islocal", true));
            Active = false;
        }
    }

     
}
