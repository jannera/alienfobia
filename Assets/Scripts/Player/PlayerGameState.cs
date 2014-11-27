using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class PlayerGameState : PhotonBehaviour
    {
        void Start()
        {
            if (photonView.isMine)
            {
                GameState.MyPlayerJoined();
            }
            else
            {
                GameState.OtherPlayerJoined(photonView.owner);
            }
        }
    }
}