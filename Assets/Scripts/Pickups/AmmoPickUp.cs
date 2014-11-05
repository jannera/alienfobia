using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CompleteProject
{
    public class AmmoPickUp : PickUp
    {
        public int ammoAmount = 10;

        override protected void PickedUp(GameObject player)
        {
            WeaponInventory selector = player.GetComponent<WeaponInventory>();
            if (selector == null)
            {
                Debug.LogError("Couldn't find weapon selector from " + player);
                return;
            }
            foreach (Weapon w in selector.weapons)
            {
                if (!w.HasInfiniteClips())
                {
                    w.ReceiveAmmo(ammoAmount);
                }
            }
        }
    }
}
