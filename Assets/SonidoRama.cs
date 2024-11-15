using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoRama : MonoBehaviour
{
    public AudioSource rama;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            rama.Play();
        }
    }
}
