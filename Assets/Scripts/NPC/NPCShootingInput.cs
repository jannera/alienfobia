using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class NPCShootingInput : MonoBehaviour
    {
        public PlayerShooting playerShooting;
        public float shootingRadius = 10f;
        public float smoothTurning = 10f;

        Ray shootRay;
        RaycastHit shootHit;
        GameObject myPlayer;

        private Quaternion targetRotation;
        private PlayerHealth ownHealth;

        // Use this for initialization
        void Start()
        {
            if (!PlayerManager.IsNPCPlayer(gameObject))
            {
                Destroy(this);
            }
            else
            {
                myPlayer = PlayerManager.GetMyPlayer();
                targetRotation = myPlayer.transform.rotation;
                ownHealth = GetComponentInParent<PlayerHealth>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (ownHealth.isDead)
            {
                return; // stop moving when dead
            }

            // rotate towards the target
            myPlayer.transform.rotation = Quaternion.Slerp(myPlayer.transform.rotation, targetRotation, Time.deltaTime * smoothTurning);

            if (!playerShooting.CanFire())
            {
                return;
            }

            if (playerShooting.bullets == 0)
            {
                if (!playerShooting.isReloading)
                {
                    playerShooting.StartReloading();
                }
                return;
            }

            GameObject enemy = GetClosestEnemy();
            if (enemy == null)
            {
                return;
            }

            // start turning the player towards the enemy
            targetRotation = Quaternion.LookRotation(enemy.transform.position - myPlayer.transform.position);

            playerShooting.Shoot(enemy, enemy.transform.position);
        }

        private GameObject GetClosestEnemy()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, shootingRadius);
            GameObject closest = null;
            float closestDst = 0;

            foreach (Collider c in hitColliders)
            {
                if (!c.CompareTag("Enemy"))
                {
                    continue;
                }

                if (c.GetComponent<EnemyHealth>().isDead) {
                    continue;
                }
                    
                if (!DirectLineExists(c.gameObject))
                {
                    continue;
                }
                float dst = (c.gameObject.transform.position - transform.position).magnitude;
                if (closest == null)
                {
                    closest = c.gameObject;
                    closestDst = dst;
                }
                else if (dst < closestDst)
                {
                    closest = c.gameObject;
                    closestDst = dst;
                }
            }
            return closest;
        }

        private bool DirectLineExists(GameObject other)
        {
            Vector3 diff = other.transform.position - transform.position;
            shootRay.origin = transform.position;
            shootRay.direction = diff;
            if (Physics.Raycast(shootRay, out shootHit, diff.magnitude))
            {
                // Debug.Log(shootHit.collider.gameObject + " is blocking line of fire! " + shootRay.origin + " -> " + shootRay.direction + " to " + other.GetComponent<PhotonView> ().instantiationId);
                return false;
            }
            return true;
        }
    }
}