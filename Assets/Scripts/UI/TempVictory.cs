using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempVictory : MonoBehaviour
{
    public bool isVictory = false;
    private CanvasGroup canvasGroup;
    private LevelManager manager;

    private SceneController sceneController;

    public void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        GameObject levelManager = GameObject.Find("LevelManager");
        if (levelManager != null)
        {
            manager = levelManager.GetComponent<LevelManager>();
            if (manager == null)
            {
                Debug.LogWarning("LevelManager does not have LevelManager component");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject 'LevelManager' found.");
        }


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
    }

    public void Update()
    {
        if (manager.collectedFoodCount >= manager.totalFoodCount)
        {
            isVictory = true;
        }
        if (isVictory && canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += 0.01f;
            if (canvasGroup.alpha >= 1f)
            {
                Time.timeScale = 0f;
            }
            if (!canvasGroup.interactable)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        } else if (!isVictory && canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= 0.01f;
            if (canvasGroup.interactable)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        } 
    }
    public void StartGame()
    {
        sceneController.SetAmmo(0);
        sceneController.SetHealth(3);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1Scene");
    }
    public void ExitToMainMenu()
    {
        sceneController.SetAmmo(0);
        sceneController.SetHealth(3);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
}
