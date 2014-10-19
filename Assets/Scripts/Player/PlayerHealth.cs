﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CompleteProject
{
    public class PlayerHealth : MonoBehaviour
    {
        public int startingHealth = 100;                            // The amount of health the player starts the game with.
        public int currentHealth;                                   // The current health the player has.

        public AudioClip deathClip;                                 // The audio clip to play when the player dies.
        


        Animator anim;                                              // Reference to the Animator component.
        AudioSource playerAudio;                                    // Reference to the AudioSource component.
        PlayerMovement playerMovement;                              // Reference to the player's movement.
        PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
        bool isDead;                                                // Whether the player is dead.
        bool damaged;                                               // True when the player gets damaged.

        public ParticleSystem bloodParticles;


        void Awake ()
        {
            // Setting up the references.
            anim = GetComponent <Animator> ();
            playerAudio = GetComponent <AudioSource> ();
            playerMovement = GetComponent <PlayerMovement> ();
            playerShooting = GetComponentInChildren <PlayerShooting> ();

            // Set the initial health of the player.
            currentHealth = startingHealth;
        }


        void Update ()
        {
        }


        public void TakeDamage (int amount, Vector3 attackerPosition)
        {
            // Set the damaged flag so the screen will flash.
            damaged = true;

            // Reduce the current health by the damage amount.
            currentHealth -= amount;

            // Play the hurt sound effect.
            playerAudio.Play ();



            // bloodParticles.transform.rotation.SetFromToRotation(transform.position, attackerPosition);
            object[] p = {};
            GameObject blood = PhotonNetwork.InstantiateSceneObject(bloodParticles.name, transform.position, Quaternion.identity, 0, p);
            blood.GetComponent<ParticleSystem>().Play();

            // If the player has lost all it's health and the death flag hasn't been set yet...
            if(currentHealth <= 0 && !isDead)
            {
                // ... it should die.
                Death ();
            }
        }


        void Death ()
        {
            // Set the death flag so this function won't be called again.
            isDead = true;

            // Turn off any remaining shooting effects.
            playerShooting.DisableEffects ();

            // Tell the animator that the player is dead.
            anim.SetTrigger ("Die");

            // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
            playerAudio.clip = deathClip;
            playerAudio.Play ();

            // Turn off the movement and shooting scripts.
            playerMovement.enabled = false;
            playerShooting.enabled = false;
        }	

		public void AddHealth (int amount) {

			currentHealth = currentHealth += amount;
			if (currentHealth > startingHealth) {
				currentHealth = startingHealth;
			}

			Debug.Log("Gained health: " + currentHealth);
		}
    }
}