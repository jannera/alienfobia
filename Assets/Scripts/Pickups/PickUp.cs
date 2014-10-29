using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public abstract class PickUp : CompleteProject.PhotonBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            GameObject go = other.gameObject;
            // If the entering collider is the player...
            if (go.CompareTag("Player") && PlayerManager.GetMyPlayer() == go)
            {
                GetComponent<BoxCollider>().enabled = false; // disables further triggers while the master client is removing us
                PickedUp(go);
                RPC(RemovePickUp, PhotonTargets.MasterClient);
            }
        }

        protected abstract void PickedUp(GameObject player);

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