using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class FlamethrowerInput : MonoBehaviour {
        private FlamethrowerWeapon weapon;
        private WeaponSelector selector;

        void Awake()
        {
            weapon = GetComponent<FlamethrowerWeapon>();
            selector = PlayerManager.GetComponentFromMyPlayer<WeaponSelector>();
        }

	    // Update is called once per frame
	    void Update () {
            if (Input.GetButton("Fire1"))
            {
                if (weapon.CanFire())
                {
                    weapon.isFiring = true;
                }
                else
                {
                    // ran out of ammo, switch to basic weapon
                    weapon.isFiring = false;
                    selector.ActivateBasicWeapon();
                }
                
            }
            else
            {
                weapon.isFiring = false;
            }
	    }
    }
}