using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Animator ani;
    public GameObject target;
    private NavMeshAgent _agent;
    private readonly float _attackDistance = 1.1f;
    [SerializeField]
    private float _maxChaseDistance = 70f;
    private readonly float _minChaseDistance = 40f;
    private float _currentChaseDistance;
    private bool _attacking;
    public bool isFleeing;
    private LightResource _playerLightResource;
    private Vector3 lastKnownPosition;

    private enum EnemyState { Patrolling, Chasing, Attacking, Reevaluating }
    private EnemyState currentState = EnemyState.Patrolling;

    private void Start()
    {
        ani = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player");
        _playerLightResource = target.GetComponent<LightResource>();
        _agent.stoppingDistance = _attackDistance;
        _agent.speed = 8f; // Velocidad inicial de patrullaje
    }

    public void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToTarget <= _currentChaseDistance && currentState != EnemyState.Chasing && currentState != EnemyState.Attacking)
        {
            currentState = EnemyState.Chasing;
        }

        UpdateChaseDistance();
        UpdateEnemyBehavior();

        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Chasing:
                Chase();
                break;
            case EnemyState.Attacking:
                Attack();
                break;
            case EnemyState.Reevaluating:
                ReevaluatePosition();
                break;
        }
    }

    private void UpdateChaseDistance()
    {
        _currentChaseDistance = Mathf.Max(_minChaseDistance, _maxChaseDistance * (_playerLightResource.currentShine / _playerLightResource.maxShine));
    }

    private void UpdateEnemyBehavior()
    {
        float activeGeneratorsPercentage = GeneratorsManager.Instance.GetActiveGeneratorPercentage();
        if (activeGeneratorsPercentage >= 1f)
        {
            _agent.speed = Mathf.Lerp(4.5f, 12f, activeGeneratorsPercentage);  // Aumenta la velocidad con más generadores
            _maxChaseDistance = Mathf.Lerp(70f, 100f, activeGeneratorsPercentage);  // Aumenta el rango de persecución
        }
        else
        {
            _agent.speed = 4.5f;  // Velocidad normal cuando no todos los generadores están activados
            _maxChaseDistance = 70f;
        }
    }

    private void Patrol()
    {
        ani.SetBool("run", true);
        ani.SetBool("attack", false);
        _agent.speed = 8f; // Velocidad de patrullaje

        // Establece un nuevo punto de patrullaje si llega al destino anterior
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _agent.SetDestination(GetRandomPoint());
        }
    }

    private void Chase()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget <= _currentChaseDistance && distanceToTarget > _attackDistance)
        {
            _agent.SetDestination(target.transform.position);
            ani.SetBool("run", true);
            ani.SetBool("attack", false);
            _agent.speed = 4.5f; // Velocidad de persecución
        }
        else if (distanceToTarget <= _attackDistance)
        {
            currentState = EnemyState.Attacking;
        }
        else
        {
            currentState = EnemyState.Patrolling;
        }
    }

    private void Attack()
    {
        _agent.isStopped = true; // Detiene el agente mientras ataca
        ani.SetBool("attack", true); // Activa la animación de ataque

        // Verifica si la animación de ataque ha terminado antes de cambiar de estado
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Attack") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            ani.SetBool("attack", false); // Apaga la animación de ataque una vez que ha terminado
            currentState = EnemyState.Patrolling; // O regresa al estado de patrullaje u otro comportamiento que desees
        }
    }
    

    public void ReevaluatePosition()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            Vector3 directionToPlayer = (target.transform.position - transform.position).normalized;
            Vector3 sideStepDirection = Vector3.Cross(directionToPlayer, Vector3.up);

            Vector3 alternativePoint = target.transform.position + sideStepDirection * Random.Range(3f, 6f);
            _agent.SetDestination(alternativePoint);

            currentState = EnemyState.Chasing;
        }
    }

    public void FleeFromLight(Vector3 lightPosition)
    {
        if (!isFleeing)
        {
            isFleeing = true;
            lastKnownPosition = lightPosition;
            _agent.speed = 12f; // Velocidad de huida
        }

        Vector3 fleeDirection = (transform.position - lastKnownPosition).normalized;
        Vector3 fleeTarget = transform.position + fleeDirection * 5f;
        _agent.SetDestination(fleeTarget);

        if (Vector3.Distance(transform.position, lastKnownPosition) > 5f)
        {
            currentState = EnemyState.Reevaluating;
            _agent.speed = 4.5f; // Restablece la velocidad de persecución
            isFleeing = false;
        }
    }

    private Vector3 GetRandomPoint()
    {
        Vector3 forward = transform.forward;
        float minDistance = 40f;
        float maxDistance = 70f;

        for (int i = 0; i < 5; i++)
        {
            // Genera un ángulo aleatorio entre -90 y 90 grados para cubrir los lados, frente y algo de atrás
            float randomAngle = Random.Range(-90f, 90f);
            Vector3 direction = Quaternion.Euler(0, randomAngle, 0) * forward;

            // Calcula un punto de destino en esa dirección y en un rango de distancias
            Vector3 potentialPoint = transform.position + direction * Random.Range(minDistance, maxDistance);

            // Limitar las coordenadas X y Z para mantener al enemigo dentro del área deseada
            potentialPoint.x = Mathf.Clamp(potentialPoint.x, 172f, 342f);
            potentialPoint.z = Mathf.Clamp(potentialPoint.z, 128f, 260f);

            // Verifica si el punto ajustado está en el NavMesh
            if (NavMesh.SamplePosition(potentialPoint, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        // Respaldo en dirección opuesta si no encuentra otra posición
        Vector3 backupDirection = -forward;
        Vector3 backupPoint = transform.position + backupDirection * Random.Range(minDistance, maxDistance);
        backupPoint.x = Mathf.Clamp(backupPoint.x, 172f, 342f);
        backupPoint.z = Mathf.Clamp(backupPoint.z, 128f, 260f);

        if (NavMesh.SamplePosition(backupPoint, out NavMeshHit backupHit, maxDistance, NavMesh.AllAreas))
        {
            return backupHit.position;
        }

        return transform.position;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reiniciar la escena
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}