using UnityEngine;
using System.Collections;

public class ServerGOGenerator : MonoBehaviour {
    public float chancePerFrame = 0.05f;
    public GameObject toBeGenerated;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (!PhotonNetwork.inRoom)
        {
            return;
        }
        
        if (!PhotonNetwork.isMasterClient)
        {
            Destroy(this);
            return;
        }

        if (Random.Range(0, 1f) < chancePerFrame)
        {
            GenerateGameObject();
        }
	}

    private void GenerateGameObject()
    {
        object[] p = { PhotonNetwork.player.ID };
        PhotonNetwork.InstantiateSceneObject(toBeGenerated.name, Vector3.up * 5, Quaternion.identity, 0, p);
    }
}
