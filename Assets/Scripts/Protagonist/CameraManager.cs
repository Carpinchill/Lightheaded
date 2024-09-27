using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public Transform target;               //referencia al jugador
    public Vector3 normalOffset;           //offset normal de la c�mara
    public Vector3 aimOffset;              //offset al apuntar
    public Camera mainCamera;              //referencia a la c�mara principal
    public float sensibilidad = 3f;        //sensibilidad del mouse
    public float pitchMin = -40f;          //l�mite inferior del �ngulo vertical
    public float pitchMax = 60f;           //l�mite superior del �ngulo vertical
    [Range(0, 1)] public float lerpValue = 0.1f; //valor de interpolaci�n para suavizar el movimiento
    public float zoomSpeed = 0.1f;         //velocidad de cambio de zoom
    public Image Aim;
   

    private Vector3 _currentOffset;         //offset actual de la c�mara
    private float _yaw = 0f;                //rotaci�n horizontal
    private float _pitch = 0f;              //rotaci�n vertical
    public bool isAiming = false;         //indicador de si est� apuntando
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //bloquear el cursor en el centro de la pantalla
        _yaw = transform.eulerAngles.y;           //inicializar _yaw con la rotaci�n actual de la c�mara
        _pitch = transform.eulerAngles.x;         //inicializar _pitch con la rotaci�n actual de la c�mara
        _currentOffset = normalOffset;            //comenzar con el offset normal
        Aim.enabled = false;
    }

    private void LateUpdate()
    {
        //obtener la entrada del mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad;

        //actualizar las rotaciones en base al input del mouse
        _yaw += mouseX;
        _pitch -= mouseY;

        //limitar el _pitch (rotaci�n vertical) para evitar �ngulos inc�modos
        _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);

        //alternar entre el offset de zoom y el normal al hacer clic derecho
        if (Input.GetMouseButton(1)) //clic derecho
        {
            isAiming = true;
            _currentOffset = Vector3.Lerp(_currentOffset, aimOffset, zoomSpeed);
            Aim.enabled = true;
        }
        else
        {
            isAiming = false;
            _currentOffset = Vector3.Lerp(_currentOffset, normalOffset, zoomSpeed);
            Aim.enabled = false;
        }

        //crear la rotaci�n deseada basada en _yaw y _pitch
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);

        //calcular la nueva posici�n deseada de la c�mara
        Vector3 desiredPosition = target.position + rotation * _currentOffset;
        
        //aplicar la rotaci�n calculada
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, desiredPosition, lerpValue), rotation);
    }
}
