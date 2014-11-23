using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class HighscoreRowGenerator : MonoBehaviour
    {
        public GameObject rowPreFab;
        public Color lastGameColor;

        // Use this for initialization
        void Start()
        {
            ScoreStorage.Fetch(PhotonNetwork.player.name, HandleResponse);
        }

        public void HandleResponse(RowData[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                GameObject o = (GameObject)Instantiate(rowPreFab);
                o.transform.SetParent(this.transform, false);
                HighscoreRow row = o.GetComponent<HighscoreRow>();
                row.SetValues(i + 1, data[i].name, data[i].score);
                float prefHeight = row.GetPrefHeight();
                RectTransform t = o.GetComponent<RectTransform>();
                t.anchoredPosition = new Vector2(0, -prefHeight * i); // TODO: a grid might be better than setting the position by hand
                if (data[i].lastGame)
                {
                    row.SetColor(lastGameColor);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public class RowData
    {
        public RowData(string name, float score, bool lastGame)
        {
            this.name = name;
            this.score = score;
            this.lastGame = lastGame;
        }
        public string name;
        public float score;
        public bool lastGame;
    }
}