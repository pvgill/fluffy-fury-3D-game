using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management
using System.Collections;

public class PlayerLifeManager : MonoBehaviour
{
    public SceneController sceneController; //reference to SceneController
    public GameObject caughtCone; //reference to human's caughtCone
    private DogAI[] allDogs; //array to store all dog instances
    public Transform player;
    private FadeOut fade;

    private GameOver gameOver;
    SoundManager soundManager;

    void Start()
    {
        allDogs = FindObjectsOfType<DogAI>();
    }

    private void Awake()
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

        GameObject gameOverUI = GameObject.Find("GameOver");
        if (gameOverUI != null)
        {
            gameOver = gameOverUI.GetComponentInChildren<GameOver>();
            if (gameOver == null)
            {
                Debug.LogWarning("Game Over UI does not have GameOver component");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject 'GameOver' found.");
        }
        GameObject fadeOutUI = GameObject.Find("FadeOut");
        if (fadeOutUI != null)
        {
            fade = fadeOutUI.GetComponentInChildren<FadeOut>();
            if (fade == null)
            {
                Debug.LogWarning("Fade Out UI does not have FadeOut component");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject 'FadeOut' found.");
        }
    }

    void Update()
    {
        //check if the player is caught by the human or the dog
        if (caughtCone.activeSelf || IsPlayerCaughtByDog())
        {
            LoseLife();
        }
    }
    private bool IsPlayerCaughtByDog()
    {
        //check if any dog is close enough to attack the player
        foreach (var dog in allDogs)
        {
            float distance = Vector3.Distance(dog.transform.position, player.position);
            if (distance < 1.5f) //attack range
            {
                return true;
            }
        }
        return false;
    }


    public void LoseLife()
    {
        Debug.Log("Player lost a life!");
        if (!sceneController.GetDamaged())
        {
            sceneController.LoseHealth();

            int currentHealth = sceneController.GetHealth();
            PlayerPrefs.SetInt("PlayerHealth", currentHealth);

            //SFX when a player lose a heart 
            if (soundManager != null)
            {
                soundManager.PlaySFX(soundManager.loseLife);
            }

            fade.startFade = true;
            Time.timeScale = 0f;
            
            // Check if the player has no lives left
            if (currentHealth <= 0)
            {
                StartCoroutine(EndGame());
            }
            else
            {
                StartCoroutine(RespawnPlayer());
            }

        }
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        Debug.Log("Game Over!");
        gameOver.isDead = true;
        fade.SelfTerminate();
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        Debug.Log("Respawning player!");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
