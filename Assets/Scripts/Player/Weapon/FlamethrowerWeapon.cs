using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CompleteProject
{
    public class FlamethrowerWeapon : Weapon
    {
        private ParticleSystem particles;
        private AudioSource weaponAudio;
        private const float fadeOutTime = 0.5f;
        private float fader;

        public bool isFiring;

        private float secondPerAmmo = 1f; // how many seconds of constant firing does one ammo give?
        private float fuelTimer = 0f;

        // Use this for initialization
        void Awake()
        {
            particles = GetComponent<ParticleSystem>();
            weaponAudio = GetComponent<AudioSource>();

            particles.Stop();
            weaponAudio.Stop();
            isFiring = false;
        }

        public void Update()
        {
            if (isFiring)
            {
                particles.Play();
                audio.volume = 1f;
                fader = fadeOutTime;

                if (!audio.isPlaying)
                {
                    audio.Play();
                    audio.time = 0.5f;
                }

                fuelTimer += Time.deltaTime;
                while (fuelTimer > secondPerAmmo)
                {
                    currentAmmo--;
                    fuelTimer -= secondPerAmmo;
                }
            }
            else
            {
                fader -= Time.deltaTime;
                if (fader < 0)
                {
                    fader = 0;
                    audio.Stop();
                }
                particles.Stop();
                audio.volume = fader / fadeOutTime;
            }
        }

        public override bool IsReloading()
        {
            return false;
        }

        public override float ReloadReadiness()
        {
            return 1f;
        }

        public override bool CanFire()
        {
            return currentAmmo > 0;
        }

        public override bool HasInfiniteClips()
        {
            return false;
        }
    }
}
