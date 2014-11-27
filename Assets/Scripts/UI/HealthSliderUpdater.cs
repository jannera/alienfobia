using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class HealthSliderUpdater : MonoBehaviour
    {
        public UnityEngine.UI.Slider slider;
        public UnityEngine.UI.Image hitImage;
        int lastHealth;

        public float flashSpeed = 5f;
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

        PlayerHealth playerHealth;

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

            if (lastHealth != playerHealth.currentHealth)
            {
                hitImage.color = flashColour;
            }
            else
            {
                hitImage.color = Color.Lerp(hitImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }

            lastHealth = playerHealth.currentHealth;
        }
    }
}
