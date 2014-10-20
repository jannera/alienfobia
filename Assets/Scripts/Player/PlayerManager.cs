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
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                GameObject go = players[i];
                if (go.GetComponent<PositionController>().isMine)
                {
                    return go;
                }
            }
            return null;
        }
    }
}
