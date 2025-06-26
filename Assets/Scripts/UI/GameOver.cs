using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public bool isDead = false;
    private CanvasGroup canvasGroup;
    private SceneController sceneController;

    public void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

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
        if (isDead && canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha = 1f;
            if (!canvasGroup.interactable)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        } else if (!isDead && canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha = 0f;
            if (canvasGroup.interactable)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        } 
    }
    public void StartGame()
    {
        Debug.Log("Back to starting level");
        sceneController.SetAmmo(0);
        sceneController.SetHealth(3);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1Scene");
    }
    public void ExitToMainMenu()
    {
        Debug.Log("Back to main menu");
        sceneController.SetAmmo(0);
        sceneController.SetHealth(3);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
}
