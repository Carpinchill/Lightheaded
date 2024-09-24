using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;        // Referencia al componente NavMeshAgent
    public Transform player;          // Transform del jugador
    public float fleeSpeed = 5f;      // Velocidad al huir de la luz
    public float fleeDuration = 3f;   // Tiempo que huye el enemigo
    private bool _isFleeing = false;   // Indica si el enemigo est� huyendo
    private Vector3 _fleeTarget;       // Destino temporal de huida

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Si no est� huyendo, persigue al jugador
        if (!_isFleeing)
        {
            agent.destination = player.position;
        }
        else
        {
            // Verificamos si el enemigo lleg� al destino de huida
            if (Vector3.Distance(transform.position, _fleeTarget) < 0.5f)
            {
                StopFleeing(); // Detiene la huida si lleg� a su destino
            }
        }
    }

    // M�todo para hacer que el enemigo huya de la luz
    public void FleeFromLight(Vector3 lightPosition)
    {
        _isFleeing = true;

        // Calculamos la direcci�n opuesta a la luz
        Vector3 fleeDirection = transform.position - lightPosition;

        // Normalizamos la direcci�n para que no dependa de la distancia
        fleeDirection.Normalize();

        // Calculamos el destino de huida en esa direcci�n opuesta
        _fleeTarget = transform.position + fleeDirection * fleeSpeed;

        // Asignamos el nuevo destino al NavMeshAgent
        agent.destination = _fleeTarget;

        // Establecemos la velocidad de huida
        agent.speed = fleeSpeed;

        // Reiniciamos la huida despu�s de cierto tiempo
        Invoke("StopFleeing", fleeDuration); // Deja de huir despu�s de `fleeDuration` segundos
    }

    // M�todo para detener la huida
    private void StopFleeing()
    {
        _isFleeing = false;

        // Restauramos la velocidad del enemigo a la predeterminada del agente
        agent.speed = agent.speed;  // Puedes ajustar a la velocidad original aqu� si es necesario
    }
}

