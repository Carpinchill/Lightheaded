using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public float detectionRange = 10f; // Rango de detección de la linterna
    public LayerMask enemyLayer; // Capa del enemigo
    public int rays = 10; // Cantidad de rayos para el cono de luz
    public float angle = 30f; // Ángulo del cono de luz

    void Update()
    {
        DetectEnemyInCone();
    }

    void DetectEnemyInCone()
    {
        // Lanzar raycasts en diferentes ángulos dentro del cono
        for (int i = 0; i < rays; i++)
        {
            // Calculamos el ángulo del rayo en el cono de luz
            float currentAngle = Mathf.Lerp(-angle / 2, angle / 2, (float)i / (rays - 1));
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward; // Gira el rayo en el eje Y

            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, detectionRange, enemyLayer))
            {
                // Verificar si golpeamos al enemigo
                if (hit.collider.CompareTag("Enemy"))
                {
                    Enemy enemyScript = hit.collider.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        enemyScript.FleeFromLight(transform.position);
                    }
                }
            }

            // Para visualizar el raycast en la ventana de escena
            Debug.DrawRay(transform.position, direction * detectionRange, Color.yellow);
        }
    }
}
