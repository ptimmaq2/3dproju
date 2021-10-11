using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingState : IEnemyState
{
    private StatePatternEnemy enemy;
    int nextWayPoint;

    public TrackingState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
      //  Patrol();
        Track();

    }

    void Look()
    {
        RaycastHit hit;
        Debug.DrawRay(enemy.eye.position, enemy.eye.forward * enemy.sightRange, Color.green);

        if (Physics.Raycast(enemy.eye.position, enemy.eye.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        //if toteutuu vain jos s‰de osuu pelaajaan
        //Jos s‰de osuu pelaajaan, enemy menee chase tilaan ja tiet‰‰ ett‰ osuttu kappale on pelaaja.
        {
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
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
        enemy.currentState = enemy.chaseState;
    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
    }

    public void ToTrackingState()
    {
        enemy.currentState = enemy.trackingState;
    }

    void Track()
    {
     
        enemy.indicator.material.color = Color.grey;

        enemy.navMeshAgent.destination = enemy.lastKnownPlayerPosition;
        enemy.navMeshAgent.isStopped = false;

        if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending)
        {
            ToAlertState();
        }
    }


}
