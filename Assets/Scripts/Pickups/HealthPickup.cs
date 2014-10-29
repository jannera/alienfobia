using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class HealthPickup : CompleteProject.PhotonBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            GameObject go = other.gameObject;
            // If the entering collider is the player...
            if (go.CompareTag("Player") && PlayerManager.GetMyPlayer() == go)
            {
                PlayerHealth health = go.GetComponentInChildren<PlayerHealth>();
                health.AddHealth(30);
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