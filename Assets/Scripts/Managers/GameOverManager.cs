using UnityEngine;

namespace CompleteProject
{
    public class GameOverManager : MonoBehaviour
    {
        PlayerHealth playerHealth;       // Reference to the player's health.

        Animator anim;                          // Reference to the animator component.
        float restartTimer;     				// Timer to count up to restarting the level

        void Awake()
        {
            // Set up the reference.
            anim = GetComponent<Animator>();
        }

        void Update()
        {

            if (playerHealth == null)
            {
                playerHealth = PlayerManager.GetComponentFromMyPlayer<PlayerHealth>();

                if (playerHealth == null)
                {
                    return;
                }
            }
            
            if (playerHealth.isDead)
            {
                anim.SetTrigger("GameOver");
            }
        }
    }
}