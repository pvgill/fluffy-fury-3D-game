using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    // UI elements
    public GameObject tutorialPanel;
    public Text tutorialText;
    public Button nextButton;
    
    private bool isTutorialActive = false;
    
    void Start()
    {
        // Hide tutorial panel initially
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
        
        // Setup button listener
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonClicked);
        }
        
        // Show welcome message
        StartCoroutine(ShowWelcomeMessage());
    }
    
    IEnumerator ShowWelcomeMessage()
    {
        yield return new WaitForSeconds(0.5f);
        ShowTutorial("Welcome to the game! Use WASD or arrow keys to move around.");
    }
    
    void OnNextButtonClicked()
    {
        // Just hide the panel when Next is clicked
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
            isTutorialActive = false;
        }
    }
    
    // Main method to show a tutorial message
    public void ShowTutorial(string message)
    {
        if (isTutorialActive)
            return; // Don't show a new tutorial if one is already active
            
        isTutorialActive = true;
        
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }
        
        if (tutorialText != null)
        {
            tutorialText.text = message;
        }
    }
    
    // Public methods to show specific tutorials
    public void ShowMovementTutorial() 
    {
        ShowTutorial("Use WASD or arrow keys to move around.");
    }
    
    public void ShowJumpTutorial() 
    {
        ShowTutorial("Press SPACE to jump over obstacles!");
    }
    
    public void ShowProjectileTutorial1() 
    {
        ShowTutorial("Left-click to throw projectiles at targets and enemies! Try aiming at the wall ahead.");
    }

    public void ShowProjectileTutorial2() 
    {
        ShowTutorial("Try aiming at the wall ahead. When done, try using your Sap ability ahead.");
    }
    
    public void ShowSapTutorial1() 
    {
        ShowTutorial("Sap is a special projectile that slows down enemies when thrown on the ground. Press F to use Sap.");
    }

    public void ShowSapTutorial2() 
    {
        ShowTutorial("Press F to use Sap.");
    }
    
    public void ShowFoodTutorialPart1() 
    {
        ShowTutorial("Collect all food items to unlock the exit doors!");
    }
    
    public void ShowFoodTutorialPart2() 
    {
        ShowTutorial("Good! The doors should be unlocked now.");
    }
    
    public void ShowDoorTutorial() 
    {
        ShowTutorial("The door will turn green when unlocked.");
    }
    
    public void ShowPauseMenuTutorial() 
    {
        ShowTutorial("Press ESC at any time to open the pause menu. Use it to resume, return to the main menu or quit game.");
    }
    
    // Add this property to let other scripts check if a tutorial is active
    public bool IsTutorialActive
    {
        get { return isTutorialActive; }
    }
}