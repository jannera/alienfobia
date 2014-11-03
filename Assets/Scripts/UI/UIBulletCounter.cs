using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CompleteProject
{
    public class UIBulletCounter : MonoBehaviour
    {
        private Slider slider;
        private WeaponSelector weaponSelector;
        
        void Start()
        {
            slider = GetComponent<Slider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (weaponSelector == null)
            {
                weaponSelector = PlayerManager.GetComponentFromMyPlayer<WeaponSelector>();
                if (weaponSelector == null)
                {
                    return;
                }
            }

            Weapon weapon = weaponSelector.GetCurrentWeapon(); // TODO: instead of this constant polling, WeaponSelected should for example send an event when the weapon is changed
            slider.maxValue = weapon.clipSize;
            int bullets = weapon.currentAmmo;
            if (weapon.IsReloading())
            {
                // todo slide based on 
                slider.value = slider.maxValue * weapon.ReloadReadiness();
            }
            else
            {
                slider.value = bullets;
            }

        }
    }

}