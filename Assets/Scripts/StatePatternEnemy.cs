using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class StatePatternEnemy : MonoBehaviour
{

    public float searchDuration; // Alert tilan etsint�aika
    public float searchTurnSpeed; // Alert tilan py�rimisnopeus
    public float sightRange; // N�k�s�teen kantomatka
    public Transform[] wayPoints; // Waypointit taulukossa, jotta niit� voi olla useampia
    public Transform eye; // Silm�, josta n�k�s�de(Raycast) l�htee 
    public MeshRenderer indicator; // Laatikko vihollisen p��ll�, muuttaa v�ri� sen mukaan miss� tilassa ollaan. Debuggity�kalu
    public Vector3 lastKnownPlayerPosition; //pelaajan viimeisin sijainti tracking statea varten.


    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public TrackingState trackingState;
    [HideInInspector] public NavMeshAgent navMeshAgent;



    private void Awake()
    {

        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        chaseState = new ChaseState(this);
        trackingState = new TrackingState(this);

        navMeshAgent = GetComponent<NavMeshAgent>();
    }


    // Start is called before the first frame update
    void Start()
    {
        currentState = patrolState;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
}
