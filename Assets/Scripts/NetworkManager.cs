using UnityEngine;
using System.Collections;

public class NetworkManager : Photon.MonoBehaviour
{
		public GameObject playerPrefab;
		public AudioSource menuMusic;
        public AudioSource gameMusic;

		void Awake() {
			menuMusic.loop = true;
			menuMusic.Play();
		}

		void Start ()
		{
				PhotonNetwork.ConnectUsingSettings ("0.1");				
		}

		private const string roomName = "RoomName";
		private RoomInfo[] roomsList;

		void OnGUI ()
		{
				if (!PhotonNetwork.connected) {
						GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
				} else if (PhotonNetwork.room == null) {
						// Create Room
                        if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
                        {
                            PhotonNetwork.CreateRoom(roomName + System.Guid.NewGuid().ToString("N"));
                        }
 
						// Join Room
						if (roomsList != null) {
								for (int i = 0; i < roomsList.Length; i++) {
                                    if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join " + roomsList[i].name))
                                    {
                                        PhotonNetwork.JoinRoom(roomsList[i].name);
                                    }
								}
						}
				}
		}

		void OnReceivedRoomListUpdate ()
		{
				roomsList = PhotonNetwork.GetRoomList ();
		}
		void OnJoinedRoom ()
		{
				Debug.Log ("Connected to Room");
                menuMusic.Stop();
                gameMusic.Play();
				if (PhotonNetwork.isMasterClient) {
						CreatePlayer (PhotonNetwork.player.ID);
				} else {
						// ask the server to create the prefab
						photonView.RPC ("CreatePlayer", PhotonTargets.MasterClient, PhotonNetwork.player.ID);
				}
		}

		[RPC]
		void CreatePlayer (int ownerId)
		{
				object[] p = { ownerId };
				PhotonNetwork.InstantiateSceneObject (playerPrefab.name, Vector3.up * 0.5f, Quaternion.identity, 0, p);
		}
}
