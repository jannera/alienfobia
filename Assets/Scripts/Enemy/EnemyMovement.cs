using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyMovement : MonoBehaviour
    {
        Transform player;
        EnemyHealth enemyHealth;
        NavMeshAgent nav;

        void Awake()
        {
            if (PhotonNetwork.isMasterClient)
            {
                player = FindClosestPlayer(); 
                enemyHealth = GetComponent<EnemyHealth>();
                nav = GetComponent<NavMeshAgent>();

                GameState.OnLevelOver += delegate()
                {
                    if (this != null)
                    {
                        this.enabled = false;
                        nav.enabled = false;
                    }
                };
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
            
            if (!enemyHealth.isDead && PlayerManager.AreAnyPlayersAlive())
            {
                nav.SetDestination(player.position);
            }
            else
            {
                nav.enabled = false;
            }
        }
    }
}