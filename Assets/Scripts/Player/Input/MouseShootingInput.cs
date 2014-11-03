using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class MouseShootingInput : CompleteProject.PhotonBehaviour
    {
        Ray shootRay;                                   // A ray from the gun end forwards.
        RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
        
        public PlayerShooting playerShooting;
        // Use this for initialization
        void Start()
        {
            if (!photonView.isMine ||
                PlayerManager.IsNPCClient())
            {
                Destroy(this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // If the Fire1 button is being press and it's time to fire...
            if (Input.GetButton("Fire1") && playerShooting.CanFire())
            {
                // todo check the bullet things here
                if (playerShooting.bullets == 0)
                {
                    if (!playerShooting.isReloading)
                    {
                        playerShooting.StartReloading();
                    }
                }
                else
                {
                    Shoot();
                }
            }

            if (Input.GetKey(KeyCode.R) && !playerShooting.isReloading)
            {
                playerShooting.StartReloading();
            }
        }

        void Shoot()
        {
            // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;

            // Perform the raycast against gameobjects on the shootable layer and if it hits something...
            if (Physics.Raycast(shootRay, out shootHit, playerShooting.range, playerShooting.shootableMask))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                

                // Set the second position of the line renderer to the point the raycast hit.
                playerShooting.Shoot(shootHit.collider.gameObject, shootHit.point);
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range
                playerShooting.Shoot(null, shootRay.origin + shootRay.direction * playerShooting.range);
            }
        }
    }
}