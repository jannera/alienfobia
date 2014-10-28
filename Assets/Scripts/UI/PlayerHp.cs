using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class PlayerHp : MonoBehaviour
    {
        public UnityEngine.UI.Slider slider;
        public UnityEngine.UI.Image hitImage;
        int lastHealth;

        public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.

        PlayerHealth playerHealth;

        // Update is called once per frame
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

            slider.value = playerHealth.currentHealth;

            // If the player has just been damaged...
            if (lastHealth != playerHealth.currentHealth)
            {
                // ... set the colour of the damageImage to the flash colour.
                hitImage.color = flashColour;
            }
            // Otherwise...
            else
            {
                hitImage.color = Color.Lerp(hitImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }

            // Reset the damaged flag.
            lastHealth = playerHealth.currentHealth;
        }
    }
}
