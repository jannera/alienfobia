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

        void Awake()
        {
            menuMusic.loop = true;
            menuMusic.Play();
        }

        void Start()
        {
            PhotonNetwork.ConnectUsingSettings("0.1");
            roomName = System.Environment.UserName + "@" + System.Environment.MachineName;
        }

        private string roomName;
        private RoomInfo[] roomsList;

        void OnGUI()
        {
            if (!PhotonNetwork.connected)
            {
                GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
            }
            else if (PhotonNetwork.room == null)
            {
                if (automaticGameStarting)
                {
                    return;
                }
                // Create Room
                if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
                {
                    PhotonNetwork.CreateRoom(roomName);
                }

                // Join Room
                if (roomsList != null)
                {
                    for (int i = 0; i < roomsList.Length; i++)
                    {
                        if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join " + roomsList[i].name))
                        {
                            PhotonNetwork.JoinRoom(roomsList[i].name);
                        }
                    }
                }
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
                Debug.Log("Created new game and joined it as master client");
                PhotonNetwork.CreateRoom(roomName);
            }

        }
        void OnJoinedRoom()
        {
            Debug.Log("Connected to Room");
            menuMusic.Stop();
            gameMusic.Play();
            if (PhotonNetwork.isMasterClient)
            {
                CreatePlayer(PhotonNetwork.player.ID);
            }
            else
            {
                // ask the server to create the prefab
                RPC<int>(CreatePlayer, PhotonTargets.MasterClient, PhotonNetwork.player.ID);
            }
        }

        [RPC]
        void CreatePlayer(int ownerId)
        {
            object[] p = { ownerId };
            PhotonNetwork.InstantiateSceneObject(playerPrefab.name, Vector3.up * 0.5f, Quaternion.identity, 0, p);
        }
    }
}