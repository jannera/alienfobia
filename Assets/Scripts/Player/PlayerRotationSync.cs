using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CompleteProject
{
    // Handles both the server and client sides of rotation synchronizing.
    // Clients notify server of their updated rotations.
    // Server notify all clients of the rotations, but clients only care about
    // rotations of other players other than their own.
    class PlayerRotationSync : Photon.MonoBehaviour
    {
        private bool isMine;
        private Quaternion syncRotation;
        public float smoothTurning = 10f;

        void Awake()
        {
            isMine = PlayerManager.GetMyPlayer() == gameObject;
        }

        void Update()
        {
            if (isMine)
            {
                return; // no estimates on your own player
            }

            // everyone estimates on other players, even the master client
            transform.rotation = Quaternion.Slerp(transform.rotation, syncRotation, Time.deltaTime * smoothTurning);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            Debug.Log("writing " + stream.isWriting);
            if (stream.isWriting)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    if (isMine)
                    {
                        stream.SendNext(transform.rotation);
                    }
                    else
                    {
                        // send the latest data received
                        stream.SendNext(syncRotation);
                    }
                }
                else
                {
                    if (isMine)
                    {
                        // clients only send updates for their own player
                        stream.SendNext(transform.rotation);
                    }
                }
            }
            else
            {
                // reading part
                if (stream.Count == 1)
                {
                    syncRotation = (Quaternion)stream.ReceiveNext();
                }
                else
                {
                    Debug.Log("wasn't able to read");
                }
                
            }
        }
    }
}
