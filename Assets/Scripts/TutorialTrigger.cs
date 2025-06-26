using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public enum TutorialType
    {
        Movement,
        Jump,
        Projectile1,
        Projectile2,
        Sap1,
        Sap2,
        FoodPart1,
        FoodPart2,
        Door,
        PauseMenu
    }
    
    public TutorialType tutorialType;
    public bool triggerOnce = true;
    private bool hasTriggered = false;
    
    private TutorialManager tutorialManager;
    
    void Start()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
        
        if (tutorialManager == null)
        {
            Debug.LogWarning("No SimpleTutorialManager found in the scene! Trigger " + gameObject.name + " won't work.");
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (!triggerOnce || !hasTriggered))
        {
            // Only one tutorial at a time - check if one is already active
            if (tutorialManager != null && !tutorialManager.IsTutorialActive)
            {
                TriggerTutorial();
            }
        }
    }
    
    public void TriggerTutorial()
    {
        if (triggerOnce)
        {
            hasTriggered = true;
        }
        
        if (tutorialManager != null)
        {
            switch (tutorialType)
            {
                case TutorialType.Movement:
                    tutorialManager.ShowMovementTutorial();
                    break;
                case TutorialType.Jump:
                    tutorialManager.ShowJumpTutorial();
                    break;
                case TutorialType.Projectile1:
                    tutorialManager.ShowProjectileTutorial1();
                    break;
                case TutorialType.Projectile2:
                    tutorialManager.ShowProjectileTutorial2();
                    break;
                case TutorialType.Sap1:
                    tutorialManager.ShowSapTutorial1();
                    break;
                case TutorialType.Sap2:
                    tutorialManager.ShowSapTutorial2();
                    break;
                case TutorialType.FoodPart1:
                    tutorialManager.ShowFoodTutorialPart1();
                    break;
                case TutorialType.FoodPart2:
                    tutorialManager.ShowFoodTutorialPart2();
                    break;
                case TutorialType.Door:
                    tutorialManager.ShowDoorTutorial();
                    break;
                case TutorialType.PauseMenu:
                    tutorialManager.ShowPauseMenuTutorial();
                    break;
            }
        }
    }
}