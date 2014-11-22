using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CompleteProject
{
    public class ScoreManager : MonoBehaviour
    {
        public static int score;
        public static int kills;
        public Text scoreText;

        void Awake()
        {
            score = 0;
            kills = 0;
        }

        void Update()
        {
            scoreText.text = "Score: " + score;
        }
    }
}