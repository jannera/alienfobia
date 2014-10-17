using UnityEngine;
using System.Collections;

public class ServerMonsterController : MonoBehaviour {
    public float maxForce = 500;

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

        if (Random.Range(0f, 1f) < 0.01f)
        {
            // 1% chance every frame to
            Vector3 movement = new Vector3(Random.Range(-maxForce, maxForce), Random.Range(-maxForce, maxForce), Random.Range(-maxForce, maxForce));
            rigidbody.AddForce(movement);
        }
	}
}
