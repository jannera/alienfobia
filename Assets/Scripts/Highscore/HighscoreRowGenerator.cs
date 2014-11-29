using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CompleteProject
{
    public class HighscoreRowGenerator : MonoBehaviour
    {
        public GameObject rowPreFab;
        public Color lastGameColor;
        private bool local = true;
        public Text switchButtonText;

        private int waitMaxSeconds = 10;
        private int secondsWaited = 0;

        void Start()
        {
            // todo: fetch earlier global/local setting from playerprefs
            Init();
        }

        private void Init()
        {
            if (ScoreStorage.globalScores == null || ScoreStorage.localScores == null)
            {
                // give the API some more time
                if (secondsWaited >= waitMaxSeconds)
                {
                    Debug.LogError("Failed to load scores within " + secondsWaited + " seconds"); // todo put this in UI
                }
                else
                {
                    secondsWaited++;
                    Invoke(((System.Action) Init).Method.Name, 1); // try again in a second
                }
            }
            else
            {
                CreateUIRows();
                UpdateUI();
            }
        }

        private void CreateUIRows(RowData[] allData)
        {
            int amount = HowManyRowsCanFitInScreen();
            amount = Mathf.Min(allData.Length, amount);

            int startIndex = GetFirstRowIndex(allData, amount);

            RowData[] data = new RowData[amount];

            System.Array.Copy(allData, startIndex, data, 0, amount);

            for (int i = 0; i < data.Length; i++)
            {
                GameObject o = (GameObject)Instantiate(rowPreFab);
                o.transform.SetParent(this.transform, false);
                HighscoreRow row = o.GetComponent<HighscoreRow>();
                row.SetValues(startIndex + i + 1, data[i].name, data[i].score);
                float prefHeight = row.GetPrefHeight();
                RectTransform t = o.GetComponent<RectTransform>();
                t.anchoredPosition = new Vector2(0, -prefHeight * i); // TODO: a grid might be better than setting the position by hand
                if (data[i].lastGame)
                {
                    row.SetColor(lastGameColor);
                }
            }
        }

        private int GetFirstRowIndex(RowData[] allData, int amount)
        {
            if (allData.Length == 0) {
                return 0;
            }

            int currentScoreIndex = System.Array.FindIndex(allData, delegate(RowData d)
            {
                return d.lastGame;
            });

            if (currentScoreIndex == -1)
            {
                Debug.LogError("Couldn't find current game amongst the game data");
                currentScoreIndex = 0;
            }

            int beforeCurrent = Mathf.CeilToInt((amount - 1) / 2f);
            
            int startIndex = currentScoreIndex - beforeCurrent;

            return Mathf.Clamp(startIndex, 0, allData.Length - amount);
        }

        private int HowManyRowsCanFitInScreen()
        {
            GameObject o = (GameObject)Instantiate(rowPreFab);
            HighscoreRow row = o.GetComponent<HighscoreRow>();
            float prefHeight = row.GetPrefHeight();
            float heightAvailable = GetComponent<RectTransform>().rect.height;
            return Mathf.FloorToInt(heightAvailable / prefHeight);
        }

        public void SwitchList()
        {
            local = !local;
            UpdateUI();

            CreateUIRows();
        }

        private void UpdateUI()
        {
            if (local)
            {
                switchButtonText.text = "Switch to Global";
            }
            else
            {
                switchButtonText.text = "Switch to Local";
            }
        }

        private void CreateUIRows()
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            if (local)
            {
                CreateUIRows(ScoreStorage.localScores);
            }
            else
            {
                CreateUIRows(ScoreStorage.globalScores);
            }
        }

        void Update()
        {

        }
    }

    public class RowData
    {
        public RowData(string name, float score, bool lastGame, System.DateTime createdOn)
        {
            this.name = name;
            this.score = score;
            this.lastGame = lastGame;
            this.createdOn = createdOn;
        }
        public string name;
        public float score;
        public bool lastGame;
        public System.DateTime createdOn;
    }
}