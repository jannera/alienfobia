using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CompleteProject
{
    // syncs rotation from owner to non-owners.
    // non-owners rotate the gameobject smoothly
    class RotationSync : CompleteProject.PhotonBehaviour
    {
        private Quaternion syncRotation;
        public float smoothTurning = 30;

        protected GameObject syncedGO;

        void Awake()
        {
            syncedGO = gameObject;
        }

        void Update()
        {
            if (photonView.isMine)
            {
                return; // no estimates on your own player
            }

            syncedGO.transform.rotation = Quaternion.Slerp(syncedGO.transform.rotation, syncRotation, Time.deltaTime * smoothTurning);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            if (stream.isWriting)
            {
                // writing is only called for owners of the object
                stream.SendNext(syncedGO.transform.rotation);
            }
            else
            {
                // reading is only called for non-owners of the object
                syncRotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
