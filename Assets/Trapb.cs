using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapb : MonoBehaviour
{
    public float slowSpeed = 2f; // Velocidad de ralentización
    public AudioSource sonido; // Sonido que se reproduce al activar la trampa

    private TrapManagerBase trapManager;

    private void Start()
    {
        // Buscar el componente TrapManagerBase en el objeto
        trapManager = GetComponent<TrapManagerBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Solo aplicar el efecto si es el jugador
        if (trapManager != null && other.CompareTag("Player"))
        {
            string effectDescription = trapManager.GetTrapEffect();
            Debug.Log("Efecto de la trampa: " + effectDescription);

            // Si es de tipo SlowDown, aplicar el efecto de ralentización
            if (trapManager.trapType == TrapManagerBase.TrapType.SlowDown)
            {
                ApplySlowDown(other);
            }
            else if (trapManager.trapType == TrapManagerBase.TrapType.Stun)
            {
                ApplyStun(other);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Aplicar ralentización mientras el jugador está en el área de la trampa (solo para SlowDown)
        if (trapManager != null && other.CompareTag("Player"))
        {
            if (trapManager.trapType == TrapManagerBase.TrapType.SlowDown)
            {
                ApplySlowDown(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cuando el jugador sale de la trampa, restaurar su velocidad
        if (trapManager != null && other.CompareTag("Player"))
        {
            if (trapManager.trapType == TrapManagerBase.TrapType.SlowDown)
            {
                RestoreSpeed(other);
            }
        }
    }

    // Método para aplicar el efecto de ralentización
    private void ApplySlowDown(Collider other)
    {
        if (other.TryGetComponent<_playerMovement>(out var playerMovement))
        {
            playerMovement.ReduceSpeed(slowSpeed); // Reducir velocidad del jugador
            sonido?.Play(); // Reproducir sonido si está configurado
        }
    }

    // Método para aplicar el efecto de inmovilización
    private void ApplyStun(Collider other)
    {
        if (other.TryGetComponent<_playerMovement>(out var playerMovement))
        {
            StartCoroutine(playerMovement.Immobilize(1.5f)); // Inmovilizar al jugador por 1.5 segundos
            sonido?.Play(); // Reproducir sonido si está configurado
        }
    }

    // Método para restaurar la velocidad al salir de la trampa
    private void RestoreSpeed(Collider other)
    {
        if (other.TryGetComponent<_playerMovement>(out var playerMovement))
        {
            playerMovement.RestoreSpeed(); // Restaurar la velocidad del jugador
        }
    }
}
