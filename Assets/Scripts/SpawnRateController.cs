using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class SpawnRateController : MonoBehaviour
    {
        public Animator gameState;

        
        void Start()
        {

        }

        
        void Update()
        {

        }

        public void LevelEnded()
        {
            gameObject.SetActive(false);
            gameState.SetTrigger("LevelEnded");

            GameState.TimeIsUp();
        }
    }
}