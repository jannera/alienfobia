using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CompleteProject
{
    public class UIBulletCounter : MonoBehaviour
    {
        private Slider slider;
        private WeaponInventory weaponSelector;
        private Weapon activeWeapon = null;
        
        void Start()
        {
            slider = GetComponent<Slider>();

            GameState.OnMyPlayerJoined += delegate()
            {
                weaponSelector = PlayerManager.GetComponentFromMyPlayer<WeaponInventory>();
                activeWeapon = weaponSelector.GetCurrentWeapon();
                weaponSelector.OnWeaponChanged += OnWeaponChanged;
            };
        }

        void Update()
        {    
            if (activeWeapon == null)
            {
                return;
            }

            slider.maxValue = activeWeapon.clipSize;
            int bullets = activeWeapon.currentAmmo;
            if (activeWeapon.IsReloading())
            {
                slider.value = slider.maxValue * activeWeapon.ReloadReadiness();
            }
            else
            {
                slider.value = bullets;
            }

        }

        void OnWeaponChanged(Weapon w)
        {
            activeWeapon = w;
        }
    }

}