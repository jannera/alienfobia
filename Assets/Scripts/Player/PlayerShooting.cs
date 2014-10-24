using UnityEngine;

namespace CompleteProject
{
    public class PlayerShooting : CompleteProject.PhotonBehaviour
    {
        public int damagePerShot = 20;                  // The damage inflicted by each bullet.
        public float timeBetweenBullets = 0.15f;        // The time between each shot.
        public float timeBetweenRockets = 2f;
        public float timeToReload = 1f;
        public float range = 100f;                      // The distance the gun can fire.


        float timer;                                    // A timer to determine when to fire.
        float rocketTimer = 0;
        float reloadTimer = 0;
        Ray shootRay;                                   // A ray from the gun end forwards.
        RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
        int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
        ParticleSystem gunParticles;                    // Reference to the particle system.
        LineRenderer gunLine;                           // Reference to the line renderer.
        AudioSource gunAudio;                           // Reference to the audio source.
        AudioSource gunReload;
        Light gunLight;                                 // Reference to the light component.
        float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.
        public const int clipSize = 60;
        public int bullets = clipSize;

        public GameObject grenadePreFab;

        public int grenades = 3;

        private bool isMine;
        private bool effectsDisplayedOnce = false;


        void Awake()
        {
            // Create a layer mask for the Shootable layer.
            shootableMask = LayerMask.GetMask("Shootable");

            // Set up the references.
            gunParticles = GetComponent<ParticleSystem>();
            gunLine = GetComponent<LineRenderer>();
            gunAudio = GetComponents<AudioSource>()[0];
            gunReload = GetComponents<AudioSource>()[1];
            gunLight = GetComponent<Light>();

            isMine = gameObject.GetComponentInParent<PositionSync>().isMine;
        }


        void Update()
        {
            // EXECUTED FOR EVERYONE
            // Add the time since Update was last called to the timer.
            timer += Time.deltaTime;

            // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
            if (timer >= timeBetweenBullets * effectsDisplayTime)
            {
                // ... disable the effects.
                if (effectsDisplayedOnce)
                {
                    DisableEffects();
                }
                else
                {
                    effectsDisplayedOnce = true;
                }
            }

            if (!isMine)
            {
                return;
            }

            // EXECUTED FOR ONLY THE OWNING PLAYER
            
            rocketTimer += Time.deltaTime;
            reloadTimer += Time.deltaTime;

            if (reloadTimer >= timeToReload && bullets == 0)
            {
                bullets = clipSize;
            }

            // If the Fire1 button is being press and it's time to fire...
            if (Input.GetButton("Fire1") && timer >= timeBetweenBullets)
            {
                // ... shoot the gun.
                Shoot();
            }

            if (Input.GetButton("Fire2") && rocketTimer >= timeBetweenRockets && grenades > 0)
            {
                Debug.Log("throwing gren");
                ThrowGrenade();
            }

            if (Input.GetKey(KeyCode.R))
            {
                StartReloading();
            }
        }


        public void DisableEffects()
        {
            // Disable the line renderer and the light.
            gunLine.enabled = false;
            gunLight.enabled = false;
        }

        void StartReloading()
        {
            bullets = 0;
            reloadTimer = 0f;
            gunReload.Play();
        }

        void Shoot()
        {
            if (bullets == 0)
            {
                return;
            }
            --bullets;
            if (bullets == 0)
            {
                StartReloading();
                return;
            }
            // Reset the timer.
            timer = 0f;

            // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;

            Vector3 firingEndPos;
            // Perform the raycast against gameobjects on the shootable layer and if it hits something...
            if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

                // If the EnemyHealth component exist...
                if (enemyHealth != null)
                {
                    // ... the enemy should take damage.
                    enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                }

                // Set the second position of the line renderer to the point the raycast hit.
                firingEndPos = shootHit.point;
                
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                firingEndPos = shootRay.origin + shootRay.direction * range;
            }

            // TODO: actually, because ray positions must be calculated locally on each client, we just need to
            // pass the enemy and the local hit point on the enemy to each client
            RPC<Vector3>(EnableFiringEffects, PhotonTargets.All, firingEndPos);
        }

        [RPC]
        private void EnableFiringEffects(Vector3 firingEndPos)
        {
            timer = 0f;

            // Play the gun shot audioclip.
            gunAudio.Play();

            // Enable the light.
            gunLight.enabled = true;

            // Stop the particles from playing if they were, then start the particles.
            gunParticles.Stop();
            gunParticles.Play();

            // Enable the line renderer and set it's first position to be the end of the gun.
            gunLine.enabled = true;
            gunLine.SetPosition(0, transform.position);
            gunLine.SetPosition(1, firingEndPos);
            effectsDisplayedOnce = false;
        }

        void ThrowGrenade()
        {
            rocketTimer = 0f;
            grenades--;

            RPC<Vector3, float>(CreateGrenade, PhotonTargets.MasterClient,
                transform.position, transform.rotation.eulerAngles.y);
        }


        [RPC]
        void CreateGrenade(Vector3 pos, float angle)
        {
            if (PhotonNetwork.isMasterClient)
            {
                object[] p = { angle };
                PhotonNetwork.InstantiateSceneObject(grenadePreFab.name, pos, Quaternion.identity, 0, p);
            }
        }

        // returns a number between 0 and 1 that tells how ready reloading is
        public float ReloadStatus()
        {
            return Mathf.Clamp(reloadTimer / timeToReload, 0, 1);
        }
    }
}