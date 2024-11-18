using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;        //referencia al componente NavMeshAgent
    public Transform player;          //transform del jugador
    public float fleeSpeed = 5f;      //velocidad al huir de la luz
    public float fleeDuration = 3f;   //tiempo que huye el enemigo
    private bool _isFleeing = false;   //indica si el enemigo est� huyendo
    private Vector3 _fleeTarget;       //destino temporal de huida

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //si no est� huyendo, persigue al jugador
        if (!_isFleeing)
        {
            agent.destination = player.position;
        }
        else
        {
            //verificamos si el enemigo lleg� al destino de huida
            if (Vector3.Distance(transform.position, _fleeTarget) < 0.5f)
            {
                StopFleeing(); //detiene la huida si lleg� a su destino
            }
        }
    }

    //m�todo para hacer que el enemigo huya de la luz
    public void FleeFromLight(Vector3 lightPosition)
    {
        _isFleeing = true;

        //calculamos la direcci�n opuesta a la luz
        Vector3 fleeDirection = transform.position - lightPosition;

        //normalizamos la direcci�n para que no dependa de la distancia
        fleeDirection.Normalize();

        //calculamos el destino de huida en esa direcci�n opuesta
        _fleeTarget = transform.position + fleeDirection * fleeSpeed;

        //asignamos el nuevo destino al NavMeshAgent
        agent.destination = _fleeTarget;

        //rstablecemos la velocidad de huida
        agent.speed = fleeSpeed;

        //reiniciamos la huida despu�s de cierto tiempo
        Invoke(nameof(StopFleeing), fleeDuration); //deja de huir despu�s de `fleeDuration` segundos
    }

    //m�todo para detener la huida
    private void StopFleeing()
    {
        _isFleeing = false;

        //restauramos la velocidad del enemigo a la predeterminada del agente
        agent.speed = agent.speed;  //puedes ajustar a la velocidad original aqu� si es necesario
    }
}

