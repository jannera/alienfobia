using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CompleteProject
{
    class MouseGrenadeInput : CompleteProject.PhotonBehaviour
    {
        private GrenadeThrowing grenadeThrowing;
        void Start()
        {
            if (!photonView.isMine ||
                PlayerManager.IsNPCClient())
            {
                Destroy(this);
            }
            else
            {
                grenadeThrowing = GetComponent<GrenadeThrowing>();
                GameState.OnLevelOver += delegate()
                {
                    this.enabled = false;
                };
            }
        }

        void Update()
        {
            if (Input.GetButton("Fire2") && grenadeThrowing.CanThrowGrenade())
            {
                grenadeThrowing.ThrowGrenade();
            }
        }
    }
}
