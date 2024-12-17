using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI objectiveText; // Referencia al texto
    public float displayDuration = 5f;   // Tiempo que se mostrar� el texto

    void Start()
    {
        // Asegurarte de que el texto est� activo al iniciar
        if (objectiveText != null)
        {
            objectiveText.gameObject.SetActive(true);
            Invoke("HideObjectiveText", displayDuration); // Ocultar despu�s del tiempo
        }
    }

    void HideObjectiveText()
    {
        if (objectiveText != null)
        {
            objectiveText.gameObject.SetActive(false); // Ocultar el texto
 �������}
    }

}
