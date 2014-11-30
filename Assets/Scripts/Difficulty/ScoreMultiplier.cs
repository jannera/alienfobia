using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class ScoreMultiplier : MonoBehaviour
    {
        private OptionMultiplier[] bonusDefininitions;

        void Start()
        {
            bonusDefininitions = new OptionMultiplier[] {
                new OptionMultiplier(Option.Health, new float[] {0, 0.25f, 0.5f, 1f}),
                new OptionMultiplier(Option.Speed, new float[]  {0, 0.5f,  1f, 1.5f}),
                new OptionMultiplier(Option.Damage, new float[] {0, 0.25f, 0.5f, 1f}),
                new OptionMultiplier(Option.Amount, new float[] {0, 0, 0, 0}),
            };
        }

        public float GetMultiplier()
        {
            float result = 1.0f;

            for (int i = 0; i < bonusDefininitions.Length; i++)
            {
                result += bonusDefininitions[i].GetBonus();
            }

            return result;
        }
    }

    public class OptionMultiplier
    {
        public Option option;
        public float[] scoreBonus;

        public OptionMultiplier(Option option, float[] scoreBonus)
        {
            if (scoreBonus.Length != 4)
            {
                Debug.LogError("Expected exactly 4 bonus definitions");
            }
            this.scoreBonus = scoreBonus;
            this.option = option;
        }

        public float GetBonus()
        {
            return scoreBonus[DifficultyOptionPersister.GetValue(option)];
        }
    }
}