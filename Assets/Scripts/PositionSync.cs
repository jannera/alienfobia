﻿using UnityEngine;
using System.Collections;

/**
 * Syncs physics objects between master client that owns
 * them and clients that do not.
 * 
 * On client end, estimates positions of objects based
 * on their earlier position, velocity and time since update.
 **/
public class PositionSync : Photon.MonoBehaviour
{
    public bool isMine;
    public float movementEpsilon = 1; // velocity magnitudes above this are considering "moving" (for animations etc)

    void Awake()
    {
        int ownerId = (int)photonView.instantiationData[0];
        isMine = ownerId == PhotonNetwork.player.ID;
        syncTimestamp = double.NaN;
        maxVelSqr = maximumVelocity * maximumVelocity;

        if (!PhotonNetwork.isMasterClient)
        {
            Destroy(this.GetComponent<Rigidbody>());
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // sending is only called for the owner, i.e. server
            stream.SendNext(rigidbody.position);
            stream.SendNext(rigidbody.velocity);
        }
        else
        {
            // reading is only called for non-owners, i.e. clients
            syncPosition = (Vector3)stream.ReceiveNext();
            syncVelocity = (Vector3)stream.ReceiveNext();

            if (double.IsNaN(syncTimestamp))
            {
                // this is the first time we're updating position, so just directly set it
                transform.position = syncPosition;
            }
            syncTimestamp = info.timestamp;
        }
        
    }

    // SERVER specific part
    // TODO maximum velocity limiting could be moved to it's own component
    public float speed = 3000;
    public float maximumVelocity = 4;
    private float maxVelSqr;

    void FixedUpdate()
    {
        if (PhotonNetwork.isMasterClient)
        {
            // limit the maximum velocities
            if (rigidbody.velocity.sqrMagnitude > maxVelSqr)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * maximumVelocity;
            }
        }
    }

    // TODO: this is probably not the right class for this function
    [RPC]
    void Move(Vector3 movement)
    {
        rigidbody.AddForce(movement * speed * Time.deltaTime);
    }

    // CLIENT specific part
    private double syncTimestamp;
    private Vector3 syncPosition; // latest position master sent client
    private Vector3 syncVelocity; // latest velocity master sent client
    public float maxVelocityStepInSecond = 5;

    void Update()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            // only clients estimate positions
            SyncedMovement();
        }
        
    }

    private void SyncedMovement()
    {
        if (double.IsNaN(syncTimestamp))
        {
            return; // still haven't received data from the server
        }
        float pingInSeconds = (float)PhotonNetwork.GetPing() * 0.001f;
        float timeSinceLastUpdate = (float)(PhotonNetwork.time - syncTimestamp);
        float totalTimePassed = pingInSeconds + timeSinceLastUpdate;

        Vector3 extrapolatedTargetPosition = syncPosition + syncVelocity * totalTimePassed;

        Vector3 newPosition = Vector3.MoveTowards(transform.position, extrapolatedTargetPosition,
            maxVelocityStepInSecond * Time.deltaTime);

        // Debug.Log(transform.position + " -> " + newPosition);
        if ((newPosition - extrapolatedTargetPosition).magnitude > 0.5f)
        {
            // if position has fallen too far out of sync, just teleport
            transform.position = extrapolatedTargetPosition;
            Debug.Log("teleported " + gameObject);
        }
        else
        {
            transform.position = newPosition;
        }
    }

    public bool IsMoving()
    {
        Vector3 vel;
        if (PhotonNetwork.isMasterClient)
        {
            vel = rigidbody.velocity;
        }
        else
        {
            vel = syncVelocity;
        }
        Debug.Log(vel.magnitude);
        return vel.magnitude > movementEpsilon;
    }
}