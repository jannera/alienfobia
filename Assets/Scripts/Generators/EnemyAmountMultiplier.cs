using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyAmountMultiplier : MonoBehaviour
    {
        private static OptionMultiplier multiplier = new OptionMultiplier(Option.Amount, new float[] { 1.0f, 1.25f, 1.5f, 1.75f });
        
        void Start()
        {
            SimpleGOGenerator generator = GetComponent<SimpleGOGenerator>();
            generator.difficultyAmountMultiplier = multiplier.GetBonus();
        }
    }
}