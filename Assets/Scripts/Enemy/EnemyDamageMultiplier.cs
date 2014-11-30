using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyDamageMultiplier : MonoBehaviour
    {
        private EnemyAttack enemyAttack;
        private static OptionMultiplier multiplier = new OptionMultiplier(Option.Damage, new float[] { 1.0f, 1.25f, 1.5f, 1.75f });

        void Awake()
        {
            enemyAttack = GetComponent<EnemyAttack>();
            enemyAttack.attackDamage *= multiplier.GetBonus();
        }
    }
}