using UnityEngine;
using System.Collections;

public class ClientPositionController : MonoBehaviour
{
		private double syncTimestamp;
		private Vector3 syncPosition;
		private Vector3 syncVelocity;

        public float maxVelocityStepInSecond = 5;

		// Use this for initialization
		void Start ()
		{
				syncTimestamp = double.NaN;
		}
	
		// Update is called once per frame
		void Update ()
		{
				SyncedMovement ();
		}

		private void SyncedMovement ()
		{
				if (double.IsNaN (syncTimestamp)) {
						return; // still haven't received data from the server
				}
				float pingInSeconds = (float)PhotonNetwork.GetPing () * 0.001f;
				float timeSinceLastUpdate = (float)(PhotonNetwork.time - syncTimestamp);
				float totalTimePassed = pingInSeconds + timeSinceLastUpdate;

				Vector3 extrapolatedTargetPosition = syncPosition + syncVelocity * totalTimePassed;

				Vector3 newPosition = Vector3.MoveTowards (transform.position, extrapolatedTargetPosition,
                    maxVelocityStepInSecond * Time.deltaTime);

				// Debug.Log(transform.position + " -> " + newPosition);
				if ((newPosition - extrapolatedTargetPosition).magnitude > 0.5f)
                {
                    // if position has fallen too far out of sync, just teleport
                    transform.position = extrapolatedTargetPosition;
                    Debug.Log("teleported " + gameObject);
                }
                else
                {
                    transform.position = newPosition;
                }
		}

		public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
				if (stream.isWriting) {
						return; // client only listens for changes
				}
				syncPosition = (Vector3)stream.ReceiveNext ();
				syncVelocity = (Vector3)stream.ReceiveNext ();

				if (double.IsNaN (syncTimestamp)) {
						// this is the first time we're updating position, so just directly set it
						transform.position = syncPosition;
				}
				syncTimestamp = info.timestamp;
		}

}
