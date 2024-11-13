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

    public LightResource lightResource; // Referencia al script LightResource

    // Para controlar la recarga
    public bool isChargingShine = false;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _originalSpeed = playerSpeed;
        lightResource = GetComponent<LightResource>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!_isImmobilized)
        {
            // Obtener el input de movimiento del jugador
            horizontalMove = Input.GetAxis("Horizontal");
            verticalMove = Input.GetAxis("Vertical");

            _playerInput = new Vector3(horizontalMove, 0, verticalMove);
            _playerInput = Vector3.ClampMagnitude(_playerInput, 1);

            CamDirection();
            MovePlayer();
            _movePlayer = _playerInput.x * _camRight + _playerInput.z * _camForward;

            anim.SetFloat("VelX", horizontalMove);
            anim.SetFloat("VelY", verticalMove);

            // Verificar si está presionando Shift para correr
            bool isRunning = Input.GetKey(KeyCode.LeftShift);

            if (isRunning)
            {
                playerSpeed = _originalSpeed * 2f;  // Aumenta la velocidad al correr
            }
            else
            {
                playerSpeed = _originalSpeed;  // Restaurar velocidad normal
            }

            // Iniciar o detener la carga de luz
            if (Input.GetKey(KeyCode.C) && !isRunning && !isChargingShine)
            {
                StartChargingShine();
            }
            else if (Input.GetKeyUp(KeyCode.C) || isRunning)
            {
                StopChargingShine();
            }

            if (cameraManager.isAiming)
            {
                RotatePlayer(cameraManager.mainCamera.transform.forward);
            }
            else
            {
                RotatePlayer(_movePlayer);
            }
        }
    }

    void FixedUpdate()
    {
        if (!_isImmobilized && !isChargingShine)
        {
            Vector3 movement = playerSpeed * Time.deltaTime * _movePlayer;
            playerRb.MovePosition(playerRb.position + movement);
        }
    }

    void RotatePlayer(Vector3 direction)
    {
        Quaternion targetRotation;

        if (cameraManager.isAiming)
        {
            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0;

            targetRotation = Quaternion.LookRotation(cameraForward);
        }
        else
        {
            if (direction != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(direction);
            }
            else
            {
                return;
            }
        }

        playerRb.rotation = Quaternion.Slerp(playerRb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void CamDirection()
    {
        _camForward = mainCamera.transform.forward;
        _camRight = mainCamera.transform.right;

        _camForward.y = 0;
        _camRight.y = 0;

        _camForward = _camForward.normalized;
        _camRight = _camRight.normalized;
    }

    public IEnumerator Immobilize(float duration)
    {
        _isImmobilized = true;
        yield return new WaitForSeconds(duration);
        _isImmobilized = false;
    }

    public void ReduceSpeed()
    {
        if (playerSpeed == _originalSpeed)
        {
            playerSpeed /= 2;
        }
    }

    public void RestoreSpeed()
    {
        if (playerSpeed != _originalSpeed)
        {
            playerSpeed = _originalSpeed;
        }
    }

    private void StartChargingShine()
    {
        ReduceSpeed();
        isChargingShine = true;

        // Se inicia la recarga sin importar si el jugador tiene suficiente luz
        lightResource.StartRegen();
    }

    private void StopChargingShine()
    {
        RestoreSpeed();
        isChargingShine = false;
        lightResource.StopRegen();
    }

    private void MovePlayer()
    {
        if (_movePlayer != Vector3.zero)
        {
            RotatePlayer(_movePlayer);
        }

        Vector3 movement = playerSpeed * Time.deltaTime * _movePlayer;
        playerRb.MovePosition(playerRb.position + movement);
    }
}
