using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class SpawnRateController : MonoBehaviour
    {
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