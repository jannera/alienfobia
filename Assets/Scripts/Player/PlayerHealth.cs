using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CompleteProject
{
    public class PlayerHealth : CompleteProject.PhotonBehaviour
    {
        public int startingHealth = 100;                            // The amount of health the player starts the game with.
        public int currentHealth { get; private set; }                                   // The current health the player has.

        public AudioClip deathClip;                                 // The audio clip to play when the player dies
        Animator anim;                                              // Reference to the Animator component.
        AudioSource playerAudio;                                    // Reference to the AudioSource component.
        PlayerMovement playerMovement;                              // Reference to the player's movement.
        PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
        public bool isDead { get; private set; }                           // Whether the player is dead.

        public GameObject bloodParticles;


        void Awake()
        {
            // Setting up the references.
            anim = GetComponent<Animator>();
            playerAudio = GetComponent<AudioSource>();
            playerMovement = GetComponent<PlayerMovement>();
            playerShooting = GetComponentInChildren<PlayerShooting>();

            // Set the initial health of the player.
            currentHealth = startingHealth;
        }


        void Update()
        {
        }


        public void TakeDamage(int amount, Vector3 attackerPosition)
        {
            // Reduce the current health by the damage amount.
            currentHealth -= amount;

            RPC(ShowDamageEffects, PhotonTargets.All);

            // If the player has lost all it's health and the death flag hasn't been set yet...
            if (currentHealth <= 0 && !isDead)
            {
                // ... it should die.
                RPC(Death, PhotonTargets.All);
            }
        }

        [RPC]
        public void ShowDamageEffects()
        {
            // Play the hurt sound effect.
            playerAudio.Play();

            // bloodParticles.transform.rotation.SetFromToRotation(transform.position, attackerPosition);
            
            GameObject blood = (GameObject) Instantiate(bloodParticles, transform.position, Quaternion.identity);
            blood.GetComponent<ParticleSystem>().Play();
        }

        [RPC]
        void Death()
        {
            // Set the death flag so this function won't be called again.
            isDead = true;

            // Turn off any remaining shooting effects.
            playerShooting.DisableEffects();

            // Tell the animator that the player is dead.
            anim.SetTrigger("Die");

            // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
            playerAudio.clip = deathClip;
            playerAudio.Play();

            // Turn off the movement and shooting scripts.
            playerMovement.enabled = false;
            playerShooting.enabled = false;
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
    }
}