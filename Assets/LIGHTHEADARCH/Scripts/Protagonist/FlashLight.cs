using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    //BRUNO
    public float detectionRange = 10f; 
    public int rays = 10; 
    public float angle = 30f; 
    public float shineConsumptionRate; 
    public LightResource lightResource; 
    public Weapons weapons; 
    public CameraManager cameraManager; 
    public Light flashLightLight;
    public ParticleSystem flashLightParticles; 

    private bool canUseFlashlight = true; 
    private float cooldownTime = 5f; 
    private float currentCooldownTime; 

    private void Start()
    {
        flashLightLight.enabled = false;
        flashLightParticles.Stop(); 
    }

    void Update()
    {
        
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
                        enemyScript.FleeFromLight(transform.position); 
                    }
                }
            }

            Debug.DrawRay(transform.position, direction * detectionRange, Color.yellow);
        }
    }
}
