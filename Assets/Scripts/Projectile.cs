using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    public GameObject hitEffect; // Optional - particle effect when hitting something
    public float stunDuration = 3.0f; // Increased stun duration to 3 seconds
    public Color flashColor = Color.red; // The color to flash
    
    void Start()
    {
        // Get the Rigidbody component
        Rigidbody rb = GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            //rb.drag = 0.1f;
            //rb.useGravity = false;
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // Check if we hit a target
        if (collision.gameObject.CompareTag("Target"))
        {
            // You can add code here to damage targets later
            Debug.Log("Hit a target!");
            
            // Play hit effect if assigned
            if (hitEffect != null)
            {
                Instantiate(hitEffect, collision.contacts[0].point, Quaternion.identity);
            }
        }
        
        // Check if we hit an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Apply damage feedback (red flash)
            Debug.Log("Hit an enemy!");
            
            // Add the FlashEffect component to the enemy
            EnemyFlashEffect flashEffect = collision.gameObject.GetComponent<EnemyFlashEffect>();
            
            // If the enemy doesn't have the FlashEffect component, add it
            if (flashEffect == null)
            {
                flashEffect = collision.gameObject.AddComponent<EnemyFlashEffect>();
                flashEffect.stunCooldown = stunDuration + 1.0f; // Cooldown is stun duration + 1 second
            }
            
            // Check if the enemy can be stunned (for debugging)
            bool canStun = flashEffect.CanBeStunned();
            if (!canStun)
            {
                Debug.Log("Enemy is immune to stun (already stunned or in cooldown)");
            }
            
            // Trigger the flash and stun - flash now lasts for the entire stun duration
            flashEffect.FlashAndStun(flashColor, stunDuration, stunDuration);
            
            // Play hit effect if assigned
            if (hitEffect != null)
            {
                Instantiate(hitEffect, collision.contacts[0].point, Quaternion.identity);
            }
        }
        
        // The projectile will be destroyed after hitting anything
        Destroy(gameObject);
    }
}