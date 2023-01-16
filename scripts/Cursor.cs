using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor : MonoBehaviour
{
    //Composantes fournis par Unity
    private AudioListener audio;
    private AudioSource source;
    void Start()
    {
        //barer le curseur et music de fond
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        source = GetComponent<AudioSource>();
        source.Play();
    }

}
