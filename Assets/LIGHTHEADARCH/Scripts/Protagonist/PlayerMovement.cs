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
        playerRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;  //evitar inclinaci�n o rotaci�n en ejes X y Z
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
            _playerInput = Vector3.ClampMagnitude(_playerInput, 1);  // Limitar la magnitud para evitar movimientos r�pidos

            // Determinar la direcci�n en la que la c�mara est� mirando
            CamDirection();
            _movePlayer = _playerInput.x * _camRight + _playerInput.z * _camForward;

            // Actualizar los par�metros de animaci�n
            anim.SetFloat("VelX", horizontalMove);
            anim.SetFloat("VelY", verticalMove);

            // Controlar el movimiento de las armas (desactivarlas mientras se recarga)
            if (!_isChargingShine)
            {
                // Si no se est� recargando, se puede mover normalmente
                MovePlayer();
            }
            else
            {
                // Si se est� recargando, reducir la velocidad
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

            // Si el jugador est� apuntando, rotar hacia donde est� mirando la c�mara
            if (cameraManager.isAiming)
            {
                RotatePlayer(cameraManager.mainCamera.transform.forward);  // Rotar hacia donde est� mirando la c�mara
            }
            else
            {
                RotatePlayer(_movePlayer);  // De lo contrario, rotar hacia la direcci�n de movimiento
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
            // Aplicar movimiento solo si no se est� recargando el "mana"
            Vector3 movement = playerSpeed * Time.deltaTime * _movePlayer;
            playerRb.MovePosition(playerRb.position + movement);
        }
    }

    // Funci�n para rotar al jugador hacia la direcci�n de movimiento o c�mara (si est� apuntando)
    void RotatePlayer(Vector3 direction)
    {
        Quaternion targetRotation;

        if (cameraManager.isAiming)
        {
            // Obtener la direcci�n de la c�mara en el plano horizontal (ignorando el eje Y)
            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0;  // Evitar que el jugador gire hacia arriba o abajo

            // Calcular la rotaci�n hacia donde est� mirando la c�mara
            targetRotation = Quaternion.LookRotation(cameraForward);
        }
        else
        {
            // Si no est� apuntando, la rotaci�n ser� hacia la direcci�n de movimiento
            if (direction != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(direction);
            }
            else
            {
                // Si no hay movimiento, no hacer rotaci�n
                return;
            }
        }

        // Aplicar la rotaci�n suavizada
        playerRb.rotation = Quaternion.Slerp(playerRb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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

    // Funci�n para inmovilizar al jugador (usada por el sistema de habilidades, por ejemplo)
    public IEnumerator Immobilize(float duration)
    {
        _isImmobilized = true;
        yield return new WaitForSeconds(duration);
        _isImmobilized = false;
    }

    // Funci�n para reducir la velocidad mientras se recarga
    public void ReduceSpeed()
    {
        if (playerSpeed == _originalSpeed)
        {
            playerSpeed /= 2;  // Reducir la velocidad a la mitad
        }
    }

    // Funci�n para restaurar la velocidad original
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
        lightResource.StartRegen();  // Comienza la regeneraci�n de "mana" en el script de la luz
    }

    // Detener la recarga de mana
    private void StopChargingShine()
    {
        _isChargingShine = false;
        lightResource.StopRegen();  // Llama a la funci�n de LightResource para detener la recarga
        RestoreSpeed();  // Restaurar la velocidad original
    }

    // Funci�n de movimiento del jugador (aplicada solo cuando no est� recargando)
    private void MovePlayer()
    {
        if (_movePlayer != Vector3.zero)
        {
            RotatePlayer(_movePlayer);  // Aplicar la rotaci�n hacia la direcci�n de movimiento
        }

        Vector3 movement = playerSpeed * Time.deltaTime * _movePlayer;
        playerRb.MovePosition(playerRb.position + movement);
    }
}
