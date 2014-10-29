using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class GrenadePickUp : PickUp
    {
        override protected void PickedUp(GameObject player)
        {
            PlayerShooting shooting = player.GetComponentInChildren<PlayerShooting>();
            shooting.grenades++;
        }
    }
}