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
        private static ScoreBoardService scoreBoardService;
        private static GameService gameService;

        private static string gameName;
        private static string levelRevision = "006";
        private static string currentGameScoreId; 

        public static RowData[] localScores { get; private set; }
        public static RowData[] globalScores { get; private set; }

        private delegate void SuccessHandler(object response);
        private delegate void ExceptionHandler(System.Exception e);

        private static App42CallBack Wrap(SuccessHandler onSuccess)
        {
            return Wrap(onSuccess, delegate(System.Exception e)
            {
                Debug.LogError("While communicating with App42: " + e);
            });
        }

        // wraps delegates into DefaultCallBacks, so the code is easier to read
        private static App42CallBack Wrap(SuccessHandler onSuccess, ExceptionHandler onException)
        {
            return new DefaultCallBack(onSuccess, onException);
        }

        private class DefaultCallBack : App42CallBack
        {
            SuccessHandler onSuccess;
            ExceptionHandler onException;

            public DefaultCallBack(SuccessHandler onSuccess, ExceptionHandler onException)
            {
                this.onSuccess = onSuccess;
                this.onException = onException;
            }
            public void OnSuccess(object response)
            {
                onSuccess(response);
            }

            public void OnException(System.Exception e)
            {
                onException(e);
            }
        }

        private static void FetchLocalScores(string localPlayerName)
        {
            scoreBoardService.GetScoresByUser(gameName, localPlayerName, Wrap(delegate(object response)
            {
                localScores = TransformScoresToRows((Game)response);
            }));
        }

        private static void FetchGlobalScores(string localPlayerName)
        {
            scoreBoardService.GetTopNRankers(gameName, 100, Wrap(delegate(object response)
            {
                globalScores = TransformScoresToRows((Game)response);
                System.Array.ForEach(globalScores, delegate(RowData d)
                {
                    d.lastGame = d.name == localPlayerName;
                });
            }));
        }

        private static RowData[] TransformScoresToRows(Game game)
        {
            IList<Game.Score> scores = game.GetScoreList();
            RowData[] rows = new RowData[scores.Count];
            for (int i = 0; i < scores.Count; i++)
            {
                Game.Score s = scores[i];
                rows[i] = new RowData(s.userName, (float)s.GetValue(), s.GetScoreId() == currentGameScoreId, s.createdOn);
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

            // this is stored as variable (and not created inside the gameService.GetGameByName -call), because 
            // it can be called directly after confirming the game exists, or after checking that 
            // the game does not exist and creating a new game.
            SuccessHandler saveScore = delegate(object response)
            {
                scoreBoardService.SaveUserScore(gameName, data.name, data.score, Wrap(delegate(object r)
                {
                    // after saving the score..
                    Game game = (Game)r;
                    currentGameScoreId = game.GetScoreList()[0].GetScoreId(); // store the current game's id
                    // start fetching and storing the local and global scores
                    FetchLocalScores(data.name);
                    FetchGlobalScores(data.name);
                }));
            };

            gameService.GetGameByName(gameName, Wrap(
                saveScore, // if a game exists, just save the score
                delegate(System.Exception e)
                {
                    // if a game did not exist..
                    Debug.LogWarning("While trying to find game : " + e);
                    gameService.CreateGame(gameName, "dummy desc", Wrap(saveScore)); // create it, and as response to saving game, save the score
                }
            ));

            return true;
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
                results[i] = new RowData(name, score, first, new System.DateTime());
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
                if (a.score == b.score)
                {
                    return a.createdOn.CompareTo(b.createdOn);
                }
                return b.score.CompareTo(a.score);
            });
        }

        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";

        public static void Reset()
        {
            serviceApi = null;
            scoreBoardService = null;
            gameService = null;

            currentGameScoreId = null;

            localScores = null;
            globalScores = null;
        
        }
    }
}