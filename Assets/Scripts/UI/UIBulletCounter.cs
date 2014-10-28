using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CompleteProject
{
    public class UIBulletCounter : MonoBehaviour
    {
        private Slider slider;
        private PlayerShooting playerShooting;
        
        void Start()
        {
            slider = GetComponent<Slider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerShooting == null)
            {
                playerShooting = PlayerManager.GetComponentFromMyPlayer<PlayerShooting>();
                if (playerShooting == null)
                {
                    return;
                }
            }
            
            slider.maxValue = PlayerShooting.clipSize;
            int bullets = playerShooting.bullets;
            if (playerShooting.isReloading)
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