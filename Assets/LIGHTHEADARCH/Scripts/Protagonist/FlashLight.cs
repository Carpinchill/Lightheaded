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
    public ParticleSystem flashLightParticles; // Sistema de partículas de la linterna

    private bool canUseFlashlight = true; // Determina si la linterna puede ser usada
    private float cooldownTime = 5f; // Tiempo de cooldown
    private float currentCooldownTime; // Tiempo actual de cooldown

    private void Start()
    {
        flashLightLight.enabled = false;
        flashLightParticles.Stop(); // Asegurarse de que las partículas estén apagadas al inicio
    }

    void Update()
    {
        // Control de cooldown
        if (currentCooldownTime > 0)
        {
            currentCooldownTime -= Time.deltaTime;
            return;
        }

        if (weapons.currentWeapon == Weapons.WeaponState.Flashlight && cameraManager.isAiming)
        {
            if (Input.GetMouseButton(0))
            {
                if (lightResource.GetCurrentShine() >= shineConsumptionRate * Time.deltaTime)
                {
                    flashLightLight.enabled = true;
                    if (!flashLightParticles.isPlaying)
                    {
                        flashLightParticles.Play();
                        Debug.Log("Linterna encendida: partículas activadas.");
                    }
                    DetectEnemyInCone();
                    lightResource.UseShine(shineConsumptionRate * Time.deltaTime);
                }
                else
                {
                    flashLightLight.enabled = false;
                    if (flashLightParticles.isPlaying)
                    {
                        flashLightParticles.Stop();
                        Debug.Log("Linterna sin Shine: partículas desactivadas.");
                    }
                    canUseFlashlight = false;
                }
            }
            else
            {
                flashLightLight.enabled = false;
                if (flashLightParticles.isPlaying)
                {
                    flashLightParticles.Stop();
                    Debug.Log("Linterna apagada: partículas desactivadas.");
                }
                canUseFlashlight = true;
            }
        }
        else
        {
            flashLightLight.enabled = false;
            if (flashLightParticles.isPlaying)
            {
                flashLightParticles.Stop();
                Debug.Log("Linterna no equipada o no apuntando: partículas desactivadas.");
            }
            canUseFlashlight = true;
        }

        if (!canUseFlashlight && lightResource.GetCurrentShine() >= shineConsumptionRate)
        {
            canUseFlashlight = true;
            currentCooldownTime = cooldownTime;
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
