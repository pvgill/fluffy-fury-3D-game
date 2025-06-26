using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public CanvasGroup credits;
    private Animator creditsAnim;
    public GameObject startMenu;
    public GameObject mainMenu;

    private SceneController sceneController;

    public void Awake()
    {
        creditsAnim = credits.GetComponent<Animator>();


        if (creditsAnim == null)
        {
            Debug.LogError("Could not get animator");
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
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (startMenu.activeSelf)
            {
                ToggleStartMenu();
            }
        }
    }
    public void ToggleStartMenu()
    {
        foreach (GameObject item in new List<GameObject> { mainMenu,startMenu})
        {
            item.SetActive(!item.activeSelf);
        }

    }
    public void StartGame()
    {
        sceneController.SetAmmo(0);
        sceneController.SetHealth(3);
        SceneManager.LoadScene("Level1Scene");
        Time.timeScale = 1f;
    }
    public void StartTutorial()
    {
        sceneController.SetAmmo(0);
        sceneController.SetHealth(3);
        SceneManager.LoadScene("TutorialLevelScene");
        Time.timeScale = 1f;
    }
    public void StartCredits()
    {

        credits.interactable = true;
        credits.blocksRaycasts = true;
        credits.alpha = 1f;
        Time.timeScale = 1f;
        creditsAnim.SetTrigger("Rising");
        creditsAnim.SetBool("playing", true);

        mainMenu.SetActive(false);
    }
}
