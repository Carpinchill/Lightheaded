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
    public float rotationSpeed = 100f; // Velocidad de rotaci�n
    private bool _isImmobilized = false;
    private float _originalSpeed;

    void Start()
    {
        player = GetComponent<CharacterController>();
        _originalSpeed = playerSpeed; // Inicializar la velocidad original aqu�
    }

    // En el m�todo Update
    void Update()
    {
        // Si el jugador no est� inmovilizado
        if (!_isImmobilized)
        {
            // Obtener el input de movimiento del jugador
            horizontalMove = Input.GetAxis("Horizontal");
            verticalMove = Input.GetAxis("Vertical");

            _playerInput = new Vector3(horizontalMove, 0, verticalMove);
            _playerInput = Vector3.ClampMagnitude(_playerInput, 1); // Limitar la magnitud para evitar movimientos r�pidos

            // Determinar la direcci�n en la que la c�mara est� mirando
            CamDirection();
            _movePlayer = _playerInput.x * _camRight + _playerInput.z * _camForward;

            // Si el jugador se est� moviendo, rotar hacia la direcci�n de movimiento
            if (_movePlayer != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_movePlayer);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Mover al jugador en la direcci�n calculada
            player.Move(playerSpeed * Time.deltaTime * _movePlayer);
        }
    }

    // Determinar la direcci�n en la que est� mirando la c�mara, ignorando la rotaci�n en el eje Y
    void CamDirection()
    {
        _camForward = mainCamera.transform.forward;
        _camRight = mainCamera.transform.right;

        // Ignorar la rotaci�n en el eje Y de la c�mara
        _camForward.y = 0;
        _camRight.y = 0;

        _camForward = _camForward.normalized;
        _camRight = _camRight.normalized;
    }
    
    public IEnumerator Immobilize(float duration)
    {
        _isImmobilized = true; // Inmovilizar al jugador
        yield return new WaitForSeconds(duration); // Esperar el tiempo de inmovilizaci�n
        _isImmobilized = false; // Liberar al jugador
    }

    public void ReduceSpeed()
    {
        // Solo reducir la velocidad si es igual a la original
        if (playerSpeed == _originalSpeed)
        {
            playerSpeed /= 2; // Reducir la velocidad a la mitad
        }
    }

    public void RestoreSpeed()
    {
        // Solo restaurar si la velocidad ha sido reducida
        if (playerSpeed != _originalSpeed)
        {
            playerSpeed = _originalSpeed; // Restaurar la velocidad original
        }
    }
}