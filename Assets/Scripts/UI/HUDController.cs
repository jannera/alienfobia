using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class HUDController : MonoBehaviour
    {

        Animator anim;

        void Awake()
        {
            anim = GetComponent<Animator>();

            GameState.OnMyPlayerDied += delegate()
            {
                anim.SetTrigger("GameOver");
            };

            GameState.OnTimeIsUp += delegate()
            {
                anim.SetTrigger("LevelEnded");
            };
        }
    }
}