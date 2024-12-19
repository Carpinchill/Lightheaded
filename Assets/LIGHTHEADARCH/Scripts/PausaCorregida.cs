using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausaCorregida : MonoBehaviour
{
    //BRUNO
    public GameObject MenuPause; 
    public CameraManager cameraManager; 
    private bool _isPaused = false; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    
    public void PauseGame()
    {
        MenuPause.SetActive(true); 
        _isPaused = true;

        Time.timeScale = 0; 

        Cursor.visible = true; 
        Cursor.lockState = CursorLockMode.None; 

        if (cameraManager != null)
        {
            cameraManager.enabled = false; 
        }
    }

    
    public void ResumeGame()
    {
        MenuPause.SetActive(false);
        _isPaused = false;

        Time.timeScale = 1; 

        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked; 

        if (cameraManager != null)
        {
            cameraManager.enabled = true; 
        }
    }
}
