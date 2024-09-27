using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public enum TrapType { Immobilize, SlowDown }
    public TrapType trapType;

    private void OnTriggerEnter(Collider other)
    {
        //verificar si el objeto que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                if (trapType == TrapType.Immobilize)
                {
                    //iniciar la inmovilización
                    StartCoroutine(playerMovement.Immobilize(1.5f));
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //verificar si el objeto que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null && trapType == TrapType.SlowDown)
            {
                playerMovement.ReduceSpeed(); //reducir la velocidad mientras está en la trampa
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //restablecer la velocidad cuando el jugador salga de la tramp
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null && trapType == TrapType.SlowDown)
            {
                playerMovement.RestoreSpeed(); //restaurar la velocidad original
            }
        }
    }
}
