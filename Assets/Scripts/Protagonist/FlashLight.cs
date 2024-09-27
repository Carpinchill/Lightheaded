using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public float detectionRange = 10f; //rango de detección de la linterna
    public LayerMask enemyLayer; //capa del enemigo
    public int rays = 10; //cantidad de rayos para el cono de luz
    public float angle = 30f; //angulo del cono de luz


    public float maxUsageTime = 3f;  //tiempo máximo de uso en segundos
    private float _currentUsageTime;  //tiempo actual de uso
    public float cooldownTime = 5f;  //tiempo de cooldown
    private float _currentCooldownTime;  //tiempo actual de cooldown

    private bool canUseFlashlight = true;
    public Weapons weapons;
    public CameraManager cameraManager;

    void Update()
    {
        if (!canUseFlashlight)
        {
            _currentCooldownTime -= Time.deltaTime;
            if (_currentCooldownTime <= 0)
            {
                canUseFlashlight = true;
                _currentUsageTime = maxUsageTime;  //restaurar tiempo de uso
            }
        }
        if (canUseFlashlight && cameraManager.isAiming && weapons.currentWeapon == Weapons.WeaponState.Flashlight && Input.GetMouseButton(0))
        {
            DetectEnemyInCone();
            UseFlashlight();
        }
    }

    void UseFlashlight()
    {
        _currentUsageTime -= Time.deltaTime;
        if (_currentUsageTime <= 0)
        {
            canUseFlashlight = false;
            _currentCooldownTime = cooldownTime;  //iniciar cooldown
        }
    }
    void DetectEnemyInCone()
    {
        //lanzar raycasts en diferentes ángulos dentro del cono
        for (int i = 0; i < rays; i++)
        {
            //calculamos el ángulo del rayo en el cono de luz
            float currentAngle = Mathf.Lerp(-angle / 2, angle / 2, (float)i / (rays - 1));
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward; //gira el rayo en el eje Y

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, detectionRange, 1 << LayerMask.NameToLayer("Enemy")))
            {
                //verificar si golpeamos al enemigo
                if (hit.collider.CompareTag("Enemy"))
                {
                    if (hit.collider.TryGetComponent<Enemy>(out var enemyScript))
                    {
                        enemyScript.FleeFromLight(transform.position);
                    }
                }
            }

            //para visualizar el raycast en la ventana de escena
            Debug.DrawRay(transform.position, direction * detectionRange, Color.yellow);
        }
    }
}
