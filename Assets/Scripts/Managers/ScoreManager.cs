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

        static ScoreManager()
        {
            // if game ends because of time running out, give 25% score bonus
            GameState.OnTimeIsUp += delegate()
            {
                score = (int)(score * 1.25f);

                ScoreStorage.Store(new RowData(PhotonNetwork.player.name, score, true, new System.DateTime()));
            };

            GameState.OnMyPlayerDied += delegate()
            {
                ScoreStorage.Store(new RowData(PhotonNetwork.player.name, score, true, new System.DateTime()));
            };
        }

        void Update()
        {
            scoreText.text = "Score: " + score;
        }
    }
}