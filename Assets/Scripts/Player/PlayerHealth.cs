using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CompleteProject
{
    public class PlayerHealth : CompleteProject.PhotonBehaviour
    {
        public int startingHealth = 100;
        public float currentHealth { get; private set; }

        Animator anim;
        
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
                    return;
                }
                downTimer += Time.deltaTime;
                
                if (downTimer >= automaticReviveTime && othersAlive)
                {
                    RPC(Up, PhotonTargets.All);
                }
                
            }
        }


        public void TakeDamage(float amount, Vector3 attackerPosition)
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

            anim.SetTrigger("FallDown");

            if (photonView.isMine) {
                GameState.MyPlayerDown();
            }
            else
            {
                GameState.OtherPlayerDown(photonView.owner);
            }
        }

        [RPC]
        void Up()
        {
            isDowned = false;
            currentHealth = (int)(startingHealth * reviveHealthPercentage);
            anim.SetTrigger("GetUp");
            if (photonView.isMine)
            {
                GameState.MyPlayerRevived();
            }
            else
            {
                GameState.OtherPlayerRevived(photonView.owner);
            } 
        }

        [RPC]
        void Death()
        {
            isDead = true;
            AudioSource src = Utility.PickRandomly(deathSounds);
            src.Play();
            if (photonView.isMine)
            {
                GameState.MyPlayerDied();
            }
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