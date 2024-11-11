using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Salir : MonoBehaviour
{
    public void Jugar(string LightHead)
    {
        SceneManager.LoadScene(LightHead);
    }
    public void Salir()
    {
        Application.Quit();
    }

}
