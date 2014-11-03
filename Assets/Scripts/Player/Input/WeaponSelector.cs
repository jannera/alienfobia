using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class WeaponSelector : MonoBehaviour
    {

        void Awake()
        {
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

        public void Activate(string name)
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("InventoryWeapon"))
                {
                    child.gameObject.SetActive(child.gameObject.name == name);
                }
            }
        }

        public void Activate(int index)
        {
            int i = 0;
            foreach (Transform child in transform)
            {
                if (child.CompareTag("InventoryWeapon"))
                {
                    child.gameObject.SetActive(i == index);
                    i++;
                }
            }
        }
    }
}