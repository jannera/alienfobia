using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class GrenadePickUp : CompleteProject.PhotonBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            GameObject go = other.gameObject;
            // If the entering collider is the player...
            if (go.CompareTag("Player") && PlayerManager.GetMyPlayer() == go)
            {
                PlayerShooting shooting = go.GetComponentInChildren<PlayerShooting>();
                shooting.grenades++;
                GetComponent<BoxCollider>().enabled = false; // disables further triggers while the master client is removing us
                RPC(RemovePickUp, PhotonTargets.MasterClient);
            }
        }

        [RPC]
        public void RemovePickUp()
        {
            PhotonNetwork.Destroy(gameObject);
        }

        void FixedUpdate()
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 30);
        }
    }

}