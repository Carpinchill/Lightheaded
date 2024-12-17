using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGenerator : MonoBehaviour
{
    public GameObject interactionCanvas; // Referencia al Canvas
    public float detectionRadius = 20f;  // Radio de detecci�n
    private Transform player;           // Referencia al jugador

    void Start()
    {
        // Busca al jugador autom�ticamente
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Aseg�rate de que el Canvas est� desactivado al iniciar
        if (interactionCanvas != null)
        {
            interactionCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // Verificar la distancia entre el jugador y el generador
        if (player != null && interactionCanvas != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);

            // Mostrar el Canvas si el jugador est� cerca
            if (distance <= detectionRadius)
            {
                interactionCanvas.SetActive(true);

                // Opcional: Hacer que el texto mire hacia la c�mara
                interactionCanvas.transform.LookAt(Camera.main.transform);
            }
            else
            {
                interactionCanvas.SetActive(false);
            }
        }
    }

}
