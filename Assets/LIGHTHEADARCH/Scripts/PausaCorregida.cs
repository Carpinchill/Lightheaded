using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausaCorregida : MonoBehaviour
{
    public GameObject MenuPause; // Panel del men� de pausa
    public CameraManager cameraManager; // Script que controla la c�mara
    private bool isPaused = false; // Estado de pausa

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    // M�todo para pausar el juego
    public void PauseGame()
    {
        MenuPause.SetActive(true); // Activa el men� de pausa
        isPaused = true;

        Time.timeScale = 0; // Pausa la f�sica y el tiempo del juego

        Cursor.visible = true; // Habilita el cursor
        Cursor.lockState = CursorLockMode.None; // Desbloquea el cursor

        if (cameraManager != null)
        {
            cameraManager.enabled = false; // Desactiva el control de la c�mara
        }
    }

    // M�todo para reanudar el juego
    public void ResumeGame()
    {
        MenuPause.SetActive(false); // Oculta el men� de pausa
        isPaused = false;

        Time.timeScale = 1; // Reactiva la f�sica y el tiempo del juego

        Cursor.visible = false; // Oculta el cursor
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor al centro de la pantalla

        if (cameraManager != null)
        {
            cameraManager.enabled = true; // Reactiva el control de la c�mara
        }
    }
}
