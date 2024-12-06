using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : Enemy
{
    NavMeshAgent enemyMove;
    [SerializeField] Transform[] waypoints;
    int index;
    private Animator animator;
    void Start(){
        enemyMove = GetComponent<NavMeshAgent>();
        enemyMove.SetDestination(waypoints[0].position);
        animator = GetComponent<Animator>();
    }

    void Update(){
        Movimentacao();
    }

    public override void Movimentacao(){
        if(enemyMove.remainingDistance < 1){
            if(index >= waypoints.Length - 1){
                index = 0;
            }else{
                index++;
            }

            enemyMove.SetDestination(waypoints[index].position);
        }
    }
}
