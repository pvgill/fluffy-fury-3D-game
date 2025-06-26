using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    // Animator
    private Animator anim;

    // Movement variables
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private bool isGrounded;
    private int groundCount = 0;
    private Rigidbody rb;
    public float extraGravity = 1f;
    
    // Projectile variables
    public Transform shootPoint;
    public float projectileSpeed = 10f;
    private ProjectileManager projectileManager;
    
    // Aiming line variable
    public LineRenderer aimLine;

    // Access sound manager for SFX audio effects
    SoundManager soundManager;
    
    // Reference to the TutorialManager
    private TutorialManager tutorialManager;

    // Initialize the SoundManager reference
    void Awake()
    {
        GameObject soundObj = GameObject.FindGameObjectWithTag("Sound");
        if (soundObj != null)
        {
            soundManager = soundObj.GetComponent<SoundManager>();
            if (soundManager == null)
            {
                Debug.LogWarning("GameObject with 'Sound' tag found, but it doesn't have a SoundManager component. Sound effects will be disabled.");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject with 'Sound' tag found. Sound effects will be disabled.");
        }
    }

    void Start()
    {
        // Animator is in child object
        anim = GetComponentInChildren<Animator>();

        if (anim == null) {
            Debug.LogError("No Animator found in children!", this);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        rb = GetComponent<Rigidbody>();
        
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Configure the Rigidbody to prevent wobbling
        //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smoother movement

        if (shootPoint == null)
        {
            GameObject shootPointObj = new GameObject("ShootPoint");
            shootPointObj.transform.parent = transform;
            shootPointObj.transform.localPosition = new Vector3(0, 0, 1);
            shootPoint = shootPointObj.transform;
        }
        
        if (aimLine == null)
        {
            GameObject aimLineObj = new GameObject("AimLine");
            aimLine = aimLineObj.AddComponent<LineRenderer>();
            aimLine.startWidth = 0.05f;
            aimLine.endWidth = 0.05f;
            aimLine.positionCount = 2;
            aimLine.material = new Material(Shader.Find("Sprites/Default"));
            aimLine.startColor = Color.red;
            aimLine.endColor = Color.red;
        }

        projectileManager = GetComponent<ProjectileManager>();
        
        // Find the tutorial manager
        tutorialManager = FindObjectOfType<TutorialManager>();
        
        // Make sure the player has the Player tag
        gameObject.tag = "Player";
        
        // Ensure the player is standing upright at start
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
    
    void Update()
    {
        // Check if tutorial is active
        bool isTutorialActive = (tutorialManager != null) ? tutorialManager.IsTutorialActive : false;
        
        // Only process movement input if no tutorial is active
        if (!isTutorialActive)
        {
            // Handle movement only when no tutorial is active
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            
            Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
            
            if (movement.magnitude > 1)
            {
                movement.Normalize();
            }
            
            Vector3 newVelocity = Quaternion.AngleAxis(-45, Vector3.up) * movement * moveSpeed;
            newVelocity.y = rb.velocity.y;
            rb.velocity = newVelocity;
            
            // Update animator speed parameter
            float speed;
            if (newVelocity.x != 0)
            {
                speed = Math.Abs(newVelocity.x);
            }
            else if (newVelocity.z != 0)
            {
                speed = Math.Abs(newVelocity.z);
            }
            else
            {
                speed = 0;
            }
            
            // Set animator parameter
            anim.SetFloat("Speed", speed);
            
            // Handle jumping
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("Grounded: " + isGrounded);
                Debug.Log("GameObject height: " + rb.transform.position);
                
                if (!isGrounded)
                {
                    RaycastHit hit;
                    isGrounded = (Physics.Raycast(rb.transform.position, Vector3.down, out hit, 3.5f, 1 << 3));
                }
                
                if (isGrounded)
                {
                    Jump();
                    if (soundManager != null)
                    {
                        soundManager.PlaySFX(soundManager.jump); //added SFX jump sound effect
                    }
                }
            }
            
            // Check for click on UI elements
            bool clickedOnUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
            
            // Only handle shooting if not clicking on UI and game is not paused
            if (Input.GetButtonDown("Fire1") && !clickedOnUI && Time.timeScale != 0f)
            {
                ShootProjectile();
            }
            
            // Handle cursor aim rotation
            RotateTowardsCursor();
        }
        else
        {
            // If tutorial is active, stop all movement
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            
            // Set animator to idle
            anim.SetFloat("Speed", 0);
        }
        
        // Always update the aim line regardless of tutorial state
        UpdateAimLine();
    }
    
    void Jump()
    {
        //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        rb.velocity = new Vector3(rb.velocity[0], jumpForce, rb.velocity[2]);
        isGrounded = false;
    }
    
    void ShootProjectile()
    {
        if (projectileManager.CanShoot())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 targetPoint;
            
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(20);
            }
            
            Vector3 shootDirection = (targetPoint - shootPoint.position).normalized;
            projectileManager.Shoot(shootPoint.position, shootDirection, projectileSpeed);
            
            if (soundManager != null)
            {
                soundManager.PlaySFX(soundManager.shootProjectile); //added SFX projectile shooting effect
            }
        }
        else
        {
            Debug.LogWarning("No projectiles available to throw!");
            
            if (soundManager != null)
            {
                soundManager.PlaySFX(soundManager.noProjectile); //added SFX when running out of projectiles 
            }
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * rb.mass * extraGravity);
    }

    void RotateTowardsCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 lookDirection = (hit.point - transform.position).normalized;
            lookDirection.y = 0;
            transform.forward = lookDirection;
        }
    }
    
    void UpdateAimLine()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;
        
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(20);
        }
        
        aimLine.SetPosition(0, shootPoint.position);
        aimLine.SetPosition(1, targetPoint);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ProjectilePickup"))
        {
            projectileManager.PickupProjectile();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Food"))
        {
            Food food = other.gameObject.GetComponent<Food>();
            if (food != null)
            {
                // Call the OnCollected method to notify the level manager
                food.OnCollected();
            }
            
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            groundCount++;
            if (groundCount > 0)
            {
                isGrounded = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            groundCount--;
            if (groundCount <= 0)
            {
                isGrounded = false;
            }
        }
    }
}