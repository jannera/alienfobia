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
    class PlayerRotationSync : CompleteProject.PhotonBehaviour
    {
        private Quaternion syncRotation;
        public float smoothTurning = 30;

        private GameObject player;

        void Awake()
        {
            player = PlayerManager.GetPlayerWithOwnerId(photonView.owner.ID);
        }

        void Update()
        {
            if (photonView.isMine)
            {
                return; // no estimates on your own player
            }

            // Debug.Log("estimating " + ownerId);
            // everyone estimates on other players, even the master client
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, syncRotation, Time.deltaTime * smoothTurning);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            if (stream.isWriting)
            {
                // writing is only called for owners of the object
                stream.SendNext(player.transform.rotation);
            }
            else
            {
                // reading is only called for non-owners of the object
                syncRotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
