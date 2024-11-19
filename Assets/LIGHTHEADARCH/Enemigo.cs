using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemigo : MonoBehaviour, IEnemyActions // Implementa la interfaz IEnemyActions
{
    public Transform player;
    public NavMeshAgent agent;
    public Animator ani;

    // Rango de detección y velocidades
    public float detectionRange = 10f; // Rango de detección del jugador
    public float chaseSpeed = 5f; // Velocidad al perseguir
    public float patrolSpeed = 8f; // Velocidad al patrullar

    // Puntos de patrullaje y variables de patrullaje
    private List<Transform> patrolPoints = new List<Transform>(); // Lista de puntos de patrullaje
    private int currentPatrolIndex = 0; // Índice del punto actual de patrullaje

    // Variables de comportamiento
    private bool isChasingPlayer = false; // Bandera para saber si está persiguiendo
    private bool atacando = false; // Bandera para saber si está atacando
    private float cronometro = 0f; // Cronómetro para cambiar la rutina de patrullaje
    private int rutina = 0; // Rutina de patrullaje (número de estado de patrullaje)

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed; // Comienza patrullando a velocidad de patrullaje

        // Verifica que el objeto del jugador haya sido asignado
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        // Buscar y asignar los puntos de patrullaje
        FindPatrolPoints();

        // Verifica si se encontraron puntos de patrullaje
        if (patrolPoints.Count > 0)
        {
            GoToNextPatrolPoint();
        }
        else
        {
            Debug.LogWarning("No patrol points found. Ensure that patrol points are tagged as 'PatrolPoint'.");
        }

        // Inicializar el Animator
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si el jugador está dentro del rango de detección, el enemigo lo persigue
        if (distanceToPlayer < detectionRange)
        {
            isChasingPlayer = true;
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
            ani.SetBool("run", true); // Activar animación de correr
            ani.SetBool("walk", false); // Desactivar animación de caminar
        }
        else
        {
            // Si estaba persiguiendo, volver a la patrullaje
            if (isChasingPlayer)
            {
                isChasingPlayer = false;
                agent.speed = patrolSpeed;
                GoToNextPatrolPoint();
            }

            // Si llega al punto de patrullaje, pasa al siguiente
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextPatrolPoint();
            }

            // Verificar si el enemigo está moviéndose al patrullar
            if (agent.velocity.magnitude > 0.1f) // Usamos "magnitude" en lugar de "sqrMagnitude"
            {
                ani.SetBool("walk", true); // Activar animación de caminar
            }
            else
            {
                ani.SetBool("walk", false); // Desactivar animación de caminar
            }

            ani.SetBool("run", false); // Desactivar animación de correr
        }

        // Verifica si el enemigo debe atacar
        if (distanceToPlayer < 2f && !atacando)
        {
            ani.SetBool("walk", false);
            ani.SetBool("run", false);
            ani.SetBool("attack", true); // Activar animación de ataque
            atacando = true;
        }
        else if (distanceToPlayer >= 2f)
        {
            ani.SetBool("attack", false);
            atacando = false;
        }

        // Llamar al comportamiento del enemigo (rutinas de patrullaje)
        Comportamiento_Enemigo();
    }

    private void Comportamiento_Enemigo()
    {
        if (!isChasingPlayer)
        {
            cronometro += Time.deltaTime;

            if (cronometro >= 4)
            {
                rutina = Random.Range(0, 2); // Cambiar entre rutina de patrullaje o giro
                cronometro = 0;
            }

            switch (rutina)
            {
                case 0: // Esperar en un punto
                    ani.SetBool("walk", false); // Desactivar animación de caminar
                    break;

                case 1: // Girar a un ángulo aleatorio
                    float grado = Random.Range(0, 360);
                    Quaternion angulo = Quaternion.Euler(0, grado, 0);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 1f);
                    rutina++;
                    break;

                case 2: // Mover hacia adelante
                    transform.Translate(Vector3.forward * patrolSpeed * Time.deltaTime);
                    ani.SetBool("walk", true); // Activar animación de caminar
                    break;
            }
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Count == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
    }

    private void FindPatrolPoints()
    {
        GameObject[] patrolObjects = GameObject.FindGameObjectsWithTag("PatrolPoint");

        foreach (GameObject patrolObject in patrolObjects)
        {
            patrolPoints.Add(patrolObject.transform);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(3); // Cargar escena de "Game Over" o similar
        }
    }

    // Métodos de la interfaz IEnemyActions
    public void MoveToPlayer(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }

    public void StartAttack()
    {
        ani.SetBool("attack", true);
        atacando = true;
    }

    public void StopAttack()
    {
        ani.SetBool("attack", false);
        atacando = false;
    }

    public void Patrol()
    {
        // Si el enemigo no está persiguiendo al jugador, se encarga de patrullar
        if (!isChasingPlayer)
        {
            GoToNextPatrolPoint();
        }
    }
}
