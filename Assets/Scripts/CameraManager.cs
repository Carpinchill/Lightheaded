using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public Transform target;               // Referencia al jugador
    public Vector3 normalOffset;           // Offset normal de la c�mara
    public Vector3 aimOffset;              // Offset al apuntar
    public Camera mainCamera;              // Referencia a la c�mara principal
    public float sensibilidad = 3f;        // Sensibilidad del mouse
    public float pitchMin = -40f;          // L�mite inferior del �ngulo vertical
    public float pitchMax = 60f;           // L�mite superior del �ngulo vertical
    [Range(0, 1)] public float lerpValue = 0.1f; // Valor de interpolaci�n para suavizar el movimiento
    public float zoomSpeed = 0.1f;         // Velocidad de cambio de zoom
    public Image Aim;
   

    private Vector3 _currentOffset;         // Offset actual de la c�mara
    private float _yaw = 0f;                // Rotaci�n horizontal
    private float _pitch = 0f;              // Rotaci�n vertical
    private bool _isAiming = false;         // Indicador de si est� apuntando
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor en el centro de la pantalla
        _yaw = transform.eulerAngles.y;           // Inicializar _yaw con la rotaci�n actual de la c�mara
        _pitch = transform.eulerAngles.x;         // Inicializar _pitch con la rotaci�n actual de la c�mara
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

        // Limitar el _pitch (rotaci�n vertical) para evitar �ngulos inc�modos
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

        // Crear la rotaci�n deseada basada en _yaw y _pitch
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);

        // Calcular la nueva posici�n deseada de la c�mara
        Vector3 desiredPosition = target.position + rotation * _currentOffset;
        
        // Aplicar la rotaci�n calculada
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, desiredPosition, lerpValue), rotation);

        // Detectar disparo con clic izquierdo solo si est� apuntando
        if (Input.GetMouseButtonDown(0) && _isAiming)
        {
            FireLightShard();
        }
    }

    // M�todo para disparar un raycast desde el centro de la pantalla
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
