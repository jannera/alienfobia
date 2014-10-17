using UnityEngine;
using System.Collections;

public class PlayerController : Photon.MonoBehaviour
{
    public float speed = 10f;
    private bool isMine;
    public ClientPlayerController clientController;
    public ServerPlayerController serverController;

    void Update()
    {
        if (isMine)
        {
            InputColorChange();
        }
    }

    void Start()
    {
        int ownerId = (int)photonView.instantiationData[0];
        isMine =  ownerId == PhotonNetwork.player.ID;

        Debug.Log("Created " + photonView.viewID);

        if (!PhotonNetwork.isMasterClient)
        {
            // Destroy(this.GetComponent<Rigidbody>());
            Destroy(this.GetComponent<ServerPlayerController>());
            serverController = null;
        }
        else
        {
            clientController = null;
            Destroy(this.GetComponent<ClientPlayerController>());
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
        // Debug.Log("Applying force on " + photonView.viewID);
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        rigidbody.AddForce(movement * speed * Time.deltaTime);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (clientController != null)
        {
            clientController.OnPhotonSerializeView(stream, info);
        }
        if (serverController != null)
        {
            serverController.OnPhotonSerializeView(stream, info);
        }
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