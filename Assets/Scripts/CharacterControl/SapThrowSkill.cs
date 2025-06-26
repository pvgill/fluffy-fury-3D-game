using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SapThrowSkill : MonoBehaviour
{
    public GameObject sapPrefab;
    public KeyCode sapKey = KeyCode.F;
    public int sapCost = 2;
    public Vector3 sapLocation = new Vector3(0, 0, 0);
    private ProjectileManager projectile;

    private Animator anim;
    private HUDController HUDcontr;
    SoundManager soundManager;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        projectile = GetComponent<ProjectileManager>();

        if (projectile == null)
        {
            Debug.LogWarning("ProjectileManager not found!");
        }

        anim = GetComponentInChildren<Animator>();

        if (anim == null)
        {
            Debug.LogError("No Animator found in children!", this);
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

        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody found!", this);
        }
    }
    void Update()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(rb.transform.position, Vector3.down, out hit, 9999999f, 1 << 3);

        if (Input.GetKeyDown(sapKey) && projectile.projectileCount >= sapCost && isGrounded)
        {
            float downDistance = hit.distance;
            DropSap(downDistance);
            projectile.projectileCount -= sapCost;
            HUDcontr.SetAmmoText(projectile.projectileCount);
        }
    }

    void DropSap(float downDistance)
    {
        if (sapPrefab != null)
        {
            Vector3 sapPosition = transform.position + transform.forward * sapLocation.z +
                                    transform.up * (sapLocation.y - downDistance + 0.1f) +
                                    transform.right * sapLocation.x;

            // Spawn the sap area
            Instantiate(sapPrefab, sapPosition, Quaternion.identity);

            // Play throwing animation
            anim.Play("Squirrel throw", 0, 0f);

            if (soundManager != null)
            {
                soundManager.PlaySFX(soundManager.dropSap);
            }
        }
        else
        {
            Debug.LogWarning("Sap Prefab is not assigned!");
        }
    }
}
