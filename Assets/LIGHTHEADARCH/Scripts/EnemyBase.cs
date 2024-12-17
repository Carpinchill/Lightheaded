using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public abstract class EnemyBase : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public float detectionRange = 10f; // Rango de detección del jugador
    public float chaseSpeed = 5f;     // Velocidad al perseguir
    public float patrolSpeed = 2f;    // Velocidad al patrullar

    protected bool isChasingPlayer = false; // Indica si está persiguiendo al jugador
    protected bool isAttacking = false;    // Indica si está atacando
    private Vector3 currentAvoidPoint;     // Punto actual para evitar la bengala
    private bool avoidingLight = false;    // Indica si está evitando la luz

    // Puntos de patrullaje
    protected int currentPatrolIndex = 0;
    protected Vector3[] patrolPoints;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player")?.transform;
        }

        InitializePatrolPoints();
        agent.speed = patrolSpeed;

        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            GoToNextPatrolPoint();
        }
    }

    protected virtual void Update()
    {
        if (avoidingLight)
        {
            // Si estamos evitando la luz, nos movemos hacia el punto de evasión
            agent.SetDestination(currentAvoidPoint);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            ChasePlayer();
        }
        else if (isChasingPlayer)
        {
            StopChasingPlayer();
        }
        else
        {
            Patrol();
        }

        if (distanceToPlayer < 2f)
        {
            AttackPlayer();
        }
        else if (isAttacking)
        {
            StopAttack();
        }
    }

    protected abstract void AttackPlayer(); // Método que define el ataque específico de cada enemigo

    protected virtual void StopAttack()
    {
        isAttacking = false;
    }

    protected virtual void ChasePlayer()
    {
        isChasingPlayer = true;
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    protected virtual void StopChasingPlayer()
    {
        isChasingPlayer = false;
        agent.speed = patrolSpeed;
        GoToNextPatrolPoint();
    }

    protected virtual void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }
    }

    protected void GoToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex];
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    protected abstract void InitializePatrolPoints();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light")) // Detectar si es un área de luz
        {
            avoidingLight = true;

            // Calcular un punto fuera del área de luz
            Vector3 directionAway = (transform.position - other.transform.position).normalized;
            float lightRadius = other.bounds.extents.magnitude; // Obtener el radio aproximado del SphereCollider
            currentAvoidPoint = other.transform.position + directionAway * (lightRadius + 2f);

            agent.speed = patrolSpeed; // Ajustar la velocidad mientras rodea
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light")) // Salir del área de luz
        {
            avoidingLight = false;
        }
    }
}