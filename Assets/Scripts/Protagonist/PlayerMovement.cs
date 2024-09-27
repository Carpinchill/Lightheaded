using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontalMove;
    public float verticalMove;
    public Rigidbody playerRb;  // Rigidbody del jugador
    private Vector3 _movePlayer;
    public CameraManager cameraManager;

    private Vector3 _playerInput;

    public Camera mainCamera;
    private Vector3 _camForward;
    private Vector3 _camRight;

    public float playerSpeed;
    public float rotationSpeed = 10f; // Velocidad de rotación suavizada
    private bool _isImmobilized = false;
    private float _originalSpeed;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();  // Obtener el Rigidbody del jugador
        playerRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;  // Evitar inclinación o rotación en ejes X y Z
        _originalSpeed = playerSpeed;  // Guardar la velocidad original
    }

    void Update()
    {
        if (!_isImmobilized)
        {
            // Obtener el input de movimiento del jugador
            horizontalMove = Input.GetAxis("Horizontal");
            verticalMove = Input.GetAxis("Vertical");

            _playerInput = new Vector3(horizontalMove, 0, verticalMove);
            _playerInput = Vector3.ClampMagnitude(_playerInput, 1);  // Limitar la magnitud para evitar movimientos rápidos

            // Determinar la dirección en la que la cámara está mirando
            CamDirection();
            _movePlayer = _playerInput.x * _camRight + _playerInput.z * _camForward;

            // Aplicar rotación hacia la dirección de movimiento solo si hay movimiento y no está apuntando
            if (_movePlayer != Vector3.zero)
            {
                RotatePlayer(_movePlayer);
            }
        }
    }

    void FixedUpdate()
    {
        if (!_isImmobilized)
        {
            // Aplicar movimiento al Rigidbody
            Vector3 movement = playerSpeed * Time.deltaTime * _movePlayer;
            playerRb.MovePosition(playerRb.position + movement);
        }
    }

    // Función para rotar al jugador hacia la dirección de movimiento
    void RotatePlayer(Vector3 direction)
    {
        Quaternion targetRotation;

        // Si el jugador está apuntando, la rotación será hacia la dirección de la cámara
        if (cameraManager.isAiming)
        {
            // Obtener la dirección de la cámara en el plano horizontal (ignorando el eje Y)
            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0;  // Evitar que el jugador gire hacia arriba o abajo

            // Calcular la rotación hacia donde está mirando la cámara
            targetRotation = Quaternion.LookRotation(cameraForward);
        }
        else
        {
            // Si no está apuntando, la rotación será hacia la dirección de movimiento
            targetRotation = Quaternion.LookRotation(direction);
        }

        // Aplicar la rotación suavizada
        playerRb.rotation = Quaternion.Slerp(playerRb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Determinar la dirección en la que está mirando la cámara, ignorando la rotación en el eje Y
    void CamDirection()
    {
        _camForward = mainCamera.transform.forward;
        _camRight = mainCamera.transform.right;

        // Ignorar la rotación en el eje Y de la cámara
        _camForward.y = 0;
        _camRight.y = 0;

        _camForward = _camForward.normalized;
        _camRight = _camRight.normalized;
    }

    public IEnumerator Immobilize(float duration)
    {
        _isImmobilized = true;  // Inmovilizar al jugador
        yield return new WaitForSeconds(duration);  // Esperar el tiempo de inmovilización
        _isImmobilized = false;  // Liberar al jugador
    }

    public void ReduceSpeed()
    {
        if (playerSpeed == _originalSpeed)
        {
            playerSpeed /= 2;  // Reducir la velocidad a la mitad
        }
    }

    public void RestoreSpeed()
    {
        if (playerSpeed != _originalSpeed)
        {
            playerSpeed = _originalSpeed;  // Restaurar la velocidad original
        }
    }
}
