using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class PlayerMovement : CompleteProject.PhotonBehaviour
    {
        Animator anim;
        public float speed = 5f;
        public float smoothTurning = 10f;

        PositionSync positionController;
        public GameObject rotationSyncPreFab;
        Quaternion targetRotation;

        void Awake()
        {
            // Set up references.
            anim = GetComponent<Animator>();
            positionController = GetComponent<PositionSync>();
            targetRotation = transform.rotation;

            if (photonView.isMine)
            {
                // create rotation synchronizer
                PhotonNetwork.Instantiate(rotationSyncPreFab.name, Vector3.zero,
                    Quaternion.identity, 0);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Set the player's rotation to this new rotation.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothTurning);

            if (!photonView.isMine)
            {
                Animating(positionController.IsMoving());
            }
        }

        public void Move(Vector3 movement) {
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