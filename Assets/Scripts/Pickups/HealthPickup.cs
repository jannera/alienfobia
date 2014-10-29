using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class HealthPickUp : PickUp
    {
        override protected void PickedUp(GameObject player) {
            PlayerHealth health = player.GetComponentInChildren<PlayerHealth>();
            health.AddHealth(30);
        }
    }
} 