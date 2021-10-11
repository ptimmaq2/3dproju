using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    private StatePatternEnemy enemy;

    public ChaseState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Chase();
        Look();

    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {
        //ei k‰ytet‰.
    }

    public void ToPatrolState()
    {
        //harvemmin k‰ytet‰‰n.
        //palaa patrolliin, jos eliminoi pelaajan.
        enemy.currentState = enemy.patrolState;
    }

    void Chase()
    {
        enemy.indicator.material.color = Color.red;
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;
        enemy.navMeshAgent.isStopped = false;
        
    }
    void Look()
    {

        Vector3 enemyToTarget = enemy.chaseTarget.position - enemy.eye.position;
        RaycastHit hit;
        Debug.DrawRay(enemy.eye.position, enemy.eye.forward * enemy.sightRange, Color.red);

        if (Physics.Raycast(enemy.eye.position, enemyToTarget, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        //if toteutuu vain jos s‰de osuu pelaajaan
        //Jos s‰de osuu pelaajaan, enemy menee chase tilaan ja tiet‰‰ ett‰ osuttu kappale on pelaaja.
        {
            //enemyn silm‰t on pelaajassa kii
            enemy.chaseTarget = hit.transform;
           // ToChaseState();
        }
        else
        {
            //  enemy jahtaa pelaajaa muttei en‰‰ n‰e sit‰
            enemy.lastKnownPlayerPosition = enemy.chaseTarget.position;
            ToTrackingState();
        }
    }

    public void ToTrackingState()
    {
        enemy.currentState = enemy.trackingState;
    }


}
