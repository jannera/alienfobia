using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CompleteProject
{
    public class WeaponSelector : MonoBehaviour
    {
        public delegate void WeaponChangedAction(Weapon newWeapon);
        public event WeaponChangedAction OnWeaponChanged;

        private int currentWeapon;
        public List<Weapon> weapons { get; private set; }

        void Awake()
        {
            weapons = GetWeaponsList();
            Activate(0);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                Activate(0);
            }

            if (Input.GetKey(KeyCode.Alpha2))
            {
                Activate(1);
            }

            if (Input.GetKey(KeyCode.Alpha3))
            {
                Activate(2);
            }
        }

        public void Activate(int index)
        {
            int i = 0;
            Weapon w = null;
            foreach (Transform child in transform)
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

        public void ActivateBasicWeapon()
        {
            Activate(0);
        }

        public Weapon GetCurrentWeapon()
        {
            int i = 0;
            foreach (Transform child in transform)
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

            foreach (Transform child in transform)
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