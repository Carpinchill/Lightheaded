using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    //BRUNO
    public Transform target;               
    public Vector3 normalOffset;           
    public Vector3 aimOffset;              
    public Camera mainCamera;              
    public float sensibilidad = 3f;        
    public float pitchMin = -40f;          
    public float pitchMax = 60f;           
    [Range(0, 1)] public float lerpValue = 0.1f; 
    public float zoomSpeed = 0.1f;         
    public Image Aim;


    private Vector3 _currentOffset;         
    private float _yaw = 0f;               
    private float _pitch = 0f;            
    public bool isAiming = false;        

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        _yaw = transform.eulerAngles.y;           
        _pitch = transform.eulerAngles.x;        
        _currentOffset = normalOffset;           
        Aim.enabled = false;
    }

    private void LateUpdate()
    {
        
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad;

        
        _yaw += mouseX;
        _pitch -= mouseY;

        
        _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);

        
        if (Input.GetMouseButton(1)) 
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

        
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);

        
        Vector3 desiredPosition = target.position + rotation * _currentOffset;

        
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, desiredPosition, lerpValue), rotation);
    }
}