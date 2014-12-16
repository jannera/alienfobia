using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class NetworkManager : CompleteProject.PhotonBehaviour
    {
        public GameObject playerPrefab;
        public AudioSource menuMusic;
        public AudioSource gameMusic;
        public bool automaticGameStarting = true; // used for automatically creating a game. if a game already is running on this computer, join it
        public GameObject startMenu;

        void Awake()
        {
            menuMusic.loop = true;
            menuMusic.Play();
        }

        void Start()
        {
            PhotonNetwork.ConnectUsingSettings("0.1");
            
            roomName = System.Environment.UserName + "@" + System.Environment.MachineName;
            SetPlayerName();
        }

        private string roomName;
        private RoomInfo[] roomsList;

        void OnGUI()
        {
            if (!PhotonNetwork.connected)
            {
                GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
            }
        }

        void OnReceivedRoomListUpdate()
        {
            roomsList = PhotonNetwork.GetRoomList();
            if (automaticGameStarting)
            {
                foreach (RoomInfo info in roomsList)
                {
                    if (info.name.Equals(roomName))
                    {
                        PhotonNetwork.JoinRoom(info.name);
                        Debug.Log("Joined existing game as client");
                        return;
                    }
                }

                // there was no running game on this computer to join, so set one up
                Debug.Log("Created new game and joining it as master client");
                PhotonNetwork.CreateRoom(roomName);
            }

        }
        void OnJoinedRoom()
        {
            Debug.Log("Connected to Room");
            if (!startMenu.GetActive() && PhotonNetwork.isMasterClient)
            {
                startMenu.SetActive(true);
            }
        }

        public void StartPlaying()
        {
            RPC(ReallyStartPlaying, PhotonTargets.All);
        }

        [RPC]
        private void ReallyStartPlaying()
        {
            menuMusic.Stop();
            gameMusic.Play();

            PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero,
                Quaternion.identity, 0, new object[] { PhotonNetwork.player.ID });
        }

        void SetPlayerName()
        {
            string name;
            if (automaticGameStarting && Application.isEditor)
            {
                string path = Application.dataPath;
                string[] parts = path.Split('/');
                name = parts[parts.Length - 2];
            }
            else
            {
                name = System.Environment.UserName;
            }
            PhotonNetwork.player.name = name;
        }
    }
}