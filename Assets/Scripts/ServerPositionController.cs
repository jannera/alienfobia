using UnityEngine;
using System.Collections;

public class ServerPositionController : MonoBehaviour
{
		public float maximumVelocity = 4;
		private float maxVelSqr;
		public PhotonView photonView;

		public float speed = 3000;

		// Use this for initialization
		void Start ()
		{
				maxVelSqr = maximumVelocity * maximumVelocity;
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void FixedUpdate ()
		{
				// limit the maximum velocities
				if (rigidbody.velocity.sqrMagnitude > maxVelSqr) {
						rigidbody.velocity = rigidbody.velocity.normalized * maximumVelocity;
				}
		}

		public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
				if (!stream.isWriting) {
						return; // server only sends
				}
				stream.SendNext (rigidbody.position);
				stream.SendNext (rigidbody.velocity);
		}

		[RPC]
		void Move (Vector3 movement)
		{
            rigidbody.AddForce(movement * speed * Time.deltaTime);
		}
}
