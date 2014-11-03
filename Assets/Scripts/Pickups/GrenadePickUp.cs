using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class GrenadePickUp : PickUp
    {
        override protected void PickedUp(GameObject player)
        {
            GrenadeThrowing throwing = player.GetComponent<GrenadeThrowing>();
            throwing.grenades++;
        }
    }
}