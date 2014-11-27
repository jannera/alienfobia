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
        }

        void Update()
        {
            if (weaponSelector == null)
            {
                // todo: instead of this polling, create an event for creating player? or your own player?
                // register once your player is created
                weaponSelector = PlayerManager.GetComponentFromMyPlayer<WeaponInventory>();
                if (weaponSelector == null)
                {
                    return;
                }
                activeWeapon = weaponSelector.GetCurrentWeapon();

                weaponSelector.OnWeaponChanged += OnWeaponChanged;
            }

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