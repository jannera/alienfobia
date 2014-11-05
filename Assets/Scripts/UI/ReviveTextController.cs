using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CompleteProject
{
    public class ReviveTextController : MonoBehaviour
    {
        private PlayerHealth health;
        private Text text;

        // Use this for initialization
        void Awake()
        {
            text = GetComponent<Text>();
            HideText();
        }

        // Update is called once per frame
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

            if (health.isDowned && !health.isDead)
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