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
        public static event DOtherPlayerDown OnOtherPlayerDown;
        public static event DOtherPlayerRevived OnOtherPlayerRevived;
        public static event DMyPlayerDown OnMyPlayerDown;
        public static event DMyPlayerRevived OnMyPlayerRevived;

        public delegate void DMyPlayerJoined();
        public delegate void DMyPlayerDied();
        public delegate void DOtherPlayerJoined(PhotonPlayer player);
        public delegate void DLevelOver();
        public delegate void DTimeIsUp();
        public delegate void DOtherPlayerDown(PhotonPlayer player);
        public delegate void DMyPlayerDown();
        public delegate void DOtherPlayerRevived(PhotonPlayer player);
        public delegate void DMyPlayerRevived();

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

        
    }
}
