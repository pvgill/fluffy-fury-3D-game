using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanPatrol : MonoBehaviour
{

    public NavMeshAgent agent;
    private Animator anim;

    [Header("WAYPOINTS---------")]
    public Transform waypointRoot;
    public Transform[] waypoints;
    public int waypointIndex;

    [Header("RADIUS SETTINGS---------")]
    public float detectionRadius = 6f;
    public float chasingRadius = 6f;
    public float chasingBuffer = 0.5f;
    private Transform player;

    [Header("SPEED SETTINGS---------")]
    public float humanSpeed = 5f;
    public float humanSpeedSlowed = 2.25f;
    public float rotationSpeed = 150f; // temp var that is changed based on whether or not there's sap
    public float rotationSpeedOriginal = 150f;
    public float rotationSpeedSlowed = 75f;

    private enum AIState { Patrol, Detecting, Chasing }
    private AIState currentState = AIState.Patrol;

    private CapsuleCollider cc;
    private bool isSlowed;

    private float detectTimer = 0f;
    public float chaseReactionTime = 0.8f;

    void Start()
     {
        // Load all the waypoints from the children of waypointRoot into a list
        List<Transform> waypointList = new List<Transform>();
        foreach (Transform child in waypointRoot)
        {
            waypointList.Add(child);
        }
        waypoints = waypointList.ToArray();
        Debug.Log("Waypoints loaded: " + waypoints.Length);

        //Randomize the waypoints order
        ShuffleWaypoints();

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        //debugging animation, NavMeshAgent, and Player tag
        if (anim == null) {
             Debug.LogError("No Animator found in children!", this);
         }

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is not found!", this);
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object with tag 'Player' not found in the scene!", this);
        }

        //rb = GetComponentInChildren<Rigidbody>(); 

        //if (rb == null)
        //{
        //    Debug.LogError("No Rigidbody found in children!", this);
        //}

        CapsuleCollider[] colliders = GetComponentsInChildren<CapsuleCollider>();

        foreach (CapsuleCollider collider in colliders)
        {
            if (collider.gameObject.name == "body main")
            {
                Debug.Log("Found CapsuleCollider: " + collider.gameObject.name);

                cc = collider;

                break;
            }
        }

        // Start moving toward the first waypoint
        WaypointPatrol();
    }

     void Update()
     {

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case AIState.Patrol:
                // If the player is close enough, stop patrolling and detect the player
                if (distanceToPlayer <= detectionRadius)
                {
                    currentState = AIState.Detecting;
                    detectTimer = 0f;
                    agent.SetDestination(transform.position);
                    anim.SetFloat("Speed", 0.1f);
                    //agent.isStopped = true;
                    //anim.SetFloat("Speed", 0);
                }
                // If the human reaches its current waypoint, go to the next one
                else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    WaypointPatrol();
                }

                //animation speed 
                float speed = agent.velocity.magnitude;
                anim.SetFloat("Speed", speed);
                break;

            case AIState.Detecting:
                detectTimer += Time.deltaTime; 
                RotateHuman();
                // If the player leaves the radius, human goes back to patrolling
                /*if (distanceToPlayer > detectionRadius) 
                {
                    currentState = AIState.Patrol;
                    agent.isStopped = false;
                    WaypointPatrol();
                } */

                //This moved to chasing player after detecting by using new state, Chasing
                if (detectTimer >= chaseReactionTime)
                {
                    currentState = AIState.Chasing;
                    agent.isStopped = false;
                }

                //if a player is away from radius by running away from chasing, human goes back to patrol state
                if (distanceToPlayer > detectionRadius + chasingBuffer)
                {
                    detectTimer = 0f;
                    currentState = AIState.Patrol;
                    agent.isStopped = false;
                    WaypointPatrol();
                }
                break;

            case AIState.Chasing:
                agent.SetDestination(player.position);
                anim.SetFloat("Speed", agent.velocity.magnitude);
                if (distanceToPlayer > chasingRadius + chasingBuffer)
                {
                    currentState = AIState.Patrol;
                    WaypointPatrol();
                }
                break;
        }

        RaycastHit hit;
        isSlowed = Physics.Raycast(cc.transform.position + new Vector3(0, .5f, 0), Vector3.down, out hit, 4f, 1 << 6);

        if (isSlowed)
        {
            agent.speed = humanSpeedSlowed;
            rotationSpeed = rotationSpeedSlowed;
            Debug.Log("SLOWED");
        }
        else
        {
            agent.speed = humanSpeed;
            rotationSpeed = rotationSpeedOriginal;
        }
    }

    void WaypointPatrol()
    {
        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[waypointIndex].position);

        //Go to the next waypoint and after reaching to last waypoint, loop back
        waypointIndex++;
        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
    }

    // Randomize the order of waypoints
    void ShuffleWaypoints()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, waypoints.Length);
            Transform temp = waypoints[i];
            waypoints[i] = waypoints[randomIndex];
            waypoints[randomIndex] = temp;
        }
    }

    //human with a cone rotates to scan around 360 degrees of the area
    void RotateHuman()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with Player, triggering life loss.");
            var playerLife = FindObjectOfType<PlayerLifeManager>();
            if (playerLife != null)
            {
                playerLife.LoseLife();
            }
        }
    }
}


