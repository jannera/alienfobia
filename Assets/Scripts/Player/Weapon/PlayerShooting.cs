﻿using UnityEngine;

namespace CompleteProject
{
    public class PlayerShooting : Weapon
    {
        public int damagePerShot = 20;                  // The damage inflicted by each bullet.
        public float timeBetweenBullets = 0.15f;        // The time between each shot.
        public float timeToReload = 1f;
        public float range = 100f;                      // The distance the gun can fire.

        float timer;                                    // A timer to determine when to fire.
        float reloadTimer = 0;
        ParticleSystem gunParticles;                    // Reference to the particle system.
        LineRenderer gunLine;                           // Reference to the line renderer.
        AudioSource gunAudio;                           // Reference to the audio source.
        AudioSource gunReload;
        Light gunLight;                                 // Reference to the light component.
        float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.
        private bool isReloading;

        private bool effectsDisplayedOnce = false;

        private GameObject barrelEnd;

        void Awake()
        {
            currentAmmo = clipSize;

            barrelEnd = GetBarrelEnd();

            // Set up the references.
            gunParticles = barrelEnd.GetComponent<ParticleSystem>();
            gunLine = GetComponent<LineRenderer>();
            gunAudio = GetComponents<AudioSource>()[0];
            gunReload = GetComponents<AudioSource>()[1];
            gunLight = barrelEnd.GetComponent<Light>();
        }

        public GameObject GetBarrelEnd()
        {
            foreach (Transform t in transform)
            {
                if (t.CompareTag("BarrelEnd"))
                {
                    Debug.Log("barrel= " + t.gameObject.name);
                    return t.gameObject;
                }
            }
            return null;
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

            if (!photonView.isMine)
            {
                return;
            }

            // EXECUTED FOR ONLY THE OWNING PLAYER

            reloadTimer += Time.deltaTime;

            if (reloadTimer >= timeToReload && isReloading)
            {
                currentAmmo = clipSize;
                isReloading = false;
            }
        }

        public void DisableEffects()
        {
            // Disable the line renderer and the light.
            gunLine.enabled = false;
            gunLight.enabled = false;
        }

        public void StartReloading()
        {
            currentAmmo = 0;
            reloadTimer = 0f;
            isReloading = true;
            gunReload.Play();
        }

        public void Shoot(GameObject enemy, Vector3 hit)
        {
            --currentAmmo;
            timer = 0f;

            if (enemy != null)
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damagePerShot, hit);
                }
            }

            RPC<Vector3>(EnableFiringEffects, PhotonTargets.All, hit);
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
            gunLine.SetPosition(0, barrelEnd.transform.position);
            gunLine.SetPosition(1, firingEndPos);
            effectsDisplayedOnce = false;
        }        

        // returns a number between 0 and 1 that tells how ready reloading is
        public override float ReloadReadiness()
        {
            return Mathf.Clamp(reloadTimer / timeToReload, 0, 1);
        }

        public override bool CanFire()
        {
            return timer >= timeBetweenBullets && !isReloading;
        }

        public bool IsFullClip()
        {
            return currentAmmo == clipSize;
        }

        public override bool IsReloading()
        {
            return isReloading;
        }

        public override bool HasInfiniteClips()
        {
            return true;
        }
    }
}