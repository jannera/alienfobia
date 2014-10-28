using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CompleteProject
{
    /*
     * One of these syncers are created for every Player.
     * Each client tells everyone else what their Players rotation is,
     * and everyone else listens. Everyone also makes sure that when 
     * someone elses Players rotation changes, the change happens smoothly.
     * */
    class PlayerRotationSync : CompleteProject.RotationSync
    {
        void Awake()
        {
            syncedGO = PlayerManager.GetPlayerWithOwnerId(photonView.owner.ID);
        }
    }
}
