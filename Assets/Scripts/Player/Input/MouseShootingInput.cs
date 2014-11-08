using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class MouseShootingInput : CompleteProject.PhotonBehaviour
    {
        Ray shootRay;                                   // A ray from the gun end forwards.
        RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
        public GameObject playerGO;
        
        public PlayerShooting playerShooting;

        // Use this for initialization
        void Start()
        {
            if (!photonView.isMine ||
                PlayerManager.IsNPCClient())
            {
                Destroy(this);
                return;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // If the Fire1 button is being press and it's time to fire...
            if (Input.GetButton("Fire1") && playerShooting.CanFire())
            {
                // todo check the bullet things here
                if (playerShooting.currentAmmo == 0)
                {
                    if (!playerShooting.IsReloading())
                    {
                        playerShooting.StartReloading();
                    }
                }
                else
                {
                    Shoot();
                }
            }

            if (Input.GetKey(KeyCode.R) && !playerShooting.IsReloading())
            {
                playerShooting.StartReloading();
            }
        }

        void Shoot()
        {
            // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
            shootRay.origin = transform.position;
            shootRay.direction = playerGO.transform.forward;

            if (Physics.Raycast(shootRay, out shootHit, playerShooting.range))
            {
                playerShooting.Shoot(shootHit.collider.gameObject, shootHit.point);
            }
            else
            {
                playerShooting.Shoot(null, shootRay.origin + shootRay.direction * playerShooting.range);
            }
        }
    }
}