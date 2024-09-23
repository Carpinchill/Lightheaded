using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generador : MonoBehaviour
{
    public float activationDistance = 2f; // Distancia para activar el generador
    private bool _isActive = false; // Para comprobar si el generador está activo
    private Transform playerTransform;

    void Start()
    {
        // Buscar el jugador por su etiqueta
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Comprobar si el jugador está cerca
        if (Vector3.Distance(transform.position, playerTransform.position) < activationDistance && !_isActive)
        {
            // Comprobar si se presiona la tecla "E"
            if (Input.GetKey(KeyCode.E))
            {
                StartCoroutine(ActivateGenerator());
            }
        }
    }

    private IEnumerator ActivateGenerator()
    {
        // Esperar 4 segundos mientras se mantiene "E"
        float holdTime = 0f;

        while (Input.GetKey(KeyCode.E) && holdTime < 4f)
        {
            holdTime += Time.deltaTime;
            yield return null; // Esperar un fras
        }

        // Comprobar si se mantuvo durante 4 segundos
        if (holdTime >= 4f)
        {
            _isActive = true;
            Debug.Log("Se activó el generador");
            // Aquí puedes añadir la lógica para activar el generador
        }
    }
}
