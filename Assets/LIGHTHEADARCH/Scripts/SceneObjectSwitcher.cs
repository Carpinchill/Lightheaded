using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using UnityEngine;

public class SceneObjectSwitcher : MonoBehaviour
{
    public GameObject objectToDeactivate; // El objeto que se desactivar�
    public GameObject objectToActivate;   // El objeto que se activar�
    public GameObject thirdObjectToActivate; // Un tercer objeto que tambi�n se activar�

    void Start()
    {
        StartCoroutine(SwitchObjects());
    }

    IEnumerator SwitchObjects()
    {
        yield return new WaitForSeconds(1f); // Espera 1 segundo

        // Desactiva el primer objeto
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }

        // Activa el segundo objeto
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        // Activa el tercer objeto
        if (thirdObjectToActivate != null)
        {
            thirdObjectToActivate.SetActive(true);
        }

        // Configuraci�n del cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
