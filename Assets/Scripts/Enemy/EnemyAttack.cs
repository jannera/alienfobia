﻿using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyAttack : MonoBehaviour
    {
        public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
        public int attackDamage = 10;               // The amount of health taken away per attack.


        Animator anim;                              // Reference to the animator component.
        GameObject player;                          // Reference to the player GameObject.
        PlayerHealth playerHealth;                  // Reference to the player's health.
        EnemyHealth enemyHealth;                    // Reference to this enemy's health.
        bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
        float timer;                                // Timer for counting up to the next attack.


        void Awake()
        {
            // Setting up the references.
            player = PlayerManager.GetMyPlayer();
            playerHealth = player.GetComponent<PlayerHealth>();
            enemyHealth = GetComponent<EnemyHealth>();
            anim = GetComponent<Animator>();
        }


        void OnTriggerEnter(Collider other)
        {
            // If the entering collider is the player...
            if (other.gameObject == player)
            {
                // ... the player is in range.
                playerInRange = true;
            }
        }


        void OnTriggerExit(Collider other)
        {
            // If the exiting collider is the player...
            if (other.gameObject == player)
            {
                // ... the player is no longer in range.
                playerInRange = false;
            }
        }


        void Update()
        {
            if (player == null)
            {
                return;
            }
            // Add the time since Update was last called to the timer.
            timer += Time.deltaTime;

            // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
            if (timer >= timeBetweenAttacks && playerInRange && !enemyHealth.isDead)
            {
                // ... attack.
                Attack();
            }

            if (!PlayerManager.AreAnyPlayersAlive())
            {
                anim.SetTrigger("PlayerDead");
            }
        }


        void Attack()
        {
            // Reset the timer.
            timer = 0f;

            // If the player has health to lose...
            if (playerHealth.currentHealth > 0)
            {
                // ... damage the player.
                playerHealth.TakeDamage(attackDamage, transform.position);
            }
        }
    }
}