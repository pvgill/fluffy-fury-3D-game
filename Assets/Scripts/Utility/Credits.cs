using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Menu toggle needs menu to function
[RequireComponent(typeof(CanvasGroup))]
public class Credits : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Animator anim;
    public GameObject mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null ) 
        {
            Debug.LogError("Could not get canvas group");
        }
        anim = GetComponentInChildren<Animator>();
        if (anim == null)
        {
            Debug.LogError("Could not get animator");
        }
        if (mainMenu == null)
        {
            Debug.LogError("Could not get main menu");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && anim.GetBool("playing"))
        {
            EndCredits();
        }
    }
    public void EndCredits()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;

        anim.ResetTrigger("Rising");
        anim.SetBool("playing", false);

        mainMenu.SetActive(true);

    }
}
