using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemigo : EnemyBase
{
    public Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void AttackPlayer()
    {
        if (!isAttacking)
        {
            animator.SetBool("attack", true);
            isAttacking = true;
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
    }

    protected override void StopChasingPlayer()
    {
        base.StopChasingPlayer();
        animator.SetBool("run", false);
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
            SceneManager.LoadScene("LoseScene");
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