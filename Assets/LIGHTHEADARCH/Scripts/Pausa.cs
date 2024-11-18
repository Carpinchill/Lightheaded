using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausa : MonoBehaviour
{
    public GameObject MenuPause;
    public bool Pause = false;
    // Start is called before the first frame update}
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Pause==false)
            {
                MenuPause.SetActive(true);
                Pause = true;

                Time.timeScale = 0;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Pause == true)
            {
                Reload();
            }
        }

        
    }

    public void Reload()
    {
        MenuPause.SetActive(false);
        Pause = false;

        Time.timeScale = 1;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
