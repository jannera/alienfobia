using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CompleteProject
{
    public class ReviveTextController : MonoBehaviour
    {
        private PlayerHealth health;
        private Text text;

        void Awake()
        {
            text = GetComponent<Text>();
            HideText();
        }

        void Update()
        {
            if (health == null)
            {
                health = PlayerManager.GetComponentFromMyPlayer<PlayerHealth>();
                if (health == null)
                {
                    return;
                }
            }

            if (health.isDowned && !health.isDead && PlayerManager.AreAnyPlayersAlive())
            {
                text.enabled = true;
                text.text = "Revive in " + Mathf.CeilToInt(Mathf.Max(health.DownSecondsLeft(), 0f));
            }
            else
            {
                text.enabled = false;
            }
        }

        private void HideText()
        {
            text.enabled = false;
        }
    }
}