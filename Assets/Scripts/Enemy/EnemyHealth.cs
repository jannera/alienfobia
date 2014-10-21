using System;
using UnityEngine;

namespace CompleteProject
{
    public class EnemyHealth : Photon.MonoBehaviour
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
        public GameObject droppedPickUp;


        void Awake ()
        {
            // Setting up the references.
            anim = GetComponent <Animator> ();
            enemyAudio = GetComponent <AudioSource> ();
            hitParticles = GetComponentInChildren <ParticleSystem> ();
            capsuleCollider = GetComponent <CapsuleCollider> ();

            // Setting the current health when the enemy first spawns.
            currentHealth = startingHealth;
        }


        void Update ()
        {
            // If the enemy should be sinking...
            if(isSinking)
            {
                // ... move the enemy down by the sinkSpeed per second.
                transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
            }
        }


        public void TakeDamage (int amount, Vector3 hitPoint)
        {
            // If the enemy is dead...
            if(isDead)
                // ... no need to take damage so exit the function.
                return;

            // TODO: damage each enemy takes must be told to other clients.. and other clients need to play SFXs for enemies when they take damage..
            // Play the hurt sound effect.
            enemyAudio.Play ();

            // Reduce the current health by the amount of damage sustained.
            currentHealth -= amount;
            
            // Set the position of the particle system to where the hit was sustained.
            hitParticles.transform.position = hitPoint;

            // And play the particles.
            hitParticles.Play();

            // If the current health is less than or equal to zero...
            if(currentHealth <= 0)
            {
                // ... the enemy is dead.
                photonView.RPC("Death", PhotonTargets.All, new object[] {});
            }
        }

        [RPC]
        void Death()
        {
            if (isDead)
            {
                return; // this can happen when multiple clients kill the same bear same time
            }
            // The enemy is dead.
            isDead = true;

            // Turn the collider into a trigger so shots can pass through it.
            capsuleCollider.isTrigger = true;

            // Tell the animator that the enemy is dead.
            anim.SetTrigger("Dead");

            // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
            enemyAudio.clip = deathClip;
            enemyAudio.Play();

            // randomly generate a pickup
            if (PhotonNetwork.isMasterClient && UnityEngine.Random.Range(0f, 1f) < pickUpGenChance)
            {
                object[] p = { };
                PhotonNetwork.InstantiateSceneObject(droppedPickUp.name, transform.position + Vector3.up * 1f, Quaternion.identity, 0, p);
            }
        }


        public void StartSinking ()
        {
            if (PhotonNetwork.isMasterClient)
            {
                GetComponent<NavMeshAgent>().enabled = false;

                // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
                GetComponent<Rigidbody>().isKinematic = true;
            }
            
            isSinking = true;

            ScoreManager.score += scoreValue;
			ScoreManager.kills += 1;

            // After 2 seconds destroy the enemy.
            Destroy (gameObject, 2f);
        }
    }
}