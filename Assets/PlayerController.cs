using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform playerBody;  // El cuerpo del personaje que girar� con la c�mara
    public float mouseSensitivity = 100f;  // Sensibilidad del mouse
    public Transform cameraTransform;  // Transform de la c�mara
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

        // Rotar la c�mara en el eje Y (vertical)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Limitar la rotaci�n vertical

        // Aplicar la rotaci�n vertical a la c�mara
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotar el personaje en la direcci�n de la c�mara
        float targetYRotation = cameraTransform.eulerAngles.y;  // Usar la rotaci�n Y de la c�mara
        playerBody.rotation = Quaternion.Euler(0f, targetYRotation, 0f);

        // (Opcional) Puedes mantener el control del mouseX si a�n quieres alguna rotaci�n adicional del cuerpo:
        // playerBody.Rotate(Vector3.up * mouseX);
����}


}
