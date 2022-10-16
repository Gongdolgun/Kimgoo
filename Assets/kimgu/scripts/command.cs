using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class command : MonoBehaviour
{
    public Animator animator;
    public AudioClip ac;
    public AudioClip ac1;
    public AudioClip ac2;
    public AudioClip ac3;
    public AudioSource audioSource;
    public void SetTrigger(string _s)
    {
        Debug.Log(_s);
        animator.SetTrigger(_s);
        ac =  Resources.Load(_s) as AudioClip;
        if(ac)audioSource.PlayOneShot(ac);

    }
}
