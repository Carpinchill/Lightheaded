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
    private bool _isFleeing = false;   // Indica si el enemigo está huyendo
    private Vector3 _fleeTarget;       // Destino temporal de huida

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Si no está huyendo, persigue al jugador
        if (!_isFleeing)
        {
            agent.destination = player.position;
        }
        else
        {
            // Verificamos si el enemigo llegó al destino de huida
            if (Vector3.Distance(transform.position, _fleeTarget) < 0.5f)
            {
                StopFleeing(); // Detiene la huida si llegó a su destino
            }
        }
    }

    // Método para hacer que el enemigo huya de la luz
    public void FleeFromLight(Vector3 lightPosition)
    {
        _isFleeing = true;

        // Calculamos la dirección opuesta a la luz
        Vector3 fleeDirection = transform.position - lightPosition;

        // Normalizamos la dirección para que no dependa de la distancia
        fleeDirection.Normalize();

        // Calculamos el destino de huida en esa dirección opuesta
        _fleeTarget = transform.position + fleeDirection * fleeSpeed;

        // Asignamos el nuevo destino al NavMeshAgent
        agent.destination = _fleeTarget;

        // Establecemos la velocidad de huida
        agent.speed = fleeSpeed;

        // Reiniciamos la huida después de cierto tiempo
        Invoke("StopFleeing", fleeDuration); // Deja de huir después de `fleeDuration` segundos
    }

    // Método para detener la huida
    private void StopFleeing()
    {
        _isFleeing = false;

        // Restauramos la velocidad del enemigo a la predeterminada del agente
        agent.speed = agent.speed;  // Puedes ajustar a la velocidad original aquí si es necesario
    }
}

