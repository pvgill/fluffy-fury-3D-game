using UnityEngine;

public class ProjectilePickup : MonoBehaviour
{
    public float rotationSpeed = 50f; // How fast the pickup rotates
    public float bobSpeed = 1f; // How fast the pickup moves up and down
    public float bobHeight = 0.5f; // How high the pickup moves up and down
    
    private Vector3 startPosition;
    
    void Start()
    {
        // Store the initial position
        startPosition = transform.position;
        
        // Make sure this object has the correct tag for the player to detect
        gameObject.tag = "ProjectilePickup";
    }
    
    void Update()
    {
        // Rotate the pickup
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
        // Make the pickup bob up and down
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}