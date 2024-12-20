using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemigo : EnemyBase
{
    public Animator animator;
    public AudioSource tensionMusic; // Referencia al AudioSource de la m�sica de tensi�n
    public float fadeOutSpeed = 1f;  // Velocidad de desvanecimiento de la m�sica

    private Coroutine fadeOutCoroutine;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();

        // Aseg�rate de que la m�sica no est� sonando al inicio
        if (tensionMusic != null)
        {
            tensionMusic.volume = 0f;
            tensionMusic.Stop();
        }
    }

    protected override void AttackPlayer()
    {
        if (!_isAttacking)
        {
            animator.SetBool("attack", true);
            _isAttacking = true;
        }
    }

    protected override void StopAttack()
    {
        base.StopAttack();
        animator.SetBool("attack", false);
    }

    protected override void ChasePlayer()
    {
        base.ChasePlayer();
        animator.SetBool("run", true);

        // Activa la m�sica de tensi�n
        if (tensionMusic != null)
        {
            if (!tensionMusic.isPlaying)
            {
                tensionMusic.Play();
            }
            tensionMusic.volume = 1f; // Aseg�rate de que suene al m�ximo
        }

        // Det�n cualquier fade out previo
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
    }

    protected override void StopChasingPlayer()
    {
        base.StopChasingPlayer();
        animator.SetBool("run", false);

        // Inicia el desvanecimiento de la m�sica
        if (tensionMusic != null)
        {
            fadeOutCoroutine = StartCoroutine(FadeOutMusic());
        }
    }

    private IEnumerator FadeOutMusic()
    {
        while (tensionMusic.volume > 0f)
        {
            tensionMusic.volume -= Time.deltaTime * fadeOutSpeed;
            yield return null;
        }

        // Det�n la m�sica una vez que el volumen sea 0
        tensionMusic.Stop();
    }

    protected override void Patrol()
    {
        base.Patrol();
        animator.SetBool("walk", agent.velocity.magnitude > 0.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Derrota");
        }
    }

    protected override void InitializePatrolPoints()
    {
        GameObject[] patrolObjects = GameObject.FindGameObjectsWithTag("PatrolPoint");
        patrolPoints = new Vector3[patrolObjects.Length];

        for (int i = 0; i < patrolObjects.Length; i++)
        {
            patrolPoints[i] = patrolObjects[i].transform.position;
        }
    }


}