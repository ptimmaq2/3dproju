using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class StatePatternEnemy : MonoBehaviour
{

    public float searchDuration; // Alert tilan etsintäaika
    public float searchTurnSpeed; // Alert tilan pyörimisnopeus
    public float sightRange; // Näkösäteen kantomatka
    public Transform[] wayPoints; // Waypointit taulukossa, jotta niitä voi olla useampia
    public Transform eye; // Silmä, josta näkösäde(Raycast) lähtee 
    public MeshRenderer indicator; // Laatikko vihollisen päällä, muuttaa väriä sen mukaan missä tilassa ollaan. Debuggityökalu
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
