using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class PlayerGameState : PhotonBehaviour
    {
        // Use this for initialization
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}