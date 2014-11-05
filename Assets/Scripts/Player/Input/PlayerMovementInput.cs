using System;
using UnityEngine;

namespace CompleteProject
{
    public class PlayerMovementInput : CompleteProject.PhotonBehaviour
    {
        Vector3 movement;
        int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
        float camRayLength = 100f;

        PlayerMovement playerMovement;

        void Awake()
        {
            if (!photonView.isMine || PlayerManager.IsNPCClient())
            {
                Destroy(this);
                return;
            }

            playerMovement = GetComponent<PlayerMovement>();
            floorMask = LayerMask.GetMask("Floor");
        }

        void FixedUpdate()
        {
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

            playerMovement.Animating(h != 0f || v != 0f);
        }

        void Move(float h, float v)
        {
            // Set the movement vector based on the axis input.
            movement.Set(h, 0f, v);

            playerMovement.Move(movement);
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

                playerMovement.StartTurningTowards(newRotation);
            }
        }
    }
}