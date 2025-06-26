using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    public SceneInfo sceneInfo;
    private HUDController HUDcontr;

    private readonly int ammoLimit = 10;
    private readonly int healthLimit = 3;
    private bool damaged = false;

    public void Awake()
    {
        if (sceneInfo == null)
        {
            Debug.LogError("SceneInfo is not assigned in SceneController!");
            return;
        }

         Debug.Log("Player health after reload: " + sceneInfo.playerHealth);

        GameObject hud = GameObject.Find("HUD");
        if (hud != null)
        {
            HUDcontr = hud.GetComponent<HUDController>();
            if (HUDcontr == null)
            {
                Debug.LogWarning("HUD does not have HUDController component");
            }
            else
            {
                HUDcontr.SetAmmoText(sceneInfo.playerAmmo);
            }
        }
        else
        {
            Debug.LogWarning("No GameObject 'HUD' found.");
        }
    }

    public int GetHealth()
    {
        return sceneInfo.playerHealth;
    }

    public int GetAmmo()
    {
        return sceneInfo.playerAmmo;
    }

    public void LoseHealth()
    {
        if (!damaged && sceneInfo.playerHealth > 0)
        {
            damaged = true;
            sceneInfo.playerHealth--;
            Debug.Log("Player lost health. Current health: " + sceneInfo.playerHealth);
        }
        
        if (HUDcontr != null)
        {
            HUDcontr.UpdateHealth();
        }
    }

    public void GainHealth()
    {
        if (sceneInfo.playerHealth < healthLimit)
        {
            sceneInfo.playerHealth++;
            Debug.Log("Player gained health. Current health: " + sceneInfo.playerHealth);
        }

        if (HUDcontr != null)
        {
            HUDcontr.UpdateHealth();
        }
    }

    public void SetHealth(int amount)
    {
        if (amount > healthLimit)
        {
            amount = healthLimit;
        }
        sceneInfo.playerHealth = amount;
        Debug.Log("Player health set to: " + sceneInfo.playerHealth);

        if (HUDcontr != null)
        {
            HUDcontr.UpdateHealth();
        }
    }

    public void SetAmmo(int amount)
    {
        if (amount > ammoLimit)
        {
            amount = ammoLimit;
        }
        sceneInfo.playerAmmo = amount;
        Debug.Log("Player ammo set to: " + sceneInfo.playerAmmo);

        if (HUDcontr != null)
        {
            HUDcontr.SetAmmoText(amount);
        }
    }
    public bool GetDamaged()
    {
        return damaged;
    }
}
