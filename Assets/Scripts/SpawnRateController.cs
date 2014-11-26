using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class SpawnRateController : MonoBehaviour
    {
        public void LevelEnded()
        {
            gameObject.SetActive(false);
            
            GameState.TimeIsUp();
        }
    }
}