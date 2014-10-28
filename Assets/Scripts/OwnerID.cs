using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    // simply stores the id of the real player of the gameobject
    public class OwnerID : MonoBehaviour
    {
        public PhotonView photonView;
        public int ownerID;

        // Use this for initialization
        void Awake()
        {
            ownerID = (int)photonView.instantiationData[0];
        }
    }

}