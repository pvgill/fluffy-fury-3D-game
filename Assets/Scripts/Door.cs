using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string targetSceneName; // Name of the scene this door leads to
    public string doorLabel; // "Animals" or "Humans"

    public SceneController sceneController;
    public HUDController HUDcontr;

    // Material colors for door states
    public Material inactiveMaterial; // Gray/Red when door is inactive
    public Material activeMaterial; // Green when door is active
    
    private Renderer doorRenderer;
    public bool isActive = false;
    private TextMesh labelText;

    // Access sound manager for SFX audio effects
    SoundManager soundManager;

    //Stops multiple triggers to make the sound only play once  
    private bool isTransitioning = false;

    void Start()
    {
        // Get the renderer component
        doorRenderer = GetComponent<Renderer>();
        if (doorRenderer != null && inactiveMaterial != null)
        {
            doorRenderer.material = inactiveMaterial;
        }
    }

    private void Awake()
    {
        GameObject sceneManager = GameObject.Find("SceneManager");
        if (sceneManager != null)
        {
            sceneController = sceneManager.GetComponent<SceneController>();
            if (sceneController == null)
            {
                Debug.LogWarning("SceneManager does not have SceneController component");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject 'SceneManager' found.");
        }

        GameObject hud = GameObject.Find("HUD");
        if (hud != null)
        {
            HUDcontr = hud.GetComponent<HUDController>();
            if (HUDcontr == null)
            {
                Debug.LogWarning("HUD does not have HUDController component");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject 'HUD' found.");
        }

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

    public void SetActive(bool active)
    {
        isActive = active;
        
        // Change the door material based on active state
        if (doorRenderer != null)
        {
            Material newMaterial = isActive ? activeMaterial : inactiveMaterial;
            
            if (newMaterial != null)
            {
                doorRenderer.material = newMaterial;
                Debug.Log("Door " + doorLabel + " changed material to " + (isActive ? "active" : "inactive"));
            }
            else
            {
                Debug.LogWarning("Door material is null! Check the door's inspector.");
            }
        }
        else
        {
            Debug.LogWarning("Door renderer is null! Check the door's inspector.");
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Door open trigger entered by: " + other.name);

        if (isTransitioning) return;

        // Only respond to player
        if (other.CompareTag("Player") && isActive)
        {
            isTransitioning = true;

            Debug.Log("Player entered " + doorLabel + " door! Loading scene: " + targetSceneName);
            
            // Load the target scene
            if (!string.IsNullOrEmpty(targetSceneName))
            {
                sceneController.SetAmmo(HUDcontr.GetCurrentAmmo()) ;
                //SceneManager.LoadScene(targetSceneName);

                if (soundManager != null)
                {
                    soundManager.PlaySFX(soundManager.doorOpen); //added SFX door open sound effect
                }

                Invoke("LoadTargetScene", 0.6f);

            }
        }
        else if (other.CompareTag("Player") && !isActive)
        {
            // Player tried to use door but hasn't collected all food
            Debug.Log("Can't use the door yet! Collect all food first.");
            
            // Here you could show a message to the player
        }
    }

    void LoadTargetScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}