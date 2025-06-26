using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int projectileCount = 0; // Starts at 0 until the player picks up projectiles
    public int maxProjectiles = 10; // Maximum projectiles the player can carry

    private HUDController HUDcontr;
    SoundManager soundManager;
    private SceneController sceneController;

    // Animator
    private Animator anim;

    void Awake()
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

        GameObject sceneManager = GameObject.Find("SceneManager");
        if (sceneManager != null)
        {
            sceneController = sceneManager.GetComponent<SceneController>();
            if (sceneController == null)
            {
                Debug.LogWarning("SceneManager does not have SceneController component");
            } else
            {
                projectileCount = sceneController.GetAmmo();
            }
        }
        else
        {
            Debug.LogWarning("No GameObject 'SceneManager' found.");
        }

    }

    private void Start()
    {
        // Animator is in child object
        anim = GetComponentInChildren<Animator>();

        if (anim == null)
        {
            Debug.LogError("No Animator found in children!", this);
        }
    }

    public bool CanShoot()
    {
        return projectileCount > 0;
    }

    public void Shoot(Vector3 position, Vector3 direction, float speed)
    {
        if (CanShoot() && projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            if (projectileRb == null)
            {
                projectileRb = projectile.AddComponent<Rigidbody>();
            }

            //projectileRb.useGravity = false;
            projectile.transform.forward = direction;
            projectileRb.AddForce(direction * speed, ForceMode.Impulse);

            // Play throwing animation
            anim.Play("Squirrel throw", 0, 0f);

            Destroy(projectile, 5f);  // Destroy after 5 seconds (increased from 3 for longer range)
            projectileCount--; // Reduce projectile count after shooting
            HUDcontr.SetAmmoText(projectileCount);
        }
    }

    public void PickupProjectile()
    {
        if (projectileCount < maxProjectiles) // Check if the max limit hasn't been reached
        {
            projectileCount = maxProjectiles;
            HUDcontr.SetAmmoText(projectileCount);

            if (soundManager != null)
            {
                soundManager.PlaySFX(soundManager.pickupProjectile);
            }
            
            Debug.Log("Picked up a projectile! Current count: " + projectileCount);
        }
        else
        {
            Debug.Log("Max projectile limit reached!");
        }
    }
}