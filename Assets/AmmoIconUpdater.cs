using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CompleteProject
{
    public class AmmoIconUpdater : MonoBehaviour
    {
        private WeaponInventory weaponSelector;
        private Weapon activeWeapon = null;
        private Image ammoIcon;

        // Use this for initialization
        void Start()
        {
            ammoIcon = GetComponent<Image>();
        }

        // Update is called once per frame
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
                OnWeaponChanged(weaponSelector.GetCurrentWeapon());

                weaponSelector.OnWeaponChanged += OnWeaponChanged;
            }
        }

        void OnWeaponChanged(Weapon w)
        {
            ammoIcon.sprite = w.ammoSprite;
        }
    }
}