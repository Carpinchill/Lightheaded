using System;
using UnityEngine;

public class HandflareThrow : MonoBehaviour, IFlareThrower
{
    //FRANCO
    public struct Flare
    {
        public string name;
        public float throwForce;
        public float throwAngle;

        
        public Flare(string name, float throwForce, float throwAngle)
        {
            this.name = name;
            this.throwForce = throwForce;
            this.throwAngle = throwAngle;
        }

        
        public string GetName() => name;
        public void SetName(string newName) => name = newName;

        
        public float GetThrowForce() => throwForce;
        public void SetThrowForce(float newForce) => throwForce = newForce;

        // Getter y Setter para el ángulo de lanzamiento
        public float GetThrowAngle() => throwAngle;
        public void SetThrowAngle(float newAngle) => throwAngle = newAngle;
    }

    
    public event Action OnFlareThrown;

    [Header("Configuración de la bengala")]
    public GameObject flarePrefab;
    public Transform throwPoint;
    public float throwForce = 10f;

    private Camera playerCamera;
    private Flare currentFlare;

    private int flareCount = 0; 
    private const int maxFlareCount = 3; 

    void Start()
    {
        
        currentFlare = new Flare("Handflare", throwForce, 45f); 
        playerCamera = Camera.main;
    }

    void Update()
    {
        
        if (Input.GetMouseButton(1)) 
        {
            
            if (Input.GetMouseButtonDown(0) && flareCount < maxFlareCount) 
            {
                ThrowFlare();
            }
        }
    }

    
    public void ThrowFlare()
    {
        
        GameObject flare = Instantiate(flarePrefab, throwPoint.position, Quaternion.identity);
        if (flare.GetComponent<Collider>() == null)
        {
            Debug.LogError("El prefab instanciado no tiene un collider.");
        }

        
        Vector3 cameraForward = playerCamera.transform.forward;

        
        float angleInRadians = currentFlare.GetThrowAngle() * Mathf.Deg2Rad;

        
        Vector3 launchDirection = cameraForward + new Vector3(0, Mathf.Tan(angleInRadians), 0);
        launchDirection.Normalize(); // Aseguramos que la dirección esté normalizada

       
        Rigidbody flareRb = flare.GetComponent<Rigidbody>();
        flareRb.AddForce(launchDirection * currentFlare.GetThrowForce(), ForceMode.Impulse);

        
        flareCount++;

        
        Destroy(flare, 10f);

        
        OnFlareThrown?.Invoke();
    }

    
    public int GetFlareCount()
    {
        return flareCount;
    }

    
    public void ResetFlareCount()
    {
        flareCount = 0;
    }
}