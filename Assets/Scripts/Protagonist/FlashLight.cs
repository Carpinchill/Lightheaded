using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{   
    public float shineConsumptionRate; // Consumo de Shine por segundo
    public LightResource lightResource; // Referencia al script de LightResource
    public Weapons weapons; // Referencia al script de Weapons
    public CameraManager cameraManager; // Referencia al script de CameraManager
    public Light flashLightLight;
    public Collider detectionCone;
    public PlayerMovement playerMovement;

    private bool canUseFlashlight = true; // Determina si la linterna puede ser usada
    private float cooldownTime = 5f; // Tiempo de cooldown
    private float currentCooldownTime; // Tiempo actual de cooldown

    private void Start()
    {
        flashLightLight.enabled = false;
        detectionCone.enabled = false;
    }
    void Update()
    {
        // Control de cooldown
        if (currentCooldownTime > 0)
        {
            currentCooldownTime -= Time.deltaTime; // Reducir el tiempo de cooldown
            return; // Si está en cooldown, no hacemos nada más
        }

        // Revisar si el jugador tiene la linterna equipada y está apuntando
        if (weapons.currentWeapon == Weapons.WeaponState.Flashlight && cameraManager.isAiming)
        {
            if (Input.GetMouseButton(0) && !playerMovement.isChargingShine) // Si se está presionando el clic izquierdo
            {
                // Solo activar la linterna si hay suficiente Shine
                if (Input.GetMouseButton(0) && lightResource.GetCurrentShine() >= shineConsumptionRate * Time.deltaTime)
                {
                    flashLightLight.enabled = true;
                    detectionCone.enabled = true; // Activa el trigger solo si la linterna está encendida
                    lightResource.UseShine(shineConsumptionRate * Time.deltaTime);
                }
                else
                {
                    flashLightLight.enabled = false;
                    detectionCone.enabled = false; // Desactiva el trigger
                }
            }
            else
            {
                flashLightLight.enabled = false; // Apagar la linterna si no se está presionando el clic
                canUseFlashlight = true; // Restablecer la posibilidad de usarla
            }
        }
        else
        {
            flashLightLight.enabled = false; // Apagar la linterna si no está equipada o no está apuntando
            detectionCone.enabled = false;
            canUseFlashlight = true; // Restablecer la posibilidad de usarla
        }

        // Si la linterna se apaga por falta de Shine, reiniciamos el cooldown
        if (!canUseFlashlight && lightResource.GetCurrentShine() >= shineConsumptionRate)
        {
            canUseFlashlight = true; // Restauramos el uso de la linterna
            currentCooldownTime = cooldownTime; // Iniciamos el cooldown
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<Enemy>(out var enemyScript))
            {
                enemyScript.FleeFromLight(transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Asegúrate de que el objeto en el trigger es un enemigo
            if (other.TryGetComponent<Enemy>(out var enemyScript))
            {
                // El enemigo deja de huir cuando ya no está en el área de la luz
                enemyScript.ReevaluatePosition();
            }
        }
    }
}
