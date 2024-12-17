using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI objectiveText; // Referencia al texto
    public float displayDuration = 5f;   // Tiempo que se mostrará el texto

    void Start()
    {
        // Asegurarte de que el texto esté activo al iniciar
        if (objectiveText != null)
        {
            objectiveText.gameObject.SetActive(true);
            Invoke("HideObjectiveText", displayDuration); // Ocultar después del tiempo
        }
    }

    void HideObjectiveText()
    {
        if (objectiveText != null)
        {
            objectiveText.gameObject.SetActive(false); // Ocultar el texto
        }
    }

}
