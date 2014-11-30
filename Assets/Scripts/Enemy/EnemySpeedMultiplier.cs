using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemySpeedMultiplier : MonoBehaviour
    {
        private NavMeshAgent agent;
        private static OptionMultiplier multiplier = new OptionMultiplier(Option.Speed, new float[] { 1.0f, 1.25f, 1.5f, 1.75f });

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed *= multiplier.GetBonus();
        }
    }
}