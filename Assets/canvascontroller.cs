using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvascontroller : MonoBehaviour
{
    public GameObject objectToActivate;  // Objeto a activar

    private void Start()
    {
        // Iniciar la coroutine para esperar 2 segundos y activar el objeto
        StartCoroutine(ActivateObject());
    }

    private IEnumerator ActivateObject()
    {
        // Esperar 2 segundos
        yield return new WaitForSeconds(2f);

        // Activar el objeto
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }
}
