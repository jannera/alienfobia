using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CompleteProject
{
    class PlayerManager
    {
        public static GameObject GetMyPlayer()
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (go.GetComponent<PositionSync>().isMine)
                {
                    return go;
                }
            }
            return null;
        }

        public static T GetComponentFromMyPlayer<T >() where T : Component
        {
            GameObject go = GetMyPlayer();
            if (go == null)
            {
                return null;
            }
            return go.GetComponentInChildren<T>();
        }

        public static GameObject GetPlayerWithOwnerId(int ownerID)
        {

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (go.GetComponent<OwnerID>().ownerID == ownerID)
                {
                    return go;
                }
            }
            return null;
        }
    }
}
