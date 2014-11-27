using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CompleteProject
{
    public class WeaponInventory : PhotonBehaviour
    {
        public delegate void WeaponChangedAction(Weapon newWeapon);
        public event WeaponChangedAction OnWeaponChanged;

        private int currentWeapon;
        public List<Weapon> weapons { get; private set; }

        public GameObject hand;

        void Awake()
        {
            weapons = GetWeaponsList();
            Activate(0);
        }

        void Update()
        {
        }

        public void Activate(int index)
        {
            RPC<int>(ActivateProxy, PhotonTargets.All, index);
        }

        [RPC]
        private void ActivateProxy(int index)
        {
            int i = 0;
            Weapon w = null;
            foreach (Transform child in hand.transform)
            {
                if (child.CompareTag("InventoryWeapon"))
                {
                    child.gameObject.SetActive(i == index);
                    if (i == index)
                    {
                        w = child.GetComponentInChildren<Weapon>();
                    }
                    i++;
                }
            }
            currentWeapon = index;
            if (w != null && OnWeaponChanged != null)
            {
                OnWeaponChanged(w);
            }
        }

        public void ActivateNextWeapon()
        {
            int nextWeapon = currentWeapon + 1;
            if (nextWeapon >= weapons.Count)
            {
                nextWeapon = 0;
            }
            Activate(nextWeapon);
        }

        public void ActivateBasicWeapon()
        {
            Activate(0);
        }

        public Weapon GetCurrentWeapon()
        {
            int i = 0;
            foreach (Transform child in hand.transform)
            {
                if (child.CompareTag("InventoryWeapon"))
                {
                    if (i == currentWeapon)
                    {
                        return child.GetComponentInChildren<Weapon>();
                    }
                    i++;
                }
            }
            return null;
        }

        private List<Weapon> GetWeaponsList()
        {
            List<Weapon> results = new List<Weapon>();

            foreach (Transform child in hand.transform)
            {
                if (child.CompareTag("InventoryWeapon"))
                {
                    Weapon w = child.GetComponentInChildren<Weapon>();
                    if (w != null)
                    {
                        results.Add(w);
                    }
                }
            }
            return results;
        }
    }
}