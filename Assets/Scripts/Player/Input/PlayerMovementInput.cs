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

            GameState.OnLevelOver += delegate()
            {
                this.enabled = false;
            };

            GameState.OnMyPlayerDown += delegate()
            {
                this.enabled = false;
            };

            GameState.OnMyPlayerRevived += delegate()
            {
                this.enabled = true;
            };
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

            
            Move(v, h);
            
            Turning();

            playerMovement.Animating(h != 0f || v != 0f);
        }

        void Move(float h, float v)
        {
            movement.Set(h, 0f, v);

            playerMovement.Move(movement);
        }

        void Turning()
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit floorHit;

            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {
                Vector3 playerToMouse = floorHit.point - transform.position;

                playerToMouse.y = 0f;

                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

                playerMovement.StartTurningTowards(newRotation);
            }
        }
    }
}