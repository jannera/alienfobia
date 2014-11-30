using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CompleteProject
{
    public class ScoreMultiplierUIUpdate : MonoBehaviour
    {
        private ScoreMultiplier scoreMultiplier;
        private Text text;

        void Start()
        {
            scoreMultiplier = GetComponent<ScoreMultiplier>();
            text = GetComponent<Text>();
        }

        void Update()
        {
            text.text = "Score multiplier: " + scoreMultiplier.GetMultiplier().ToString("0.00");
        }
    }
}