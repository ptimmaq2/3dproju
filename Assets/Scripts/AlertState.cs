using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IEnemyState
{
    private StatePatternEnemy enemy;
    float searchTimer;

    public AlertState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Search();
        Look();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToAlertState()
    {
        //ei k‰ytet‰ koska ollaan jo alerttilassa.
    }

    public void ToChaseState()
    {
        searchTimer = 0;
        enemy.currentState = enemy.chaseState;
    }

    public void ToPatrolState()
    {
        //nollaa timerin
        searchTimer = 0;
        enemy.currentState = enemy.patrolState;

    }

    void Search()
    {
        enemy.indicator.material.color = Color.yellow;
        enemy.navMeshAgent.isStopped = true; //pys‰ytet‰‰n enemy.
        enemy.transform.Rotate(0, enemy.searchTurnSpeed * Time.deltaTime, 0);
        searchTimer += Time.deltaTime;
        
        //jos menee liian kauan lˆyt‰‰ pelaaja, enemy palaa patrollaamaan.
        if(searchTimer >= enemy.searchDuration)
        {
            ToPatrolState();
        }
    }
    void Look()
    {
        RaycastHit hit;
        Debug.DrawRay(enemy.eye.position, enemy.eye.forward * enemy.sightRange, Color.yellow);

        if (Physics.Raycast(enemy.eye.position, enemy.eye.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        //if toteutuu vain jos s‰de osuu pelaajaan
        //Jos s‰de osuu pelaajaan, enemy menee chase tilaan ja tiet‰‰ ett‰ osuttu kappale on pelaaja.
        {
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
    }

    public void ToTrackingState()
    {
        enemy.currentState = enemy.trackingState;
    }

}
