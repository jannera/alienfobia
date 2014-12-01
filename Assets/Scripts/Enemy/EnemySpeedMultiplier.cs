using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemySpeedMultiplier : MonoBehaviour
    {
        private NavMeshAgent agent;
        private static OptionMultiplier multiplier = new OptionMultiplier(Option.Speed, new float[] { 1.0f, 1.16f, 1.33f, 1.5f });

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed *= multiplier.GetBonus();
        }
    }
}