using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MonkeyAI : MonoBehaviour
{
    [Header("Targets")]
    public Transform player; 
    public float RotationSpeed = 10f;
    //public Transform[] foodItems; 

    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootForce = 10f;
    public float shootInterval = 2f;
    public float shootDistance = 5f;

    private bool shotReady = true;

    [Header("Jumping Settings")]
    public float jumpDuration = 1f;

    [Header("Idle Settings")]
    public float idleDurationMin = 4f;
    public float idleDurationMax = 7f;

    private NavMeshAgent agent;
    private Animator anim;
    private MonkeyState currentState = MonkeyState.Idle;
    private float stateTimer = 0f;

    private float idleRotationSpeed = 30f;  
    private float idleRotationAmount = 45f;

    //index for assigned food item per monkey
    public int assignedFoodIndex;

    private enum MonkeyState { Idle, Jumping, Shooting }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        //assign a random food item to this monkey
        /*
        if (foodItems.Length > 0)
        {
            assignedFoodIndex = Random.Range(0, foodItems.Length); 
        }
        */

        //start
        TransitionToState(MonkeyState.Idle);
    }

    void Update()
    {
        stateTimer -= Time.deltaTime;

        //float distanceToFood = Vector3.Distance(transform.position, foodItems[assignedFoodIndex].position);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case MonkeyState.Idle:
                
                RotateIdle();

                if (distanceToPlayer < shootDistance)  //player near monkey
                {
                    TransitionToState(MonkeyState.Shooting);
                }
                else if (stateTimer <= 0f)
                    TransitionToState(MonkeyState.Jumping);
                break;

            case MonkeyState.Jumping:
                if (distanceToPlayer < shootDistance)
                {
                    // look at player
                    RotateTowardPlayer();
                }
                if (stateTimer <= 0f)
                {
                    if (distanceToPlayer < shootDistance)  //player near monkey
                    {
                        TransitionToState(MonkeyState.Shooting);
                    }
                    else
                    {
                        TransitionToState(MonkeyState.Idle);
                    }
                }
                break;

            case MonkeyState.Shooting:
                // look at player
                RotateTowardPlayer();

                if (distanceToPlayer < shootDistance && shotReady)  //player near monkey
                {
                    anim.SetTrigger("StartShoot");
                    ShootProjectile();
                    shotReady = false;
                }
                else if (stateTimer <= 0f)
                { // jump between shots
                    shotReady = true;
                    TransitionToState(MonkeyState.Jumping);
                }
                break;
        }
    }


    private void ShootProjectile()
    {
        if (projectilePrefab && shootPoint)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb)
            {
                //shoot towards player
                Vector3 direction = (player.position - shootPoint.position).normalized;
                rb.AddForce(direction * shootForce, ForceMode.Impulse);
            }
        }
    }

    void TransitionToState(MonkeyState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case MonkeyState.Idle:
                stateTimer = Random.Range(idleDurationMin, idleDurationMax);
                anim.SetTrigger("BackToIdle");  
                break;
            case MonkeyState.Jumping:
                stateTimer = jumpDuration;
                anim.SetTrigger("StartJump"); 
                break;
            case MonkeyState.Shooting:
                stateTimer = shootInterval;
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
                playerLife.LoseLife();  //player loses life on collision with monkey
            }
        }
    }

    private void RotateTowardPlayer()
    {
        // https://discussions.unity.com/t/how-do-i-rotate-an-object-towards-a-vector3-point/42488
        //find the vector pointing from our position to the target
        Vector3 direction = (player.position - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction[0], 0f, direction[2])) * Quaternion.Euler(Vector3.up * -90);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
    }

    private void RotateIdle()
    {
        float rotationAngle = Mathf.Sin(Time.time * idleRotationSpeed) * idleRotationAmount;
        Quaternion lookRotation = Quaternion.Euler(0f, rotationAngle, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
    }
}
