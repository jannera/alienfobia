using UnityEngine;

namespace CompleteProject
{
    public class GameOverManager : MonoBehaviour
    {
        PlayerHealth playerHealth;       // Reference to the player's health.

        Animator anim;                          // Reference to the animator component.
		float restartTimer;     				// Timer to count up to restarting the level

        void Awake ()
        {
            // Set up the reference.
            anim = GetComponent <Animator> ();
        }

        void Update ()
        {

			if (playerHealth == null) {
				GameObject player = GameObject.FindWithTag("Player");

				if (player != null) {
					playerHealth = player.GetComponent<PlayerHealth>();
				}
				else {
					return;
				}
			}
            // If the player has run out of health...
            if(playerHealth.currentHealth <= 0)
            {
                // ... tell the animator the game is over.
                anim.SetTrigger ("GameOver");
			
            }
        }
    }
}