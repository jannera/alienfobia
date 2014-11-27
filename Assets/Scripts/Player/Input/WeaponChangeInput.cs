using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CompleteProject
{
    class WeaponChangeInput : PhotonBehaviour
    {
        private WeaponInventory inventory;
        void Awake()
        {
            if (!photonView.isMine)
            {
                Destroy(this);
                return;
            }
            inventory = GetComponent<WeaponInventory>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                inventory.ActivateNextWeapon();
            }
        }
    }
}
