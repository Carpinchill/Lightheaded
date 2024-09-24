using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public enum TrapType { Immobilize, SlowDown }
    public TrapType trapType;

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                if (trapType == TrapType.Immobilize)
                {
                    // Iniciar la inmovilización
                    StartCoroutine(playerMovement.Immobilize(1.5f));
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Verificar si el objeto que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null && trapType == TrapType.SlowDown)
            {
                playerMovement.ReduceSpeed(); // Reducir la velocidad mientras está en la trampa
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Restablecer la velocidad cuando el jugador salga de la tramp
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null && trapType == TrapType.SlowDown)
            {
                playerMovement.RestoreSpeed(); // Restaurar la velocidad original
            }
        }
    }
}
