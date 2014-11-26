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
        private Vector3 walkingDir = new Vector3();

        Quaternion targetRotation;

        void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            targetRotation = transform.rotation;

            GameState.OnTimeIsUp += delegate()
            {
                Animating(false);
                this.enabled = false;
            };
        }

        void Update()
        {
            if (photonView.isMine)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothTurning);
            }
        }

        public void Move(Vector3 movement) {
            walkingDir = movement.normalized;
            float yAngle = transform.rotation.eulerAngles.y;
            walkingDir = Quaternion.Euler(0, -yAngle, 0) * walkingDir;
            // Debug.Log(movement.normalized + " ( " + yAngle + ") -> " + walkingDir);
            anim.SetFloat("VelocityX", walkingDir.x);
            anim.SetFloat("VelocityZ", walkingDir.z);
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
            anim.SetBool("IsWalking", walking);
        }
    }
}