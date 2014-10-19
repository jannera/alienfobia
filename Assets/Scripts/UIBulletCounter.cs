using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CompleteProject
{
    public class UIBulletCounter : MonoBehaviour
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
            int bullets = playerShooting.bullets;
            if (bullets == 0) {
                text.text = "RELOADING!";
            }
            else {
                text.text = "Bullets: " + bullets;
            }
            
        }
    }

}