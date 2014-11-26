using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    // todo rename this to rifle shooting input or something
    public class MouseShootingInput : CompleteProject.PhotonBehaviour
    {
        Ray shootRay;                                   // A ray from the gun end forwards.
        RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
        private GameObject playerGO;
        
        private PlayerShooting playerShooting;
        private GameObject barrelEnd;

        // Use this for initialization
        void Start()
        {
            if (!photonView.isMine ||
                PlayerManager.IsNPCClient())
            {
                Destroy(this);
                return;
            }
            playerGO = PlayerManager.GetMyPlayer();
            playerShooting = GetComponent<PlayerShooting>();
            barrelEnd = playerShooting.GetBarrelEnd();

            GameState.OnLevelOver += delegate()
            {
                this.enabled = false;
            };
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButton("Fire1") && playerShooting.CanFire())
            {
                if (playerShooting.currentAmmo == 0)
                {
                    if (!playerShooting.IsReloading())
                    {
                        playerShooting.StartReloading();
                    }
                }
                else
                {
                    transform.localRotation.SetLookRotation(playerGO.transform.forward, Vector3.up);
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
            //shootRay.origin = barrelEnd.transform.position;
            //shootRay.direction = playerGO.transform.forward;
            Vector3 forward = playerGO.transform.forward;

            Vector3 barrelPos = barrelEnd.transform.position;
            Vector3 upLimit = barrelPos + Vector3.up * 2f;
            Vector3 downLimit = barrelPos + Vector3.down * 2f;
            if (Physics.CapsuleCast(upLimit, downLimit, 0.2f, forward, 
                out shootHit, playerShooting.range))
            {
                playerShooting.Shoot(shootHit.collider.gameObject, shootHit.point);
            }
            else
            {
                playerShooting.Shoot(null, barrelPos + forward * playerShooting.range);
            }
        }
    }
}