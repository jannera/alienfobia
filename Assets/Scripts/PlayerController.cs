using UnityEngine;
using System.Collections;

public class PlayerController : Photon.MonoBehaviour
{
    private bool isMine;
    public ClientPlayerController clientController;
    public ServerPlayerController serverController;

    public float speed = 3000;

    void Update()
    {
        
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

        if (!isMine)
        {
            Destroy(this.GetComponent<PlayerInputController>());
        }
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

    [RPC]
    void ChangeColorTo(Vector3 color)
    {
        renderer.material.color = new Color(color.x, color.y, color.z, 1f);
    }

    [RPC]
    void ApplyForce(float moveHorizontal, float moveVertical)
    {
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        rigidbody.AddForce(movement * speed * Time.deltaTime);
    }
}