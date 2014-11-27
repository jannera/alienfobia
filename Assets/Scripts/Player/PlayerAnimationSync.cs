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

        void Start()
        {
            if (photonView.isMine)
            {
                // don't sync your own player's animations
                Destroy(this);
                return;
            }
        }

        void Update()
        {
            playerMovement.Animating(positionSync.IsMoving());
        }
    }
}