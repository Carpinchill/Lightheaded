using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 offset;               // Desplazamiento de la cámara
    private Transform target;             // Referencia al jugador
    [Range(0, 1)] public float lerpValue; // Valor de interpolación
    public float sensibilidad;            // Sensibilidad del mouse

    private void Start()
    {
        target = GameObject.Find("Player").transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        // Obtener la entrada del mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad;

        // Rotar el offset en función del movimiento del mouse
        offset = Quaternion.AngleAxis(mouseX, Vector3.up) * offset;

        // Calcular la nueva posición deseada
        Vector3 desiredPosition = target.position + offset;

        // Suavizar el movimiento de la cámara
        transform.position = Vector3.Lerp(transform.position, desiredPosition, lerpValue);

        // Mirar hacia el jugador
        transform.LookAt(target);
    }
}
