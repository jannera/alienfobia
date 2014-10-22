using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CompleteProject
{
    public class GrenadeCounter : MonoBehaviour
    {
        private Text text;
        private PlayerShooting playerShooting;
        // Use this for initialization
        void Start()
        {
            text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerShooting == null)
            {
                playerShooting = PlayerManager.GetComponentFromMyPlayer<PlayerShooting>();
                if (playerShooting == null)
                {
                    text.text = "";
                    return;
                }
            }

            text.text = "X " + playerShooting.grenades;
        }
    }

}
