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

        private bool playerExists = false;

        void Awake()
        {
            menuMusic.loop = true;
            menuMusic.Play();
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                // if a player already exists in scene, go with the offline mode
                PhotonNetwork.offlineMode = true;
                playerExists = true;
            }
        }

        void Start()
        {
            if (!PhotonNetwork.offlineMode)
            {
                PhotonNetwork.ConnectUsingSettings("0.1");
            }
            else
            {
                PhotonNetwork.CreateRoom(roomName);
            }
            
            
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

            if (!playerExists)
            {
                PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero,
                Quaternion.identity, 0, new object[] { PhotonNetwork.player.ID });
            }
        }

        void SetPlayerName()
        {
            string name;
            if (automaticGameStarting)
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