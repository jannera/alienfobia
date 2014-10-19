using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CompleteProject
{
    public class ScoreManager : MonoBehaviour
    {
        public static int score;        // The player's score.
		public static int kills;        // The player's total kills
		public Text killsText;
		public Text scoreText;


        void Awake ()
        {
            // Reset the score.
            score = 0;
			kills = 0;
        }


        void Update ()
        {
            // Set the displayed text to be the word "Score" followed by the score value.
            killsText.text = "Kills: " + kills;
			scoreText.text = "Score: " + score;
        }
    }
}