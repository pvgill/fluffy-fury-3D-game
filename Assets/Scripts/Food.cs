using UnityEngine;

public class Food : MonoBehaviour
{
    public float rotationSpeed = 40f; // How fast the food rotates
    public float bobSpeed = 1.2f; // How fast the food moves up and down
    public float bobHeight = 0.3f; // How high the food moves up and down
    
    private Vector3 startPosition;
    private LevelManager levelManager;
    private bool collected = false;
    
    void Start()
    {
        // Store the initial position
        startPosition = transform.position;
        
        // Make sure this object has the correct tag
        gameObject.tag = "Food";
        
        // Find the level manager
        levelManager = FindObjectOfType<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogWarning("No LevelManager found in the scene!");
        }
    }
    
    void Update()
    {
        // Rotate the food item
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        
        // Make the food bob up and down
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    
    // This method is called when the food is collected
    public void OnCollected()
    {

        // Notify the level manager
        if (levelManager != null && !collected)
        {
            collected = true;
            levelManager.FoodCollected();
        }
    }
}