using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class PlayerMovement : CompleteProject.PhotonBehaviour
    {
        Animator anim;
        public float speed = 5f;
        public float smoothTurning = 10f;
        public Vector3 lastVelocity { get; private set; }

        Quaternion targetRotation;

        void Awake()
        {
            // Set up references.
            anim = GetComponent<Animator>();
            targetRotation = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.isMine)
            {
                // Set the player's rotation to this new rotation.
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothTurning);
            }
        }

        public void Move(Vector3 movement) {
            lastVelocity = movement.normalized * speed * 0.25f;
            movement = movement.normalized * speed * Time.deltaTime;
            rigidbody.MovePosition(transform.position + movement);
        }

        public void StartTurningTowards(Quaternion target)
        {
            targetRotation = target;
        }

        public void Animating(bool walking)
        {
            // Debug.Log("animating " + photonView.instantiationId + " " + walking);
            anim.SetBool("IsWalking", walking);
        }
    }
}