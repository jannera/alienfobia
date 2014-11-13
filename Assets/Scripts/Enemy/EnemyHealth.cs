using System;
using UnityEngine;

namespace CompleteProject
{
    public class EnemyHealth : CompleteProject.PhotonBehaviour
    {
        public int startingHealth = 100;            // The amount of health the enemy starts the game with.
        private int currentHealth;                   // The current health the enemy has.
        public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
        public int scoreValue = 10;                 // The amount added to the player's score when the enemy dies.
        public AudioClip deathClip;                 // The sound to play when the enemy dies.


        Animator anim;                              // Reference to the animator.
        AudioSource enemyAudio;                     // Reference to the audio source.
        ParticleSystem hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
        CapsuleCollider capsuleCollider;            // Reference to the capsule collider.
        public bool isDead { get; private set; }                        // Whether the enemy is dead.
        bool isSinking;                             // Whether the enemy has started sinking through the floor.

        public float pickUpGenChance = 0.4f;
        private float sinkingTimer = 0;

        public PickUpRandomizer pickups;

        void Awake()
        {
            // Setting up the references.
            anim = GetComponent<Animator>();
            enemyAudio = GetComponent<AudioSource>();
            hitParticles = GetComponentInChildren<ParticleSystem>();
            capsuleCollider = GetComponent<CapsuleCollider>();

            // Setting the current health when the enemy first spawns.
            currentHealth = startingHealth;
        }


        void Update()
        {
            // If the enemy should be sinking...
            if (isSinking)
            {
                // ... move the enemy down by the sinkSpeed per second.
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

            // If the current health is less than or equal to zero...
            if (currentHealth <= 0)
            {
                ScoreManager.score += scoreValue;
                ScoreManager.kills += 1;
                // ... the enemy is dead.
                RPC(Death, PhotonTargets.All);
            }
        }

        [RPC]
        public void ReduceHealth(int amount)
        {
            // Reduce the current health by the amount of damage sustained.
            currentHealth -= amount;
        }

        [RPC]
        public void StartEffects(Vector3 localPoint)
        {
            Vector3 hitPoint = localPoint + transform.position;

            if (!enemyAudio.isPlaying) {
                enemyAudio.Play();
            }

            

            // Set the position of the particle system to where the hit was sustained.
            hitParticles.transform.position = hitPoint;

            // And play the particles.
            hitParticles.Play();
        }

        [RPC]
        void Death()
        {
            if (isDead)
            {
                return; // this can happen when multiple clients kill the same enemy at about the same time
            }
            // The enemy is dead.
            isDead = true;

            // Turn the collider into a trigger so shots can pass through it.
            capsuleCollider.isTrigger = true;

            if (UnityEngine.Random.RandomRange(0, 1f) > 0.5f)
            {
                anim.SetTrigger("Dead");
                Debug.Log("normal");
            }
            else
            {
                anim.SetTrigger("DeadMirrored");
                Debug.Log("mirrored");
            }
            

            // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
            enemyAudio.clip = deathClip;
            enemyAudio.Play();

            // randomly generate a pickup
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