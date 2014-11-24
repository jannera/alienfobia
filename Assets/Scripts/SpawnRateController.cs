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
            Debug.Log("Level ended!");
            ScoreManager.score = (int) (ScoreManager.score * 1.25f);
            gameObject.SetActive(false);
            gameState.SetTrigger("LevelEnded");
        }
    }
}