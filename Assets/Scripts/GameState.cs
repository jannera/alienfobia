using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompleteProject
{
    // for communicating about major gamestate changes cross all components
    public class GameState
    {
        public static event DMyPlayerJoined OnMyPlayerJoined;
        public static event DMyPlayerDied OnMyPlayerDied;
        public static event DOtherPlayerJoined OnOtherPlayerJoined;
        public static event DTimeIsUp OnTimeIsUp;
        public static event DLevelOver OnLevelOver;

        public delegate void DMyPlayerJoined();
        public delegate void DMyPlayerDied();
        public delegate void DOtherPlayerJoined(PhotonPlayer player);
        public delegate void DLevelOver();
        public delegate void DTimeIsUp();

        public static void TimeIsUp()
        {
            OnTimeIsUp();
        }

        static GameState()
        {
            // these local listeners ensure that at least one listener exists for every event (so there's no need to check for nulls)
            OnMyPlayerJoined += delegate() { };
            OnOtherPlayerJoined += delegate(PhotonPlayer player) { };
            OnLevelOver += delegate() { };
            // current level always ends the level when player dies or timer runs out. maybe later on there will be different levels..
            OnMyPlayerDied += delegate() { OnLevelOver(); };
            OnTimeIsUp += delegate() { OnLevelOver(); };
        }
    }
}
