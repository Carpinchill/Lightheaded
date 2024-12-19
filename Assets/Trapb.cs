using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapb : MonoBehaviour
{
    //BELEN
    public float slowSpeed = 2f; 
    public AudioSource sonido; 

    private TrapManagerBase trapManager;

    private void Start()
    {
        
        trapManager = GetComponent<TrapManagerBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (trapManager != null && other.CompareTag("Player"))
        {
            string effectDescription = trapManager.GetTrapEffect();
            Debug.Log("Efecto de la trampa: " + effectDescription);

            
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
        
        if (trapManager != null && other.CompareTag("Player"))
        {
            if (trapManager.trapType == TrapManagerBase.TrapType.SlowDown)
            {
                RestoreSpeed(other);
            }
        }
    }

    
    private void ApplySlowDown(Collider other)
    {
        if (other.TryGetComponent<_playerMovement>(out var playerMovement))
        {
            playerMovement.ReduceSpeed(slowSpeed); 
            sonido?.Play(); 
        }
    }

    
    private void ApplyStun(Collider other)
    {
        if (other.TryGetComponent<_playerMovement>(out var playerMovement))
        {
            StartCoroutine(playerMovement.Immobilize(1.5f)); 
            sonido?.Play(); 
        }
    }

   
    private void RestoreSpeed(Collider other)
    {
        if (other.TryGetComponent<_playerMovement>(out var playerMovement))
        {
            playerMovement.RestoreSpeed(); 
        }
    }
}
