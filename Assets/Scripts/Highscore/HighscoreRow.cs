using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CompleteProject
{
    public class HighscoreRow : MonoBehaviour
    { 
        private Text pos, playerName, score;

        void Awake()
        {
            pos = GetComponentsInChildren<Text>()[0];
            playerName = GetComponentsInChildren<Text>()[1];
            score = GetComponentsInChildren<Text>()[2];
        }

        public void SetValues(int pos, string name, float score)
        {
            this.pos.text = "#" + pos;
            this.playerName.text = name;
            this.score.text = score.ToString("0");
        }

        public void SetColor(Color c)
        {
            pos.color = c;
            playerName.color = c;
            score.color = c;
        }

        public float GetPrefHeight()
        {
            return GetComponent<RectTransform>().rect.height;
        }
    }
}