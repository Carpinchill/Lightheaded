using System.Collections;
using UnityEngine;

public class Trap : TrapManager
{
    public enum TrapType { Immobilize, SlowDown }
    public TrapType trapType;
    public AudioSource sonido;

    public override void Activate(Collider other)
    {
        if (trapType == TrapType.Immobilize)
        {
            if (other.TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                StartCoroutine(playerMovement.Immobilize(1.5f)); // Activar inmovilización
                sonido.Play();

            }
        }
        else if (trapType == TrapType.SlowDown)
        {
            if (other.TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                playerMovement.ReduceSpeed(); // Reducir velocidad
            }
        }
    }
}


