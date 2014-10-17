using UnityEngine;
using System.Collections;

public class ServerPositionController : MonoBehaviour
{
    public float maximumVelocity = 4;
    private float maxVelSqr;
    public PhotonView photonView;

    public float speed = 3000;

	// Use this for initialization
	void Start () {
        maxVelSqr = maximumVelocity * maximumVelocity;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        // limit the maximum velocities
        if (rigidbody.velocity.sqrMagnitude > maxVelSqr)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maximumVelocity;
        }
    }

    [RPC]
    void RequestColorChange(Vector3 color)
    {
        renderer.material.color = new Color(color.x, color.y, color.z, 1f);

        photonView.RPC("ChangeColorTo", PhotonTargets.OthersBuffered, color);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (!stream.isWriting)
        {
            return; // server only sends
        }
        stream.SendNext(rigidbody.position);
        stream.SendNext(rigidbody.velocity);
    }

    [RPC]
    void ApplyForce(float moveHorizontal, float moveVertical)
    {
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        rigidbody.AddForce(movement * speed * Time.deltaTime);
    }
}
