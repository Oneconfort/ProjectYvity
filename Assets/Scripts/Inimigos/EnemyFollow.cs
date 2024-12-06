using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : Enemy
{
    NavMeshAgent enemyFollow;
    public float detectionRange = 10f;
    private Animator animator;

    void Start()
    {
        enemyFollow = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Movimentacao();
        AtualizarAnimacao();
    }

    public override void Movimentacao()
    {
        float distanceToPlayer = Vector3.Distance(GameController.controller.Player.transform.position, transform.position);

        if (distanceToPlayer <= detectionRange)
        {
            Vector3 playerPosition = GameController.controller.Player.transform.position;
            enemyFollow.SetDestination(playerPosition);
        }
        else
        {
            enemyFollow.SetDestination(transform.position);
        }
    }
    void AtualizarAnimacao()
    {
        // Verifica se o agente está se movendo
        if (enemyFollow.velocity.magnitude > 0.1f)
        {
            animator.SetTrigger("Andar");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }
}
