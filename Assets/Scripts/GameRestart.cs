using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class GameRestart : MonoBehaviour
    {
        public void RestartGame()
        {
            GameState.ResetListeners();
            ScoreStorage.Reset();
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            Application.LoadLevel(0);
        }
    }
}