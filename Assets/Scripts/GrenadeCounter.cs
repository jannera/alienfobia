using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CompleteProject
{
    public class GrenadeCounter : MonoBehaviour
    {
        private Text text;
        // Use this for initialization
        void Start()
        {
            text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            GameObject playerGun = GameObject.FindGameObjectWithTag("PlayerGun");
            if (playerGun == null)
            {
                text.text = "";
                return;
            }
            PlayerShooting playerShooting = playerGun.GetComponent<PlayerShooting>();

            text.text = "X " + playerShooting.grenades;
        }
    }

}
