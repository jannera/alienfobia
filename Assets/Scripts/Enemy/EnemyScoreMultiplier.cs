using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyScoreMultiplier : MonoBehaviour
    {
        private ScoreMultiplier scoreMultiplier;
        private EnemyHealth enemyHealth;

        void Start()
        {
            scoreMultiplier = GetComponent<ScoreMultiplier>();
            enemyHealth = GetComponent<EnemyHealth>();

            enemyHealth.scoreValue *= scoreMultiplier.GetMultiplier();
        }
    }
}