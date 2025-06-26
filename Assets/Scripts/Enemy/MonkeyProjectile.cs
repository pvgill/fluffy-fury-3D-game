using UnityEngine;

public class MonkeyProjectile : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //find the PlayerLifeManager script on the player
            PlayerLifeManager playerLife = FindObjectOfType<PlayerLifeManager>();

            if (playerLife != null)
            {
                //call the method to reduce the player's life
                playerLife.LoseLife();
                Debug.Log("Player hit! Life lost.");
            }
        } 
        else if (!collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }


    }
}
