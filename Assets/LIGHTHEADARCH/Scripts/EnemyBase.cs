using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public delegate void EnemyAction(string actionDescription);

public abstract class EnemyBase : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public float detectionRange = 10f;
    public float chaseSpeed = 5f;
    public float patrolSpeed = 2f;

    protected bool _isChasingPlayer = false;
    protected bool _isAttacking = false;
    private Vector3 _currentAvoidPoint;
    private bool _avoidingLight = false;

    // Puntos de patrullaje
    public Vector3[] patrolPoints;
    protected int _currentPatrolIndex = 0;

    // Delegado para notificar acciones
    public event EnemyAction OnEnemyAction;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player")?.transform;
        }

        InitializePatrolPoints();  // Método abstracto, implementado en la clase derivada
        agent.speed = patrolSpeed;

        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            GoToNextPatrolPoint();
        }
    }

    protected virtual void Update()
    {
        if (_avoidingLight)
        {
            agent.SetDestination(_currentAvoidPoint);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            ChasePlayer();
        }
        else if (_isChasingPlayer)
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
        else if (_isAttacking)
        {
            StopAttack();
        }
    }

    protected abstract void AttackPlayer();

    protected virtual void StopAttack()
    {
        _isAttacking = false;
    }

    protected virtual void ChasePlayer()
    {
        _isChasingPlayer = true;
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        // Notificar que el enemigo está persiguiendo
        OnEnemyAction?.Invoke("El enemigo está persiguiendo al jugador.");
    }

    protected virtual void StopChasingPlayer()
    {
        _isChasingPlayer = false;
        agent.speed = patrolSpeed;
        GoToNextPatrolPoint();

        // Notificar que el enemigo ha dejado de perseguir
        OnEnemyAction?.Invoke("El enemigo ha dejado de perseguir al jugador.");
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

        agent.destination = patrolPoints[_currentPatrolIndex];
        _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
    }

    protected abstract void InitializePatrolPoints();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            _avoidingLight = true;
            Vector3 directionAway = (transform.position - other.transform.position).normalized;
            float lightRadius = other.bounds.extents.magnitude;
            _currentAvoidPoint = other.transform.position + directionAway * (lightRadius + 2f);

            agent.speed = patrolSpeed;

            // Notificar que el enemigo está evitando la luz
            OnEnemyAction?.Invoke("El enemigo está evitando la luz.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            _avoidingLight = false;

            // Notificar que el enemigo ha dejado de evitar la luz
            OnEnemyAction?.Invoke("El enemigo ha dejado de evitar la luz.");
        }
    }
}