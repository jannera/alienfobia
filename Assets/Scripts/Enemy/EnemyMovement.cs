using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyMovement : MonoBehaviour
    {
        Transform player;               // Reference to the player's position.
        EnemyHealth enemyHealth;        // Reference to this enemy's health.
        NavMeshAgent nav;               // Reference to the nav mesh agent.

        void Awake()
        {
            // Set up the references.
            if (PhotonNetwork.isMasterClient)
            {
                player = FindClosestPlayer(); 
                enemyHealth = GetComponent<EnemyHealth>();
                nav = GetComponent<NavMeshAgent>();
            }
            else
            {
                Destroy(GetComponent<NavMeshAgent>());
                Destroy(this);
            }
        }

        private Transform FindClosestPlayer()
        {
            GameObject playerGO = PlayerManager.GetClosestPlayerAlive(transform.position);
            if (playerGO == null) {
                return null;
            }
            
            return playerGO.transform;
        }

        void Update()
        {
            if (Random.Range(0f, 1f) < Time.deltaTime)
            {
                // randomly, once every second on average, find the closest player
                player = FindClosestPlayer(); 
            }

            if (player == null)
            {
                return;
            }
            // If the enemy and the player have health left...
            if (!enemyHealth.isDead && PlayerManager.AreAnyPlayersAlive())
            {
                // ... set the destination of the nav mesh agent to the player.
                nav.SetDestination(player.position);
            }
            // Otherwise...
            else
            {
                // ... disable the nav mesh agent.
                nav.enabled = false;
            }
        }
    }
}