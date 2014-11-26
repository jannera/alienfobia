using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CompleteProject
{
    class GrenadeThrowing : CompleteProject.PhotonBehaviour
    {
        public float timeBetweenGrenades = 0.8f;
        float grenadeTimer;
        public GameObject grenadePreFab = null;

        public int grenades = 3;

        void Awake()
        {
            grenadeTimer = timeBetweenGrenades;

            if (!photonView.isMine)
            {
                Destroy(this);
            }
        }

        void Update()
        {
            grenadeTimer += Time.deltaTime;
        }

        public void ThrowGrenade()
        {
            grenadeTimer = 0f;
            grenades--;

            object[] p = { transform.rotation.eulerAngles.y };
            PhotonNetwork.Instantiate(grenadePreFab.name, transform.position, Quaternion.identity, 0, p);
        }

        public bool CanThrowGrenade()
        {
            return grenadeTimer >= timeBetweenGrenades && grenades > 0;
        }
    }
}
