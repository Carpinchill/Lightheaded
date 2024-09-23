using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerBody;  // Referencia al cuerpo del personaje
    public Vector3 offset;  // Distancia de la cámara al personaje
    public float mouseSensitivity = 100f;
    public float smoothTime = 0.1f;  // Para suavizar el movimiento de la cámara

    private float xRotation = 0f;  // Controla la rotación en el eje X (vertical)
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

        // Rotación vertical
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 60f);  // Limitar la rotación vertical de la cámara

        // Rotar la cámara alrededor del personaje en el eje Y
        playerBody.Rotate(Vector3.up * mouseX);

        // Posicionar la cámara detrás del jugador con un suavizado
        Vector3 desiredPosition = playerBody.position - playerBody.forward * offset.z + Vector3.up * offset.y;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);

        // Rotar la cámara según la rotación vertical
        transform.LookAt(playerBody.position + Vector3.up * offset.y);
    }



}
