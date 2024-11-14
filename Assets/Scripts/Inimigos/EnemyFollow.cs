using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : Enemy
{
   // public GameObject player;
    NavMeshAgent enemyFollow;
    public float detectionRange = 10f;

    void Start()
    {
        enemyFollow = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Movimentacao();
    }

    public override void Movimentacao(){
        float distanceToPlayer = Vector3.Distance(GameController.controller.Player.transform.position, transform.position);
        Vector3 pos = Random.insideUnitSphere * detectionRange;

        if (distanceToPlayer <= detectionRange)
        {
            pos = GameController.controller.Player.transform.position;
        }
        enemyFollow.SetDestination(pos); 
    }
}
