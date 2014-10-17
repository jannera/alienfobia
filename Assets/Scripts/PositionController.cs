using UnityEngine;
using System.Collections;

public class PositionController : Photon.MonoBehaviour
{
    private bool isMine;
    public ClientPositionController clientController;
    public ServerPositionController serverController;

    void Start()
    {
        int ownerId = (int)photonView.instantiationData[0];
        isMine =  ownerId == PhotonNetwork.player.ID;

        Debug.Log("Created " + photonView.viewID);

        if (!PhotonNetwork.isMasterClient)
        {
            Destroy(this.GetComponent<Rigidbody>());
            Destroy(this.GetComponent<ServerPositionController>());
            serverController = null;
        }
        else
        {
            clientController = null;
            Destroy(this.GetComponent<ClientPositionController>());
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
}