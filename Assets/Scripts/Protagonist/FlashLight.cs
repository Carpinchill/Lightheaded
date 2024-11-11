using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public float detectionRange = 10f; // Rango de detección de la linterna
    public int rays = 10; // Cantidad de rayos para el cono de luz
    public float angle = 30f; // Ángulo del cono de luz
    public float shineConsumptionRate; // Consumo de Shine por segundo
    public LightResource lightResource; // Referencia al script de LightResource
    public Weapons weapons; // Referencia al script de Weapons
    public CameraManager cameraManager; // Referencia al script de CameraManager
    public Light flashLightLight;

    private bool canUseFlashlight = true; // Determina si la linterna puede ser usada
    private float cooldownTime = 5f; // Tiempo de cooldown
    private float currentCooldownTime; // Tiempo actual de cooldown

    private void Start()
    {
        flashLightLight.enabled = false;
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
            if (Input.GetMouseButton(0)) // Si se está presionando el clic izquierdo
            {
                // Solo activar la linterna si hay suficiente Shine
                if (lightResource.GetCurrentShine() >= shineConsumptionRate * Time.deltaTime)
                {
                    flashLightLight.enabled = true; // Encender la linterna
                    DetectEnemyInCone(); // Detectar enemigos en el cono de luz
                    lightResource.UseShine(shineConsumptionRate * Time.deltaTime); // Consumir Shine
                }
                else
                {
                    flashLightLight.enabled = false; // Apagar la linterna si no hay suficiente Shine
                    canUseFlashlight = false; // Desactivamos la linterna
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
            canUseFlashlight = true; // Restablecer la posibilidad de usarla
        }

        // Si la linterna se apaga por falta de Shine, reiniciamos el cooldown
        if (!canUseFlashlight && lightResource.GetCurrentShine() >= shineConsumptionRate)
        {
            canUseFlashlight = true; // Restauramos el uso de la linterna
            currentCooldownTime = cooldownTime; // Iniciamos el cooldown
        }
    }

    void DetectEnemyInCone()
    {
        for (int i = 0; i < rays; i++)
        {
            float currentAngle = Mathf.Lerp(-angle / 2, angle / 2, (float)i / (rays - 1));
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, detectionRange, 1 << LayerMask.NameToLayer("Enemy")))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    if (hit.collider.TryGetComponent<Enemy>(out var enemyScript))
                    {
                        enemyScript.FleeFromLight(transform.position); // El enemigo huye de la luz
                    }
                }
            }

            Debug.DrawRay(transform.position, direction * detectionRange, Color.yellow);
        }
    }
}
