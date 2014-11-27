using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CompleteProject
{
    public class AmmoIconUpdater : MonoBehaviour
    {
        private WeaponInventory weaponSelector;
        private Image ammoIcon;

        void Start()
        {
            ammoIcon = GetComponent<Image>();

            GameState.OnMyPlayerJoined += delegate()
            {
                weaponSelector = PlayerManager.GetComponentFromMyPlayer<WeaponInventory>();
                OnWeaponChanged(weaponSelector.GetCurrentWeapon());

                weaponSelector.OnWeaponChanged += OnWeaponChanged;
            };
        }

        void OnWeaponChanged(Weapon w)
        {
            ammoIcon.sprite = w.ammoSprite;
        }
    }
}