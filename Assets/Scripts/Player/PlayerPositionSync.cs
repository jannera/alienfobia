using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CompleteProject
{
    class PlayerPositionSync : PositionSync
    {
        private PlayerMovement playerMovement;
        public new void Awake()
        {
            base.Awake();
            playerMovement = GetComponent<PlayerMovement>();
        }

        protected override Vector3 GetVelocity()
        {
            return playerMovement.lastVelocity;
        }
    }
}
