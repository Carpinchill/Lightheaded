using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Salir : MonoBehaviour
{
    //FRANCO
    public void Jugar(string LightHead)
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Salir()
    {
        Application.Quit();
    }
    public void menu()
    {
        SceneManager.LoadScene(0);

    }
}
