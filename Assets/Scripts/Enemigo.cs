using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemigo : MonoBehaviour
{
    public Animator ani;
    public GameObject target;
    private NavMeshAgent _agent;
    private float _attackDistance = 1.1f;

    [SerializeField]
    private float _maxChaseDistance = 40f; // Máximo rango de detección basado en shine
    private float _currentChaseDistance; // Rango de detección actual
    public bool atacando;
    public float patrolRadius = 20f; // Radio para el patrullaje aleatorio
    private LightResource _playerLightResource; // Para acceder al shine del jugador

    void Start()
    {
        ani = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        if (target == null)
        {
            target = GameObject.FindWithTag("Player");
        }

        _playerLightResource = target.GetComponent<LightResource>();

        _agent.stoppingDistance = _attackDistance;
    }

    void Update()
    {
        // Actualizar rango de detección en función del shine del jugador
        UpdateChaseDistance();

        // Comportamiento del enemigo
        Comportamiento_Enemigo();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void UpdateChaseDistance()
    {
        // El rango de detección aumenta con el shine del jugador (0 a _maxChaseDistance)
        _currentChaseDistance = _maxChaseDistance * (_playerLightResource.currentShine / _playerLightResource.maxShine);
    }

    public void Comportamiento_Enemigo()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (_agent.isOnNavMesh)
        {
            if (distanceToTarget <= _currentChaseDistance && distanceToTarget > _attackDistance)
            {
                // Persigue al jugador
                _agent.isStopped = false;
                ani.SetBool("run", true);
                ani.SetBool("attack", false);
                atacando = false;

                _agent.destination = target.transform.position;

                // Ajusta la velocidad del agente cuando persigue
                _agent.speed = 4f; // Velocidad de persecución (ajustar según necesidad)
            }
            else if (distanceToTarget <= _attackDistance)
            {
                // Ataca al jugador
                _agent.isStopped = true;
                ani.SetBool("run", false);
                ani.SetBool("attack", true);
                atacando = true;

                // La velocidad de persecución ya está ajustada en el bloque anterior
            }
            else
            {
                // Patrulla si el jugador no está en rango
                ani.SetBool("run", true);
                ani.SetBool("attack", false);
                atacando = false;

                // Si el agente está detenido o ha alcanzado su destino, busca un nuevo punto aleatorio
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                {
                    _agent.isStopped = false;
                    // Decide aleatoriamente si patrullará cerca o lejos
                    _agent.destination = GetRandomPoint(); // Usa el mismo método, pero ajusta la distancia aleatoria
                }

                // Ajusta la velocidad del agente cuando patrulla
                _agent.speed = 8f; // Velocidad de patrullaje (ajustar según necesidad)
            }
        }
    }

    private Vector3 GetRandomPoint()
    {
        // Decide aleatoriamente si patrullará cerca o lejos del jugador
        float patrolRange = (Random.Range(0, 2) == 0) ? _currentChaseDistance * 0.5f : patrolRadius;

        // Genera un punto aleatorio dentro del rango determinado
        Vector3 randomDirection = Random.insideUnitSphere * patrolRange;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRange, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position; // Si no encuentra un punto, se queda en su posición actual
    }

    // Llamado al final de la animación de ataque
    public void Final_Ani()
    {
        ani.SetBool("attack", false);
        atacando = false;
    }
}
