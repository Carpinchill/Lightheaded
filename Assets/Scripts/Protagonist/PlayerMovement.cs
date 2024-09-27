using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    public float horizontalMove;
    public float verticalMove;
    public Rigidbody playerRb; 
    private Vector3 _movePlayer;
    public CameraManager cameraManager;

    private Vector3 _playerInput;

    public Camera mainCamera;
    private Vector3 _camForward;
    private Vector3 _camRight;

    public float playerSpeed;
    public float rotationSpeed = 10f; 
    private bool _isImmobilized = false;
    private float _originalSpeed;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();  //obtener el Rigidbody del jugador
        playerRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;  //evitar inclinación o rotación en ejes X y Z
        _originalSpeed = playerSpeed;  //guardar la velocidad original

        anim = GetComponent<Animator>();  //obtener el Animator
    }

    void Update()
    {
        if (!_isImmobilized)
        {
            //obtener el input de movimiento del jugador
            horizontalMove = Input.GetAxis("Horizontal");
            verticalMove = Input.GetAxis("Vertical");

            _playerInput = new Vector3(horizontalMove, 0, verticalMove);
            _playerInput = Vector3.ClampMagnitude(_playerInput, 1);  //limitar la magnitud para evitar movimientos rápidos

            //determinar la dirección en la que la cámara está mirando
            CamDirection();
            _movePlayer = _playerInput.x * _camRight + _playerInput.z * _camForward;

            //actualizar los parámetros de animación
            anim.SetFloat("VelX", horizontalMove);
            anim.SetFloat("VelY", verticalMove);

            //aplicar rotación hacia la dirección de movimiento solo si hay movimiento y no está apuntando
            if (_movePlayer != Vector3.zero)
            {
                RotatePlayer(_movePlayer);  //esta rotación ya afecta al GameObject y, por tanto, al Animator.
            }
        }
    }

    void FixedUpdate()
    {
        if (!_isImmobilized)
        {
            //aplicar movimiento al Rigidbody
            Vector3 movement = playerSpeed * Time.deltaTime * _movePlayer;
            playerRb.MovePosition(playerRb.position + movement);
        }
    }

    //función para rotar al jugador hacia la dirección de movimiento
    void RotatePlayer(Vector3 direction)
    {
        Quaternion targetRotation;

        //si el jugador está apuntando, la rotación será hacia la dirección de la cámara
        if (cameraManager.isAiming)
        {
            //obtener la dirección de la cámara en el plano horizontal (ignorando el eje Y)
            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0;  // Evitar que el jugador gire hacia arriba o abajo

            //calcular la rotación hacia donde está mirando la cámara
            targetRotation = Quaternion.LookRotation(cameraForward);
        }
        else
        {
            //si no está apuntando, la rotación será hacia la dirección de movimiento
            targetRotation = Quaternion.LookRotation(direction);
        }

        //aplicar la rotación suavizada
        playerRb.rotation = Quaternion.Slerp(playerRb.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        //si el Animator está en el mismo GameObject que el Rigidbody, este girará junto con el personaje.
    }

    //determinar la dirección en la que está mirando la cámara, ignorando la rotación en el eje Y
    void CamDirection()
    {
        _camForward = mainCamera.transform.forward;
        _camRight = mainCamera.transform.right;

        //ignorar la rotación en el eje Y de la cámara
        _camForward.y = 0;
        _camRight.y = 0;

        _camForward = _camForward.normalized;
        _camRight = _camRight.normalized;
    }

    public IEnumerator Immobilize(float duration)
    {
        _isImmobilized = true;  //inmovilizar al jugador
        yield return new WaitForSeconds(duration);  //esperar el tiempo de inmovilización
        _isImmobilized = false;  //liberar al jugador
    }

    public void ReduceSpeed()
    {
        if (playerSpeed == _originalSpeed)
        {
            playerSpeed /= 2;  //reducir la velocidad a la mitad
        }
    }

    public void RestoreSpeed()
    {
        if (playerSpeed != _originalSpeed)
        {
            playerSpeed = _originalSpeed;  //restaurar la velocidad original
        }
    }
}
