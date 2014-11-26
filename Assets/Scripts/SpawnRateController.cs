using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class SpawnRateController : MonoBehaviour
    {
        Animator anim;
        public void Awake()
        {
            anim = GetComponent<Animator>();
            GameState.OnMyPlayerJoined += delegate()
            {
                anim.enabled = true; // only start spawning and generating monsters after player's in the game
                foreach (Transform t in transform)
                {
                    t.gameObject.SetActive(true);
                }
            };
        }

        public void Start()
        {
            GameState.OnLevelOver += delegate()
            {
                gameObject.SetActive(false);
            };
        }

        public void LevelEnded()
        {
            GameState.TimeIsUp();
        }
    }
}