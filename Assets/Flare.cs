using UnityEngine;
using System;

public class HandflareThrow : MonoBehaviour
{
    // Estructura para la bengala
    public struct Flare
    {
        public string name;
        public float throwForce;
        public float throwAngle;

        // Constructor
        public Flare(string name, float throwForce, float throwAngle)
        {
            this.name = name;
            this.throwForce = throwForce;
            this.throwAngle = throwAngle;
        }

        // Getter y Setter para el nombre de la bengala
        public string GetName() => name;
        public void SetName(string newName) => name = newName;

        // Getter y Setter para la fuerza de lanzamiento
        public float GetThrowForce() => throwForce;
        public void SetThrowForce(float newForce) => throwForce = newForce;

        // Getter y Setter para el �ngulo de lanzamiento
        public float GetThrowAngle() => throwAngle;
        public void SetThrowAngle(float newAngle) => throwAngle = newAngle;
    }

    // Evento para lanzar la bengala
    public event Action OnFlareThrown;

    [Header("Configuraci�n de la bengala")]
    public GameObject flarePrefab;
    public Transform throwPoint;
    public float throwForce = 10f;

    private Camera playerCamera;
    private Flare currentFlare;

    void Start()
    {
        // Inicializar la bengala y obtener la c�mara del jugador
        currentFlare = new Flare("Handflare", throwForce, 45f); // �ngulo por defecto
        playerCamera = Camera.main;
    }

    void Update()
    {
        // Detectar si se mantiene presionado el click derecho (apuntar)
        if (Input.GetMouseButton(1)) // Click derecho
        {
            // Detectar si se presiona el click izquierdo (lanzar)
            if (Input.GetMouseButtonDown(0)) // Click izquierdo
            {
                ThrowFlare();
            }
        }
    }

    // M�todo para lanzar la bengala
    private void ThrowFlare()
    {
        // Crear la bengala a partir del prefab
        GameObject flare = Instantiate(flarePrefab, throwPoint.position, Quaternion.identity);
        if (flare.GetComponent<Collider>() == null)
        {
            Debug.LogError("El prefab instanciado no tiene un collider.");
        }
        

                // Obtener la direcci�n de la c�mara
                Vector3 cameraForward = playerCamera.transform.forward;

        // Calculamos el �ngulo en el que se lanzar� la bengala, basado en la direcci�n de la c�mara
        float angleInRadians = currentFlare.GetThrowAngle() * Mathf.Deg2Rad;

        // Ajustar la direcci�n de lanzamiento usando la direcci�n de la c�mara
        Vector3 launchDirection = cameraForward + new Vector3(0, Mathf.Tan(angleInRadians), 0);
        launchDirection.Normalize(); // Aseguramos que la direcci�n est� normalizada

        // Aplicar la fuerza de lanzamiento (con la direcci�n ajustada)
        Rigidbody flareRb = flare.GetComponent<Rigidbody>();
        flareRb.AddForce(launchDirection * currentFlare.GetThrowForce(), ForceMode.Impulse);

        // Invocar el evento de lanzamiento
        OnFlareThrown?.Invoke();
    }

}
