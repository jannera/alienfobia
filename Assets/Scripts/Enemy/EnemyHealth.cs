using System;
using UnityEngine;

namespace CompleteProject
{
    public class EnemyHealth : CompleteProject.PhotonBehaviour
    {
        public int startingHealth = 100;
        private int currentHealth;
        public float sinkSpeed = 2.5f;
        public int scoreValue = 10;


        Animator anim;
        ParticleSystem hitParticles;
        CapsuleCollider capsuleCollider;
        public bool isDead { get; private set; }
        bool isSinking;

        public float pickUpGenChance = 0.4f;
        private float sinkingTimer = 0;
        

        public PickUpRandomizer pickups;
        private AudioSource[] deathSounds;
        private AudioSource[] hurtSounds;
        private AudioLibraryController allAudio;

        void Awake()
        {
            anim = GetComponent<Animator>();
            hitParticles = GetComponentInChildren<ParticleSystem>();
            capsuleCollider = GetComponent<CapsuleCollider>();

            currentHealth = startingHealth;

            allAudio = GetComponent<AudioLibraryController>();
            deathSounds = allAudio.GetByTag("AudioDeath");
            hurtSounds = allAudio.GetByTag("AudioHurt");
        }


        void Update()
        {
            if (isSinking)
            {
                transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
                sinkingTimer += Time.deltaTime;
                if ((sinkingTimer > 2 || sinkSpeed == 0) && PhotonNetwork.isMasterClient)
                {
                    // after 2 sec sinking destroy the enemy
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }


        public void TakeDamage(int amount, Vector3 hitPoint)
        {
            if (isDead)
            {
                return;
            }
            
            RPC<int>(ReduceHealth, PhotonTargets.All, amount);
            Vector3 localDiff = hitPoint - transform.position; // calculate the local position in relation to the enemy, because the enemy's position varies from client to client
            RPC<Vector3>(StartEffects, PhotonTargets.All, localDiff);

            if (currentHealth <= 0)
            {
                ScoreManager.score += scoreValue;
                ScoreManager.kills += 1;
                RPC(Death, PhotonTargets.All);
            }
        }

        [RPC]
        public void ReduceHealth(int amount)
        {
            currentHealth -= amount;
        }

        [RPC]
        public void StartEffects(Vector3 localPoint)
        {
            Vector3 hitPoint = localPoint + transform.position;

            
            allAudio.PlayOnlyOne(hurtSounds);
            
            // Set the position of the particle system to where the hit was sustained.
            hitParticles.transform.position = hitPoint;

            hitParticles.Play();
        }

        [RPC]
        void Death()
        {
            if (isDead)
            {
                return; // this can happen when multiple clients kill the same enemy at about the same time
            }
            
            isDead = true;

            // Turn the collider into a trigger so shots can pass through it.
            capsuleCollider.isTrigger = true;

            if (UnityEngine.Random.Range(0, 1f) > 0.5f)
            {
                anim.SetTrigger("Dead");
            }
            else
            {
                anim.SetTrigger("DeadMirrored");
            }
            
            allAudio.PlayOnlyOne(deathSounds);
            
            pickups.DropRandomly(pickUpGenChance, transform.position + Vector3.up * 1f);
            Invoke(StartSinking, 1f);
        }


        public void StartSinking()
        {
            isSinking = true;
            if (PhotonNetwork.isMasterClient)
            {
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                if (agent != null) {
                    agent.enabled = false;
                }

                // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}