using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DogAI : MonoBehaviour
{
    [Header("Targets")]
    public Transform player;
    public Transform food;

    [Header("Distances")]
    public float chaseDistance = 15f;  // distance to start chasing player
    public float guardDistance = 3f;   // distance from food to guard
    public float movementThreshold = 0.1f; // min player movement that triggers reaction

    [Header("Speeds")]
    public float guardSpeed = 2f;
    public float guardSpeedSlowed = 1f;
    public float chaseSpeed = 4.5f;
    public float slowedSpeed = 2.25f;

    [Header("Durations")]
    public float chaseDuration = 3f;
    public float barkDuration = 0.5f;
    public float chaseDelay = 0f; // delay before chasing after a bark

    [Header("Guard Movement")]
    public float guardRadius = 2f;

    [Header("Chase Options")]
    public bool willBarkWhileChasing = true;


    private NavMeshAgent agent;
    private Animator anim;
    private Rigidbody rb;

    private float chaseTimer = 0f;
    private float barkTimer = 0f;
    private float guardAngle = 0f;
    private Vector3 lastPlayerPosition;

    private bool justBarked = false;
    public float barkCooldown = 3f;
    private float barkCooldownTimer = 0f;

    private bool isSlowed;

    private enum DogState { Idle, Barking, Chasing, Guarding }
    private DogState currentState = DogState.Idle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        if (anim == null)
        {
            Debug.LogError("No Animator found in children!", this);
        }

        if (player != null) lastPlayerPosition = player.position;


        DogAI[] allDogs = FindObjectsOfType<DogAI>();
        foreach (DogAI dog in allDogs)
        {
            dog.player = this.player;
            dog.food = this.food;
        }

        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody found!", this);
        }
    }

    void Update()
    {
        if (player == null) return;

        float playerToFoodDist = 1000f;
        if (food != null)
        {
            playerToFoodDist = Vector3.Distance(player.position, food.position);
        }
        float playerToDogDist = Vector3.Distance(player.position, transform.position);
        float playerMovement = Vector3.Distance(player.position, lastPlayerPosition);

        switch (currentState)
        {
            case DogState.Idle:
                Idle(playerMovement, playerToDogDist, playerToFoodDist);
                break;

            case DogState.Barking:
                Barking();
                break;

            case DogState.Chasing:
                Chasing(playerToFoodDist, playerToDogDist);
                break;

            case DogState.Guarding:
                Guarding(playerToFoodDist);
                break;
        }

        lastPlayerPosition = player.position;

        anim.SetFloat("Speed", agent.velocity.magnitude);

        RaycastHit hit;
        isSlowed = Physics.Raycast(rb.transform.position+new Vector3(0,.5f,0), Vector3.down, out hit, 2.5f, 1 << 6);
        if (!isSlowed)
        {
            isSlowed = Physics.Raycast(rb.transform.position + new Vector3(0, .5f, 1), Vector3.down, out hit, 2.5f, 1 << 6);
        }

        if (isSlowed)
        {
            agent.speed = slowedSpeed;
            Debug.Log("Slowed Dog");
        }
        else
        {
            agent.speed = chaseSpeed;
        }
    }

    private void Idle(float playerMovement, float playerToDogDist, float playerToFoodDist)
    {
        if (justBarked)
        {
            // Count down barkCooldown
            barkCooldownTimer += Time.deltaTime;
            if (barkCooldownTimer >= barkCooldown)
            {
                justBarked = false;
                barkCooldownTimer = 0f;
            }
        }
        else if (playerMovement > movementThreshold && playerToDogDist < chaseDistance)
        {
            TransitionToState(DogState.Barking);
        }
        else if (playerToFoodDist < guardDistance)
        {
            TransitionToState(DogState.Guarding);
        }
        else
        {
            agent.isStopped = true;
        }
    }

    private void Barking()
    {
        barkTimer += Time.deltaTime;
        agent.isStopped = true; // dog doesn't move while barking

        anim.Play("Dog bark", 0, 0f);

        if (barkTimer >= barkDuration)
        {
            // After finishing bark, wait a bit and then chase
            StartCoroutine(StartChaseAfterDelay());

            TransitionToState(DogState.Chasing);
        }
    }

    private IEnumerator StartChaseAfterDelay()
    {
        yield return new WaitForSeconds(chaseDelay);

        if (currentState != DogState.Chasing)
        {
            TransitionToState(DogState.Chasing);
        }
    }

    private void Chasing(float playerToFoodDist, float playerToDogDist)
    {
        chaseTimer += Time.deltaTime;
        agent.isStopped = false;
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        // Guard as soon as the player nears food
        if (playerToFoodDist < guardDistance)
        {
            TransitionToState(DogState.Guarding);
            return;
        }

        // Bark in between chasing
        if (chaseTimer >= chaseDuration && willBarkWhileChasing)
        {
            TransitionToState(DogState.Barking);
            return;
        }

        if (playerToDogDist > chaseDistance + 2f)
        {
            TransitionToState(DogState.Idle);
            return;
        }
    }

    private void Guarding(float playerToFoodDist)
    {
        agent.isStopped = false;
        if (isSlowed)
        {
            agent.speed = guardSpeedSlowed;

        } else
        {
            agent.speed = guardSpeed;

        }

        // Circle around the food
        guardAngle += Time.deltaTime * guardSpeed;
        float x = food.position.x + Mathf.Cos(guardAngle) * guardRadius;
        float z = food.position.z + Mathf.Sin(guardAngle) * guardRadius;
        Vector3 guardPos = new Vector3(x, transform.position.y, z);

        agent.SetDestination(guardPos);

        // If player leaves guard area, Idle or chase
        if (food == null || playerToFoodDist >= guardDistance)
        {
            TransitionToState(DogState.Idle);
        }
    }

    private void TransitionToState(DogState newState)
    {
        Debug.Log($"[{Time.time:F2}] Dog {name}: {currentState} -> {newState}");

        switch (currentState)
        {
            case DogState.Chasing:
                chaseTimer = 0f;
                break;
            case DogState.Barking:
                barkTimer = 0f;
                break;
        }

        currentState = newState;

        switch (newState)
        {
            case DogState.Barking:
                barkTimer = 0f;
                justBarked = true;
                break;
            case DogState.Chasing:
                chaseTimer = 0f;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerLife = FindObjectOfType<PlayerLifeManager>();
            if (playerLife != null)
            {
                playerLife.LoseLife();
            }
        }
    }
}
