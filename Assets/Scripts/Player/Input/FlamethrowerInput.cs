﻿using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class FlamethrowerInput : PhotonBehaviour {
        private FlamethrowerWeapon weapon;
        private WeaponInventory selector;

        void Awake()
        {
            if (!photonView.isMine)
            {
                Destroy(this);
                return;
            }

            GameState.OnMyPlayerDown += delegate()
            {
                this.enabled = false;
            };

            GameState.OnMyPlayerRevived += delegate()
            {
                this.enabled = true;
            };

            GameState.OnLevelOver += delegate()
            {
                this.enabled = false;
            };

            weapon = GetComponent<FlamethrowerWeapon>();
            selector = PlayerManager.GetComponentFromMyPlayer<WeaponInventory>();
        }

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