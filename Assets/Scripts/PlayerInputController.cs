using UnityEngine;
using System.Collections;

public class PlayerInputController : MonoBehaviour {
    public PhotonView photonView;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        InputColorChange();
	}

    private void InputColorChange()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("trying to change color");
            Vector3 color = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            photonView.RPC("RequestColorChange", PhotonTargets.MasterClient, color);
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (moveVertical == 0 && moveHorizontal == 0)
        {
            return;
        }

        object[] p = { moveHorizontal, moveVertical };
        photonView.RPC("ApplyForce", PhotonTargets.MasterClient, p);
    }
}
