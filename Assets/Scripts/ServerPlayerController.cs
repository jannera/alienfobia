﻿using UnityEngine;
using System.Collections;

public class ServerPlayerController : MonoBehaviour {
    public PhotonView photonView;
    public float maximumVelocity = 4;
    private float maxVelSqr;

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
}
