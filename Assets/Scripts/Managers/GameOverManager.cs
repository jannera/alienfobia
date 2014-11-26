using UnityEngine;

namespace CompleteProject
{
    public class GameOverManager : MonoBehaviour
    {
        PlayerHealth playerHealth;

        Animator anim;

        void Awake()
        {
            anim = GetComponent<Animator>();
        }

        void Update()
        {

            if (playerHealth == null)
            {
                playerHealth = PlayerManager.GetComponentFromMyPlayer<PlayerHealth>();

                if (playerHealth == null)
                {
                    return;
                }
            }
            
            if (playerHealth.isDead)
            {
                this.enabled = false;
                anim.SetTrigger("GameOver"); // todo: put a script on animator to listen for state changes
            }
        }
    }
}