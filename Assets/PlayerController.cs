using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform playerBody;  // El cuerpo del personaje que girará con la cámara
    public float mouseSensitivity = 100f;  // Sensibilidad del mouse
    public Transform cameraTransform;  // Transform de la cámara
    private float xRotation = 0f;

    void Start()
    {
        // Bloquear el cursor en el centro de la pantalla y ocultarlo
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Obtener el movimiento del mouse en los ejes X e Y
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotar la cámara en el eje Y (vertical)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Limitar la rotación vertical

        // Aplicar la rotación vertical a la cámara
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotar el personaje en la dirección de la cámara
        float targetYRotation = cameraTransform.eulerAngles.y;  // Usar la rotación Y de la cámara
        playerBody.rotation = Quaternion.Euler(0f, targetYRotation, 0f);

        // (Opcional) Puedes mantener el control del mouseX si aún quieres alguna rotación adicional del cuerpo:
        // playerBody.Rotate(Vector3.up * mouseX);
    }


}
