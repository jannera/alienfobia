using System;
using UnityEngine;

namespace CompleteProject
{
    public class PlayerMovement : CompleteProject.PhotonBehaviour
    {
        public float speed = 6f;            // The speed that the player will move at.
        Vector3 movement;                   // The vector to store the direction of the player's movement.
        Animator anim;                      // Reference to the animator component.
        int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
        float camRayLength = 100f;          // The length of the ray from the camera into the scene.
        public float smoothTurning = 10f;
        public float force = 3000;

        public GameObject rotationSyncPreFab;

        public PositionSync positionController;

        void Awake()
        {
            // Create a layer mask for the floor layer.
            floorMask = LayerMask.GetMask("Floor");

            // Set up references.
            anim = GetComponent<Animator>();

            int ownerId = (int)photonView.instantiationData[0];
            if (ownerId == PhotonNetwork.player.ID)
            {
                // create rotation synchronizer
                PhotonNetwork.Instantiate(rotationSyncPreFab.name, Vector3.zero,
                    Quaternion.identity, 0);
            }
        }

        void FixedUpdate()
        {
            Animating(positionController.IsMoving());

            if (!photonView.isMine)
            {
                return;
            }

            float h = 0, v = 0;
            if (Input.GetKey(KeyCode.W))
            {
                v += 1;
                h += 1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                v -= 1;
                h += 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                v -= 1;
                h -= 1;

            }
            if (Input.GetKey(KeyCode.D))
            {
                v += 1;
                h -= 1;
            }

            // Move the player around the scene.
            Move(v, h);

            // Turn the player to face the mouse cursor.
            Turning();

            if (h != 0f || v != 0f)
            {
                // start animating right at the key press (but don't stop straight after player stops pressing keys)
                Animating(true);
            }

        }

        void Move(float h, float v)
        {
            if (h == 0 && v == 0)
            {
                return;
            }
            // Set the movement vector based on the axis input.
            movement.Set(h, 0f, v);

            // Normalise the movement vector and make it proportional to the speed per second.
            movement = movement.normalized * speed * Time.deltaTime;

            // Move the player to it's current position plus the movement.
            ApplyForce(movement);
        }

        [RPC]
        public void ApplyForce(Vector3 movement)
        {
            rigidbody.AddForce(movement * force * Time.deltaTime);
        }

        void Turning()
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit floorHit;

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = floorHit.point - transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * smoothTurning);
            }
        }

        void Animating(bool walking)
        {
            anim.SetBool("IsWalking", walking);
        }
    }
}