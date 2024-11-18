using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPasos : MonoBehaviour
{
   
    public AudioSource pasos;

    private bool Hactivo;
    private bool Vactivo;

    private void Update()
    {
        if(Input.GetButtonDown("Horizontal"))
        {
            Hactivo = true;
            pasos.Play();
        }
        if (Input.GetButtonDown("Vertical"))
        {
            Vactivo = true;
            pasos.Play();
        }
        if(Input.GetButtonUp("Horizontal"))
        {
            Hactivo = false;
            if (Vactivo == false)
            {
                pasos.Pause();
            }
            
        }
        if (Input.GetButtonUp("Vertical"))
        {
            Vactivo = false;
            if(Hactivo == false)
            {
                pasos.Pause();
            }
            
        }
    }

}
