using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyMovement : MonoBehaviour
    {
        Transform player;               // Reference to the player's position.
        PlayerHealth playerHealth;      // Reference to the player's health.
        EnemyHealth enemyHealth;        // Reference to this enemy's health.
        NavMeshAgent nav;               // Reference to the nav mesh agent.


        void Awake ()
        {
            // Set up the references.
            if (PhotonNetwork.isMasterClient)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
                playerHealth = player.GetComponent<PlayerHealth>();
                enemyHealth = GetComponent<EnemyHealth>();
                nav = GetComponent<NavMeshAgent>();
            }
            else
            {
                Destroy(GetComponent<NavMeshAgent>());
                Destroy(this);
            }
        }


        void Update ()
        {
            // todo now and then change player, based on proximity and chance
            if (player == null)
            {
                Debug.Log("Enemy could not find player");
                return;
            }
            // If the enemy and the player have health left...
            if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
            {
                // ... set the destination of the nav mesh agent to the player.
                nav.SetDestination (player.position);
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