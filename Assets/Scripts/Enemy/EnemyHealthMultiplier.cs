using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyHealthMultiplier : MonoBehaviour
    {
        private EnemyHealth enemyHealth;
        private static OptionMultiplier multiplier = new OptionMultiplier(Option.Health, new float[] { 1.0f, 1.25f, 1.5f, 1.75f });

        void Awake()
        {
            enemyHealth = GetComponent<EnemyHealth>();
            enemyHealth.startingHealth *= multiplier.GetBonus();
        }
    }
}