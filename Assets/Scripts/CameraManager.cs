using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public Transform target;               // Referencia al jugador
    public Vector3 normalOffset;           // Offset normal de la cámara
    public Vector3 aimOffset;              // Offset al apuntar
    public Camera mainCamera;              // Referencia a la cámara principal
    public float sensibilidad = 3f;        // Sensibilidad del mouse
    public float pitchMin = -40f;          // Límite inferior del ángulo vertical
    public float pitchMax = 60f;           // Límite superior del ángulo vertical
    [Range(0, 1)] public float lerpValue = 0.1f; // Valor de interpolación para suavizar el movimiento
    public float zoomSpeed = 0.1f;         // Velocidad de cambio de zoom
    public Image Aim;
   

    private Vector3 _currentOffset;         // Offset actual de la cámara
    private float _yaw = 0f;                // Rotación horizontal
    private float _pitch = 0f;              // Rotación vertical
    private bool _isAiming = false;         // Indicador de si está apuntando
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor en el centro de la pantalla
        _yaw = transform.eulerAngles.y;           // Inicializar _yaw con la rotación actual de la cámara
        _pitch = transform.eulerAngles.x;         // Inicializar _pitch con la rotación actual de la cámara
        _currentOffset = normalOffset;            // Comenzar con el offset normal
        Aim.enabled = false;
    }

    private void LateUpdate()
    {
        // Obtener la entrada del mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad;

        // Actualizar las rotaciones en base al input del mouse
        _yaw += mouseX;
        _pitch -= mouseY;

        // Limitar el _pitch (rotación vertical) para evitar ángulos incómodos
        _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);

        // Alternar entre el offset de zoom y el normal al hacer clic derecho
        if (Input.GetMouseButton(1)) // Clic derecho
        {
            _isAiming = true;
            _currentOffset = Vector3.Lerp(_currentOffset, aimOffset, zoomSpeed);
            Aim.enabled = true;
        }
        else
        {
            _isAiming = false;
            _currentOffset = Vector3.Lerp(_currentOffset, normalOffset, zoomSpeed);
            Aim.enabled = false;
        }

        // Crear la rotación deseada basada en _yaw y _pitch
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);

        // Calcular la nueva posición deseada de la cámara
        Vector3 desiredPosition = target.position + rotation * _currentOffset;
        
        // Aplicar la rotación calculada
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, desiredPosition, lerpValue), rotation);

        // Detectar disparo con clic izquierdo solo si está apuntando
        if (Input.GetMouseButtonDown(0) && _isAiming)
        {
            FireLightShard();
        }
    }

    // Método para disparar un raycast desde el centro de la pantalla
    private void FireLightShard()
    {
        // Crear un ray desde el centro de la pantalla
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // Verificar si el raycast golpea algo
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Golpeaste a: " + hit.collider.name);
        }
    }
}
