using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CompleteProject
{
    public class UIBulletCounter : MonoBehaviour
    {

        private Slider slider;
        // Use this for initialization
        void Start()
        {
            slider = GetComponent<Slider>();
        }

        // Update is called once per frame
        void Update()
        {
            GameObject playerGun = GameObject.FindGameObjectWithTag("PlayerGun");
            if (playerGun == null)
            {
                slider.value = 0;
                return;
            }

            PlayerShooting playerShooting = playerGun.GetComponent<PlayerShooting>();
            slider.maxValue = PlayerShooting.clipSize;
            int bullets = playerShooting.bullets;
            if (bullets == 0)
            {
                // todo slide based on 
                slider.value = slider.maxValue * playerShooting.ReloadStatus();
            }
            else
            {
                slider.value = bullets;
            }

        }
    }

}