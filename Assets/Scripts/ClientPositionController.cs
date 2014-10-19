﻿using UnityEngine;
using System.Collections;

public class ClientPositionController : MonoBehaviour
{
		private double syncTimestamp;
		private Vector3 syncPosition;
		private Vector3 syncVelocity;

        private float maxVelocityStepInSecond = 10;

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
				// todo teleport check

				transform.position = newPosition;
		}

		public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
				if (stream.isWriting) {
						return; // client only listens for changes
				}
				syncPosition = (Vector3)stream.ReceiveNext ();
				syncVelocity = (Vector3)stream.ReceiveNext ();
				transform.rotation = (Quaternion)stream.ReceiveNext ();

				if (double.IsNaN (syncTimestamp)) {
						// this is the first time we're updating position, so just directly set it
						transform.position = syncPosition;
				}
				syncTimestamp = info.timestamp;
		}

}
