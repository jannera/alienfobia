using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CompleteProject
{
    public class PlayerHealth : CompleteProject.PhotonBehaviour
    {
        public int startingHealth = 100;
        public int currentHealth { get; private set; }

        Animator anim;
        PlayerMovementInput playerMovement;
        PlayerShooting playerShooting;
        public bool isDead { get; private set; }
        public bool isDowned { get; private set; } 
        public float automaticReviveTime = 10; // in seconds
        private float downTimer;
        private float reviveHealthPercentage = 0.25f;

        public GameObject bloodParticles;

        private AudioSource[] hurtSounds;
        private AudioSource[] deathSounds;

        private AudioLibraryController allAudio;

        void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            playerMovement = GetComponent<PlayerMovementInput>();
            playerShooting = GetComponentInChildren<PlayerShooting>();

            currentHealth = startingHealth;

            allAudio = GetComponent<AudioLibraryController>();
            hurtSounds = allAudio.GetByTag("AudioHurt");
            deathSounds = allAudio.GetByTag("AudioDeath");
        }


        void Update()
        {
            if (isDowned)
            {
                bool othersAlive = PlayerManager.AreAnyPlayersAlive();
                if (!othersAlive) {
                    if (!isDead)
                    {
                        RPC(Death, PhotonTargets.All);
                    }
                }
                downTimer += Time.deltaTime;
                
                if (downTimer >= automaticReviveTime && othersAlive)
                {
                    RPC(Up, PhotonTargets.All);
                }
                
            }
        }


        public void TakeDamage(int amount, Vector3 attackerPosition)
        {
            currentHealth -= amount;

            RPC(ShowDamageEffects, PhotonTargets.All);

            if (currentHealth <= 0 && !isDowned)
            {
                RPC(Down, PhotonTargets.All);
            }
        }

        [RPC]
        public void ShowDamageEffects()
        {
            AudioSource src = Utility.PickRandomly(hurtSounds);
            if (!src.isPlaying)
            {
                src.Play();
            }

            // bloodParticles.transform.rotation.SetFromToRotation(transform.position, attackerPosition);
            
            GameObject blood = (GameObject) Instantiate(bloodParticles, transform.position, Quaternion.identity);
            blood.GetComponent<ParticleSystem>().Play();
        }

        [RPC]
        void Down()
        {
            isDowned = true;
            downTimer = 0f;

            playerShooting.DisableEffects();

            anim.SetTrigger("FallDown");

            // Turn off the movement and shooting scripts.
            playerMovement.enabled = false;
            playerShooting.enabled = false;
        }

        [RPC]
        void Up()
        {
            isDowned = false;
            currentHealth = (int)(startingHealth * reviveHealthPercentage);
            anim.SetTrigger("GetUp");
            playerMovement.enabled = true;
            playerShooting.enabled = true;
        }

        [RPC]
        void Death()
        {
            isDead = true;
            AudioSource src = Utility.PickRandomly(deathSounds);
            src.Play();
            GameState.MyPlayerDied();
        }

        public void AddHealth(int amount)
        {

            currentHealth += amount;
            if (currentHealth > startingHealth)
            {
                currentHealth = startingHealth;
            }

            Debug.Log("Gained health: " + currentHealth);
        }

        public float DownSecondsLeft()
        {
            return automaticReviveTime - downTimer;
        }
    }
}