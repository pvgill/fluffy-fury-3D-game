using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{

    public TextMeshProUGUI countText;
    public GameObject sceneManager;
    public GameObject player;
    private SceneController sceneController;

    public GameObject heartTwo;
    public GameObject heartThree;

    private CanvasGroup HUDcanvas;

    private int currentAmmo;


    private void Awake()
    {
        sceneController = sceneManager.GetComponent<SceneController>();

        HUDcanvas = GetComponent<CanvasGroup>();
        HUDcanvas.interactable = false;
        HUDcanvas.blocksRaycasts = false;

        if (sceneController.GetHealth() > 1)
        {
            heartTwo.SetActive(true);
        } else
        {
            heartTwo.SetActive(false);
        }
        if (sceneController.GetHealth() > 2)
        {
            heartThree.SetActive(true);
        } else
        {
            heartThree.SetActive(false);
        }
    }

    public void SetAmmoText(int ammo)
    {
        countText.text = "Ammo: " + ammo;
        currentAmmo = ammo;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public void UpdateHealth()
    {
        if (sceneController.GetHealth() == 2)
        {
            heartTwo.SetActive(true);
            heartThree.SetActive(true);
        } else if (sceneController.GetHealth() == 1)
        {
            heartTwo.SetActive(true);
            heartThree.SetActive(false);
        }
        else if (sceneController.GetHealth() == 1)
        {
            HUDcanvas.alpha = 0f;
        }
    }
}
