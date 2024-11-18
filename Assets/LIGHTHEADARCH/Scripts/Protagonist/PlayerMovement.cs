using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public AudioSource pasos;
    private bool Hactivo;
    private bool Vactivo;
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
    private bool _isChargingShine = false;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();  //obtener el Rigidbody del jugador
        playerRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;  //evitar inclinación o rotación en ejes X y Z
        _originalSpeed = playerSpeed;  //guardar la velocidad original
        lightResource = GetComponent<LightResource>();
        anim = GetComponent<Animator>();  //obtener el Animator
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

            // Actualizar los parámetros de animación
            anim.SetFloat("VelX", horizontalMove);
            anim.SetFloat("VelY", verticalMove);

            // Controlar el movimiento de las armas (desactivarlas mientras se recarga)
            if (!_isChargingShine)
            {
                // Si no se está recargando, se puede mover normalmente
                MovePlayer();
            }
            else
            {
                // Si se está recargando, reducir la velocidad
                ReduceSpeed();
            }

            // Al presionar la tecla "C", iniciar la recarga
            if (Input.GetKey(KeyCode.C) && lightResource.GetCurrentShine() < 1f && !_isChargingShine)
            {
                // Empezar a recargar
                StartChargingShine();
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                // Dejar de recargar
                StopChargingShine();
            }

            // Si el jugador está apuntando, rotar hacia donde está mirando la cámara
            if (cameraManager.isAiming)
            {
                RotatePlayer(cameraManager.mainCamera.transform.forward);  // Rotar hacia donde está mirando la cámara
            }
            else
            {
                RotatePlayer(_movePlayer);  // De lo contrario, rotar hacia la dirección de movimiento
            }
        }

        if (Input.GetButtonDown("Horizontal"))
        {
            Hactivo = true;
            pasos.Play();
        }
        if (Input.GetButtonDown("Vertical"))
        {
            Vactivo = true;
            pasos.Play();
        }
        if (Input.GetButtonUp("Horizontal"))
        {

            Hactivo = false;
            if (Vactivo == false)
            {
                pasos.Pause();
            }

        }
        if (Input.GetButtonUp("Vertical"))
        {
            Vactivo = true;
            if (Hactivo == false)
            {
                pasos.Pause();
            }
        }
    }

    void FixedUpdate()
    {
        if (!_isImmobilized && !_isChargingShine)
        {
            // Aplicar movimiento solo si no se está recargando el "mana"
            Vector3 movement = playerSpeed * Time.deltaTime * _movePlayer;
            playerRb.MovePosition(playerRb.position + movement);
        }
    }

    // Función para rotar al jugador hacia la dirección de movimiento o cámara (si está apuntando)
    void RotatePlayer(Vector3 direction)
    {
        Quaternion targetRotation;

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
            if (direction != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(direction);
            }
            else
            {
                // Si no hay movimiento, no hacer rotación
                return;
            }
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

    // Función para inmovilizar al jugador (usada por el sistema de habilidades, por ejemplo)
    public IEnumerator Immobilize(float duration)
    {
        _isImmobilized = true;
        yield return new WaitForSeconds(duration);
        _isImmobilized = false;
    }

    // Función para reducir la velocidad mientras se recarga
    public void ReduceSpeed()
    {
        if (playerSpeed == _originalSpeed)
        {
            playerSpeed /= 2;  // Reducir la velocidad a la mitad
        }
    }

    // Función para restaurar la velocidad original
    public void RestoreSpeed()
    {
        if (playerSpeed != _originalSpeed)
        {
            playerSpeed = _originalSpeed;  // Restaurar la velocidad original
        }
    }

    // Empezar la recarga de mana
    private void StartChargingShine()
    {
        _isChargingShine = true;
        lightResource.StartRegen();  // Comienza la regeneración de "mana" en el script de la luz
    }

    // Detener la recarga de mana
    private void StopChargingShine()
    {
        _isChargingShine = false;
        lightResource.StopRegen();  // Llama a la función de LightResource para detener la recarga
        RestoreSpeed();  // Restaurar la velocidad original
    }

    // Función de movimiento del jugador (aplicada solo cuando no está recargando)
    private void MovePlayer()
    {
        if (_movePlayer != Vector3.zero)
        {
            RotatePlayer(_movePlayer);  // Aplicar la rotación hacia la dirección de movimiento
        }

        Vector3 movement = playerSpeed * Time.deltaTime * _movePlayer;
        playerRb.MovePosition(playerRb.position + movement);
    }
}
