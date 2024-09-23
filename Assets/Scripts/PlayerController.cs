using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 offset;               // Desplazamiento de la c�mara
    private Transform target;             // Referencia al jugador
    [Range(0, 1)] public float lerpValue; // Valor de interpolaci�n
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

        // Rotar el offset en funci�n del movimiento del mouse
        offset = Quaternion.AngleAxis(mouseX, Vector3.up) * offset;

        // Calcular la nueva posici�n deseada
        Vector3 desiredPosition = target.position + offset;

        // Suavizar el movimiento de la c�mara
        transform.position = Vector3.Lerp(transform.position, desiredPosition, lerpValue);

        // Mirar hacia el jugador
        transform.LookAt(target);
    }
}
