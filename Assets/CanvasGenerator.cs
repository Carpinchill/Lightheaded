using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGenerator : MonoBehaviour
{
    public GameObject interactionCanvas; // Referencia al Canvas
    public float detectionRadius = 20f;  // Radio de detección
    private Transform player;           // Referencia al jugador

    void Start()
    {
        // Busca al jugador automáticamente
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Asegúrate de que el Canvas esté desactivado al iniciar
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

            // Mostrar el Canvas si el jugador está cerca
            if (distance <= detectionRadius)
            {
                interactionCanvas.SetActive(true);

                // Opcional: Hacer que el texto mire hacia la cámara
                interactionCanvas.transform.LookAt(Camera.main.transform);
            }
            else
            {
                interactionCanvas.SetActive(false);
            }
        }
    }

}
