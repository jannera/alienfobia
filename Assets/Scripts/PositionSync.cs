using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    /**
     * Syncs physics objects between master client that owns
     * them and clients that do not.
     * 
     * On client end, estimates positions of objects based
     * on their earlier position, velocity and time since update.
     **/
    public class PositionSync : CompleteProject.PhotonBehaviour
    {
        public float movementEpsilon = 1; // velocity magnitudes above this are considering "moving" (for animations etc)

        void Awake()
        {
            syncTimestamp = double.NaN;
            maxVelSqr = maximumVelocity * maximumVelocity;
        }

        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                // sending is only called for the owner, i.e. server
                stream.SendNext(rigidbody.position);
                stream.SendNext(rigidbody.velocity);
            }
            else
            {
                // reading is only called for non-owners, i.e. clients
                syncPosition = (Vector3)stream.ReceiveNext();
                syncVelocity = (Vector3)stream.ReceiveNext();

                rigidbody.velocity = syncVelocity;

                if (double.IsNaN(syncTimestamp))
                {
                    // this is the first time we're updating position, so just directly set it
                    transform.position = syncPosition;
                    rigidbody.position = syncPosition;
                }
                syncTimestamp = info.timestamp;
            }

        }

        // SERVER specific part
        // TODO maximum velocity limiting could be moved to it's own component
        public float maximumVelocity = 4;
        private float maxVelSqr;

        void FixedUpdate()
        {
            if (photonView.isMine)
            {
                // limit the maximum velocities
                if (rigidbody.velocity.sqrMagnitude > maxVelSqr)
                {
                    rigidbody.velocity = rigidbody.velocity.normalized * maximumVelocity;
                }
            }
        }

        // CLIENT specific part
        private double syncTimestamp;
        private Vector3 syncPosition; // latest position master sent client
        private Vector3 syncVelocity; // latest velocity master sent client
        public float maxVelocityStepInSecond = 5;

        void Update()
        {
            if (!photonView.isMine)
            {
                // only clients estimate positions
                SyncedMovement();
            }

        }

        private void SyncedMovement()
        {
            if (double.IsNaN(syncTimestamp))
            {
                return; // still haven't received data from the server
            }
            float pingInSeconds = (float)PhotonNetwork.GetPing() * 0.001f;
            float timeSinceLastUpdate = (float)(PhotonNetwork.time - syncTimestamp);
            float totalTimePassed = pingInSeconds + timeSinceLastUpdate;

            Vector3 extrapolatedTargetPosition = syncPosition + syncVelocity * totalTimePassed;

            Vector3 newPosition = Vector3.MoveTowards(transform.position, extrapolatedTargetPosition,
                maxVelocityStepInSecond * Time.deltaTime);

            
            if ((newPosition - extrapolatedTargetPosition).magnitude > 2f)
            {
                // if position has fallen too far out of sync, just teleport
                transform.position = extrapolatedTargetPosition;
                Debug.Log("teleported " + gameObject + " " + photonView.instantiationId + " owned by " + photonView.ownerId);
            }
            else
            {
                transform.position = newPosition;
            }
            rigidbody.position = transform.position;
        }

        public bool IsMoving()
        {
            Vector3 vel;
            if (photonView.isMine)
            {
                vel = rigidbody.velocity;
            }
            else
            {
                vel = syncVelocity;
            }
            return vel.magnitude > movementEpsilon;
        }
    }
}