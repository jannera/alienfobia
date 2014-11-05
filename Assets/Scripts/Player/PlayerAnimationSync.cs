using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class PlayerAnimationSync : PhotonBehaviour
    {
        private PlayerMovement playerMovement;
        private PositionSync positionSync;

        void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            positionSync = GetComponent<PositionSync>();
        }

        // Use this for initialization
        void Start()
        {
            if (photonView.isMine)
            {
                // don't sync your own player's animations
                Destroy(this);
                return;
            }
        }

        // Update is called once per frame
        void Update()
        {
            playerMovement.Animating(positionSync.IsMoving());
        }
    }
}