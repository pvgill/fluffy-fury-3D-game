using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DogAIGuard : MonoBehaviour
{
    [Header("Targets")]
    public Transform player;
    public Transform food;

    [Header("Guard Settings")]
    public float guardRange = 8f;   // If player is within this distance of food, dog will chase
    public float guardSpeed = 3f;   // Speed while chasing
    public float idleSpeed = 2f;    // Speed while returning/standing guard
    public float slowedSpeed = 1.5f; // Speed while on sap
    public float stopDistance = 2f; // How close dog stands to the food when idle

    private NavMeshAgent agent;
    private Animator anim;
    private Rigidbody rb;

    private bool isSlowed;

    private float guardAngle = 0f;
    private Vector3 lastPlayerPosition;

    private enum DogState { Guarding, Chasing }
    private DogState currentState = DogState.Guarding;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        anim = GetComponentInChildren<Animator>();

        if (anim == null)
        {
            Debug.LogError("No Animator found in children!", this);
        }

        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody found!", this);
        }
    }

    void Update()
    {
        if (player == null || food == null) return;

        float playerToFoodDist = Vector3.Distance(player.position, food.position);

        if (playerToFoodDist <= guardRange && currentState != DogState.Chasing)
        {
            TransitionToState(DogState.Chasing);
        }
        else if (playerToFoodDist > guardRange && currentState != DogState.Guarding)
        {
            TransitionToState(DogState.Guarding);
        }

        switch (currentState)
        {
            case DogState.Chasing:
                Chase();
                break;

            case DogState.Guarding:
                Guard();
                break;
        }

        anim.SetFloat("Speed", agent.velocity.magnitude);

        RaycastHit hit;
        isSlowed = Physics.Raycast(rb.transform.position, Vector3.down, out hit, 2f, 1 << 6);

        if (isSlowed)
        {
            agent.speed = slowedSpeed;
        }
        else
        {
            agent.speed = guardSpeed;
        }
    }

    private void Guard()
    {
        // Dog stays near the food, returning if not close enough
        float distToFood = Vector3.Distance(transform.position, food.position);

        if (distToFood > stopDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(food.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }


    private void Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    private void TransitionToState(DogState newState)
    {
        Debug.Log($"[DogAI] Transition: {currentState} -> {newState}");
        currentState = newState;

        switch (newState)
        {
            case DogState.Guarding:
                agent.speed = idleSpeed;
                break;
            case DogState.Chasing:
                agent.speed = guardSpeed;
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
