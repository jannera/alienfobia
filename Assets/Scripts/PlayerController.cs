using UnityEngine;
using System.Collections;

public class PlayerController : Photon.MonoBehaviour
{
    public float speed = 10f;
    private bool isMine;

    private double syncTimestamp;
    private Vector3 syncPosition;
    private Vector3 syncVelocity;

    void Update()
    {
        if (isMine)
        {
            InputColorChange();
        }
        
        if (!PhotonNetwork.isMasterClient)
        {
            SyncedMovement();
        }
    }

    void Start()
    {
        int ownerId = (int)photonView.instantiationData[0];
        isMine =  ownerId == PhotonNetwork.player.ID;

        Debug.Log("Created " + photonView.viewID);

        syncTimestamp = double.NaN;

        if (!PhotonNetwork.isMasterClient)
        {
            Destroy(this.GetComponent<Rigidbody>());
            Destroy(this.GetComponent<ServerPlayerController>());
        }
        else
        {
            Destroy(this.GetComponent<ServerPlayerController>());
        }
    }

    void FixedUpdate()
    {
        if (isMine)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            if (moveVertical == 0 && moveHorizontal == 0)
            {
                return;
            }

            object[] p = { moveHorizontal, moveVertical };
            photonView.RPC("InputBasedMovement", PhotonTargets.MasterClient, p);
        }
    }

    [RPC]
    void InputBasedMovement(float moveHorizontal, float moveVertical)
    {
        Debug.Log("Applying force on " + photonView.viewID);
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        rigidbody.AddForce(movement * speed * Time.deltaTime);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            if (!PhotonNetwork.isMasterClient)
            {
                return;
            }
            // only server sends
            // tell everyone where stuff 
            stream.SendNext(rigidbody.position);
            stream.SendNext(rigidbody.velocity);
        }
        else
        {
            // only client receives
            if (!PhotonNetwork.isMasterClient)
            {
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
            syncVelocity.magnitude * Time.deltaTime);

        // todo teleport check

        transform.position = newPosition;
    }

    private void InputColorChange()
    {
        if (Input.GetKeyDown(KeyCode.R) && isMine)
        {
            Debug.Log("trying to change color");
            Vector3 color = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            photonView.RPC("RequestColorChange", PhotonTargets.MasterClient, color);
        }
    }

    [RPC]
    void ChangeColorTo(Vector3 color)
    {
        renderer.material.color = new Color(color.x, color.y, color.z, 1f);
    }
}