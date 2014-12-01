using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompleteProject
{
    // for communicating about major gamestate changes cross all components
    public class GameState
    {
        public static event DelegateNoParams OnMyPlayerJoined;
        public static event DelegateNoParams OnMyPlayerDied;
        public static event DelegatePlayer OnOtherPlayerJoined;
        public static event DelegateNoParams OnTimeIsUp;
        public static event DelegateNoParams OnLevelOver;
        public static event DelegatePlayer OnOtherPlayerDown;
        public static event DelegatePlayer OnOtherPlayerRevived;
        public static event DelegateNoParams OnMyPlayerDown;
        public static event DelegateNoParams OnMyPlayerRevived;

        public delegate void DelegateNoParams();
        public delegate void DelegatePlayer(PhotonPlayer player);

        public static void TimeIsUp()
        {
            OnTimeIsUp();
        }

        public static void MyPlayerDied()
        {
            OnMyPlayerDied();
        }

        public static void MyPlayerJoined()
        {
            OnMyPlayerJoined();
        }

        public static void OtherPlayerJoined(PhotonPlayer player)
        {
            OnOtherPlayerJoined(player);
        }

        public static void OtherPlayerDown(PhotonPlayer player)
        {
            OnOtherPlayerDown(player);
        }

        public static void OtherPlayerRevived(PhotonPlayer player)
        {
            OnOtherPlayerRevived(player);
        }

        public static void MyPlayerDown()
        {
            OnMyPlayerDown();
        }

        public static void MyPlayerRevived()
        {
            OnMyPlayerRevived();
        }

        static GameState()
        {
            AddBasicListeners();
        }

        private static void AddBasicListeners ()
        {
            // these local listeners ensure that at least one listener exists for every event (so there's no need to check for nulls)
            OnMyPlayerJoined += delegate() { };
            OnOtherPlayerJoined += delegate(PhotonPlayer player) { };
            OnLevelOver += delegate() { };
            OnOtherPlayerDown += delegate(PhotonPlayer player) { };
            OnOtherPlayerRevived += delegate(PhotonPlayer player) { };
            OnMyPlayerDown += delegate() { };
            OnMyPlayerRevived += delegate() { };
            // current level always ends the level when player dies or timer runs out. maybe later on there will be different levels..
            OnMyPlayerDied += delegate() { OnLevelOver(); };
            OnTimeIsUp += delegate() { OnLevelOver(); };
        }

        public static void ResetListeners ()
        {
            OnMyPlayerJoined = null;
            OnMyPlayerDied = null;
            OnOtherPlayerJoined = null;
            OnTimeIsUp = null;
            OnLevelOver = null;
            OnOtherPlayerDown = null;
            OnOtherPlayerRevived = null;
            OnMyPlayerDown = null;
            OnMyPlayerRevived = null;

            AddBasicListeners();
        }
    }
}
