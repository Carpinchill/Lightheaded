using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerBody;  // Referencia al cuerpo del personaje
    public Vector3 offset;  // Distancia de la c�mara al personaje
    public float mouseSensitivity = 100f;
    public float smoothTime = 0.1f;  // Para suavizar el movimiento de la c�mara

    private float xRotation = 0f;  // Controla la rotaci�n en el eje X (vertical)
    private Vector3 currentVelocity;  // Vector3 para la velocidad del suavizado

    void Start()
    {
        // Bloquear el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        // Obtener el movimiento del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotaci�n vertical
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 60f);  // Limitar la rotaci�n vertical de la c�mara

        // Rotar la c�mara alrededor del personaje en el eje Y
        playerBody.Rotate(Vector3.up * mouseX);

        // Posicionar la c�mara detr�s del jugador con un suavizado
        Vector3 desiredPosition = playerBody.position - playerBody.forward * offset.z + Vector3.up * offset.y;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);

        // Rotar la c�mara seg�n la rotaci�n vertical
        transform.LookAt(playerBody.position + Vector3.up * offset.y);
    }



}
