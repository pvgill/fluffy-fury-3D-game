using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Array to keep track of all food items in the level
    private GameObject[] foodItems;
    
    // Track how many food items need to be collected
    public int totalFoodCount;
    public int collectedFoodCount = 0;
    
    // Reference to the door game objects
    public GameObject animalsDoor;
    public GameObject humansDoor;
    
    // Optional UI text to show food collection progress
    public UnityEngine.UI.Text foodCountText;

    // Access sound manager for SFX audio effects
    SoundManager soundManager;

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
        // Find all food items in the level
        foodItems = GameObject.FindGameObjectsWithTag("Food");
        totalFoodCount = foodItems.Length;
        
        Debug.Log("Total food items in level: " + totalFoodCount);
        
        // Initially disable door colliders until all food is collected
        if (animalsDoor != null)
        {
            Door animalsDoorScript = animalsDoor.GetComponent<Door>();
            if (animalsDoorScript != null)
            {
                animalsDoorScript.SetActive(false);
            }
        }
        
        if (humansDoor != null)
        {
            Door humansDoorScript = humansDoor.GetComponent<Door>();
            if (humansDoorScript != null)
            {
                humansDoorScript.SetActive(false);
            }
        }
        
        UpdateUI();
    }
    
    public void FoodCollected()
    {
        collectedFoodCount++;
        Debug.Log("Food collected! " + collectedFoodCount + "/" + totalFoodCount);
        
        if (soundManager != null)
        {
            soundManager.PlaySFX(soundManager.collectFood);
        }

        UpdateUI();
        
        // Check if all food has been collected
        if (collectedFoodCount >= totalFoodCount)
        {
            AllFoodCollected();
        }
    }
    
    void UpdateUI()
    {
        // Update UI text if available
        if (foodCountText != null)
        {
            foodCountText.text = "Food: " + collectedFoodCount + " / " + totalFoodCount;
        }
    }
    
    public void AllFoodCollected()
    {
        Debug.Log("All food collected! Doors are now active.");
        
        if (soundManager != null)
        {
            soundManager.PlaySFX(soundManager.doorsUnlocked);
        }

        // Activate both doors
        if (animalsDoor != null)
        {
            Door animalsDoorScript = animalsDoor.GetComponent<Door>();
            if (animalsDoorScript != null)
            {
                animalsDoorScript.SetActive(true);
            }
        }
        
        if (humansDoor != null)
        {
            Door humansDoorScript = humansDoor.GetComponent<Door>();
            if (humansDoorScript != null)
            {
                humansDoorScript.SetActive(true);
            }
        }
    }
}