using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFlashEffect : MonoBehaviour
{
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    private bool isFlashing = false;
    private bool isStunned = false;
    private float stunCooldownTimer = 0f;
    public float shortFlash = 0.2f;
    
    // Stun and cooldown settings
    [HideInInspector] public float stunCooldown = 4.0f; // Time before enemy can be stunned again
    
    // Components that control movement (we'll disable these during stun)
    private Rigidbody enemyRigidbody;
    private NavMeshAgent enemyNavAgent;
    private Animator enemyAnimator;
    private MonoBehaviour[] enemyScripts; // Store custom enemy scripts
    
    // Original states
    private bool wasKinematic;
    private bool navAgentEnabled;
    private float originalSpeed;
    private Dictionary<MonoBehaviour, bool> scriptStates = new Dictionary<MonoBehaviour, bool>();
    
    // For debugging
    private float stunEndTime = 0f;
    
    private void Awake()
    {
        // Cache components
        enemyRigidbody = GetComponent<Rigidbody>();
        enemyNavAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        
        // Find enemy controller scripts (assumes they inherit from MonoBehaviour)
        // This will get scripts like EnemyController, EnemyMovement, etc.
        enemyScripts = GetComponents<MonoBehaviour>();
    }
    
    private void Update()
    {
        // Check if stun should end based on time
        if (isStunned && Time.time >= stunEndTime)
        {
            EndStun();
        }
        
        // Count down the stun cooldown timer
        if (stunCooldownTimer > 0)
        {
            stunCooldownTimer -= Time.deltaTime;
        }
    }
    
    public void Flash(Color flashColor, float duration)
    {
        if (!isFlashing)
        {
            StartCoroutine(FlashRoutine(flashColor, duration));
        }
    }
    
    public void Stun(float duration)
    {
        // Check if we're already stunned or in cooldown
        if (isStunned || stunCooldownTimer > 0)
        {
            // Can still flash as visual feedback, but won't re-stun
            return;
        }
        
        // Begin the stun immediately
        BeginStun();
        
        // Set when the stun should end
        stunEndTime = Time.time + duration;
        
        // Log for debugging
        Debug.Log(gameObject.name + " is stunned for " + duration + " seconds");
    }
    
    public bool CanBeStunned()
    {
        return !isStunned && stunCooldownTimer <= 0;
    }
    
    public void FlashAndStun(Color flashColor, float flashDuration, float stunDuration)
    {
        // Always flash for visual feedback - now using stun duration for flash duration
        float temp = flashDuration;
        if (!CanBeStunned())
        {
            temp = shortFlash;
            Debug.Log("short flash");
        }
        Flash(flashColor, temp);
        
        // Only stun if not already stunned/in cooldown
        if (CanBeStunned())
        {
            Stun(stunDuration);
        }
    }
    
    private IEnumerator FlashRoutine(Color flashColor, float duration)
    {
        isFlashing = true;
        
        // Get all renderers in this object and its children
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        
        if (renderers.Length == 0)
        {
            Debug.LogWarning("No renderers found on " + gameObject.name);
        }
        
        // Store original materials and create flash materials
        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
            {
                // Store the original materials
                Material[] originalMats = renderer.materials;
                originalMaterials[renderer] = originalMats;
                
                // Create new materials with flash color
                Material[] flashMaterials = new Material[originalMats.Length];
                for (int i = 0; i < originalMats.Length; i++)
                {
                    flashMaterials[i] = new Material(originalMats[i]);
                    flashMaterials[i].color = flashColor;
                }
                
                // Apply flash materials
                renderer.materials = flashMaterials;
            }
        }
        
        // Wait for the specified duration - which is now the same as stun duration
        yield return new WaitForSeconds(duration);
        
        // Reset to original materials
        ResetMaterials();
        isFlashing = false;
    }
    
    private void BeginStun()
    {
        if (!isStunned)
        {
            isStunned = true;
            
            // Store original states and disable movement
            DisableMovement();
        }
    }
    
    private void EndStun()
    {
        if (isStunned)
        {
            // Restore original states
            EnableMovement();
            
            // Enemy is no longer stunned but enters cooldown
            isStunned = false;
            stunCooldownTimer = stunCooldown;
            
            Debug.Log(gameObject.name + " is no longer stunned. Cooldown: " + stunCooldown + " seconds");
        }
    }
    
    private void DisableMovement()
    {
        // Disable Rigidbody movement
        if (enemyRigidbody != null)
        {
            wasKinematic = enemyRigidbody.isKinematic;
            enemyRigidbody.isKinematic = true;
            enemyRigidbody.velocity = Vector3.zero;
        }
        
        // Disable NavMeshAgent
        if (enemyNavAgent != null && enemyNavAgent.enabled)
        {
            navAgentEnabled = enemyNavAgent.enabled;
            originalSpeed = enemyNavAgent.speed;
            enemyNavAgent.speed = 0;
            enemyNavAgent.isStopped = true;
        }
        
        // Pause animation
        if (enemyAnimator != null && enemyAnimator.enabled)
        {
            enemyAnimator.speed = 0;
        }
        
        // Disable custom enemy scripts, but skip this EnemyFlashEffect
        foreach (MonoBehaviour script in enemyScripts)
        {
            if (script != null && script != this && script.enabled)
            {
                scriptStates[script] = script.enabled;
                script.enabled = false;
            }
        }
    }
    
    private void EnableMovement()
    {
        // Re-enable Rigidbody movement
        if (enemyRigidbody != null)
        {
            enemyRigidbody.isKinematic = wasKinematic;
        }
        
        // Re-enable NavMeshAgent
        if (enemyNavAgent != null)
        {
            enemyNavAgent.speed = originalSpeed;
            enemyNavAgent.isStopped = false;
            enemyNavAgent.enabled = navAgentEnabled;
        }
        
        // Resume animation
        if (enemyAnimator != null)
        {
            enemyAnimator.speed = 1;
        }
        
        // Re-enable custom enemy scripts
        foreach (KeyValuePair<MonoBehaviour, bool> scriptState in scriptStates)
        {
            if (scriptState.Key != null)
            {
                scriptState.Key.enabled = scriptState.Value;
            }
        }
        
        // Clear the dictionary
        scriptStates.Clear();
    }
    
    private void ResetMaterials()
    {
        // Restore original materials
        foreach (KeyValuePair<Renderer, Material[]> kvp in originalMaterials)
        {
            // Check if the renderer still exists
            if (kvp.Key != null)
            {
                kvp.Key.materials = kvp.Value;
            }
        }
        
        // Clear the dictionary
        originalMaterials.Clear();
    }
    
    private void OnDestroy()
    {
        // Ensure materials are reset when the component is destroyed
        ResetMaterials();
        
        // Make sure movement is re-enabled if the component is destroyed while stunned
        if (isStunned)
        {
            EnableMovement();
        }
    }
}