using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyAttack : MonoBehaviour
    {
        public float timeBetweenAttacks = 0.5f;
        public int attackDamage = 10;


        Animator anim;
        GameObject player;
        PlayerHealth playerHealth;
        EnemyHealth enemyHealth;
        bool playerInRange;
        float timer;
        private AudioLibraryController allAudio;
        private AudioSource[] attackSounds;

        void Awake()
        {
            player = PlayerManager.GetMyPlayer();
            playerHealth = player.GetComponent<PlayerHealth>();
            enemyHealth = GetComponent<EnemyHealth>();
            anim = GetComponent<Animator>();
            allAudio = GetComponent<AudioLibraryController>();
            attackSounds = allAudio.GetByTag("AudioAttack");

            GameState.OnLevelOver += delegate()
            {
                if (this != null)
                {
                    anim.SetTrigger("PlayerDead");
                    this.enabled = false;
                }
            };
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player)
            {
                playerInRange = true;
            }
        }


        void OnTriggerExit(Collider other)
        {
            if (other.gameObject == player)
            {
                playerInRange = false;
            }
        }


        void Update()
        {
            if (player == null)
            {
                return;
            }
            timer += Time.deltaTime;

            if (timer >= timeBetweenAttacks && playerInRange && !enemyHealth.isDead)
            {
                Attack();
            }
        }


        void Attack()
        {
            anim.SetTrigger("Attack");
            timer = 0f;

            allAudio.PlayOnlyOne(attackSounds);
            
            if (playerHealth.currentHealth > 0)
            {
                playerHealth.TakeDamage(attackDamage, transform.position);
            }
        }
    }
}