using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontalMove;
    public float verticalMove;
    public CharacterController player;
    private Vector3 _movePlayer;

    private Vector3 _playerInput;

    public Camera mainCamera;
    private Vector3 _camForward;
    private Vector3 _camRight;

    public float playerSpeed;
    public float rotationSpeed = 100f; // Velocidad de rotación
    private bool _isImmobilized = false;
    private float _originalSpeed;

    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Obtener el input de movimiento del jugador
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        _playerInput = new Vector3(horizontalMove, 0, verticalMove);
        _playerInput = Vector3.ClampMagnitude(_playerInput, 1); // Limitar la magnitud para evitar movimientos rápidos

        // Determinar la dirección en la que la cámara está mirando
        CamDirection();
        _movePlayer = _playerInput.x * _camRight + _playerInput.z * _camForward;

        // Si el jugador se está moviendo
        if (_movePlayer != Vector3.zero)
        {
            // Calcular la rotación hacia la dirección de movimiento
            Quaternion targetRotation = Quaternion.LookRotation(_movePlayer);
            // Suavizar la rotación del jugador hacia esa dirección
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Mover al jugador en la dirección calculada
        player.Move(playerSpeed * Time.deltaTime * _movePlayer);
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
        _isImmobilized = true; // Inmovilizar al jugador
        yield return new WaitForSeconds(duration); // Esperar el tiempo de inmovilización
        _isImmobilized = false; // Liberar al jugador
    }

    public void ReduceSpeed()
    {
        playerSpeed = _originalSpeed / 2; // Reducir la velocidad a la mitad
    }

    public void RestoreSpeed()
    {
        playerSpeed = _originalSpeed; // Restaurar la velocidad original
    }
}
