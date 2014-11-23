using UnityEngine;
using System.Collections;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.user;
using com.shephertz.app42.paas.sdk.csharp.storage;
using com.shephertz.app42.paas.sdk.csharp.game;
using System.Collections.Generic;

namespace CompleteProject
{
    public class ScoreStorage
    {
        private static string API_KEY    = "b0fed0d17e6038da2cb3ce0f7aecd94f07d77a3e33cc33e06dedc3d6611d1bd0";
        private static string SECRET_KEY = "8fc18bd8fe13255ce908a26b800ea4bb938504b3965bbbe138854a498ba259c1";

        private static ServiceAPI serviceApi;
        private static ScoreService scoreService;
        private static ScoreBoardService scoreBoardService;
        private static GameService gameService;

        private static string gameName;
        private static string levelRevision = "001";
        private static string currentGameScoreId;
        private static string localPlayerName;

        public static RowData[] localScores { get; private set; }
        public static RowData[] globalScores { get; private set; }

        private static void FetchLocalScores()
        {
            scoreBoardService.GetScoresByUser(gameName, localPlayerName, new LocalScoreSearchCallBack());
        }

        private static void FetchGlobalScores()
        {
            scoreBoardService.GetTopNRankers(gameName, 100, new GlobalScoreSearchCallBack());
        }

        private class LocalScoreSearchCallBack : App42CallBack
        {
            public void OnSuccess(object response)
            {
                localScores = TransformScoresToRows((Game)response);
            }

            public void OnException(System.Exception e)
            {
                Debug.LogError("While trying to fetch local score: " + e);
            }
        }

        private class GlobalScoreSearchCallBack : App42CallBack
        {
            public void OnSuccess(object response)
            {
                globalScores = TransformScoresToRows((Game)response);
            }

            public void OnException(System.Exception e)
            {
                Debug.LogError("While trying to fetch global score: " + e);
            }
        }

        private static RowData[] TransformScoresToRows(Game game) {
            IList<Game.Score> scores = game.GetScoreList();
            RowData[] rows = new RowData[scores.Count];
            for (int i = 0; i < scores.Count; i++)
            {
                Game.Score s = scores[i];
                rows[i] = new RowData(s.userName, (float) s.GetValue(), s.GetScoreId() == currentGameScoreId); // TODO handle the comparison differently in global list
            }
            Sort(rows);
            return rows;
        }

        private static void BuildAPI()
        {
            if (serviceApi != null)
            {
                return;
            }
            gameName = Application.loadedLevelName + "_" + levelRevision;
            serviceApi = new ServiceAPI(API_KEY, SECRET_KEY);
            scoreBoardService = serviceApi.BuildScoreBoardService();
            gameService = serviceApi.BuildGameService();
            // App42Log.SetDebug(true);
        }

        public static bool Store(RowData data)
        {
            BuildAPI();

            localPlayerName = data.name;

            gameService.GetGameByName(gameName, new GameSearchCallback(data));

            return true;
        }

        private class GameSearchCallback : App42CallBack
        {
            RowData d;
            public GameSearchCallback(RowData d)
            {
                this.d = d;
            }

            public void OnSuccess(object response)
            {
                scoreBoardService.SaveUserScore(gameName, d.name, d.score, new ScoreSaveCallback());
            }

            public void OnException(System.Exception e)
            {
                Debug.LogWarning("While trying to find game : " + e);
                gameService.CreateGame(gameName, "dummy desc", new GameCreationCallBack(d));
            }
        }

        private class GameCreationCallBack : App42CallBack
        {
            RowData d;
            public GameCreationCallBack(RowData d)
            {
                this.d = d;
            }

            public void OnSuccess(object response)
            {
                scoreBoardService.SaveUserScore(gameName, d.name, d.score, new ScoreSaveCallback());
            }

            public void OnException(System.Exception e)
            {
                Debug.LogError("While trying to create game : " + e);
            }
        }

        private class ScoreSaveCallback : App42CallBack
        {
            public void OnSuccess(object response)
            {
                Game game = (Game)response;
                currentGameScoreId = game.GetScoreList()[0].GetScoreId();
                FetchLocalScores();
                FetchGlobalScores();
                // Debug.Log("Saving succeeded");
                // LogGameData(game);
            }

            public void OnException(System.Exception e)
            {
                Debug.LogError("While trying to save score: " + e);
            }
        }

        private static void LogGameData(Game game)
        {
            Debug.Log("gameName is " + game.GetName());
            for (int i = 0; i < game.GetScoreList().Count; i++)
            {
                Debug.Log("userName is : " + game.GetScoreList()[i].GetUserName());
                Debug.Log("score is : " + game.GetScoreList()[i].GetValue());
                Debug.Log("scoreId is : " + game.GetScoreList()[i].GetScoreId());
            }
        }

        private static RowData[] GenerateRandom()
        {
            int rows = Random.Range(3, 12);
            RowData[] results = new RowData[rows];
            bool first = true;
            for (int i = 0; i < rows; i++)
            {
                int nameChars = Random.Range(1, 40);
                string name = "";
                for (int c = 0; c < nameChars; c++)
                {
                    name += chars[Random.Range(0, chars.Length)];
                }

                float score = Random.Range(1, 100000);
                results[i] = new RowData(name, score, first);
                if (first)
                {
                    first = false;
                }
            }
            Sort(results);
            return results;
        }

        private static void Sort(RowData[] results)
        {
            System.Array.Sort(results, delegate(RowData a, RowData b)
            {
                return b.score.CompareTo(a.score);
            });
        }

        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
    }
}