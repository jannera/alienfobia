﻿using System;
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
                if (go.GetComponent<PhotonView>().isMine)
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

        public static GameObject GetClosestPlayer(Vector3 pos)
        {
            GameObject closest = null;
            float closestDst = float.MaxValue;
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (closest == null)
                {
                    closest = go;
                    closestDst = Vector2.Distance(closest.transform.position, pos);
                }
                else {
                    float dst = Vector2.Distance(go.transform.position, pos);
                    if (dst < closestDst) {
                        closest = go;
                        closestDst = dst;
                    }
                }
            }
            return closest;
        }

        public static GameObject GetClosestPlayerAlive(Vector3 pos)
        {
            GameObject closest = null;
            float closestDst = float.MaxValue;
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                PlayerHealth playerHealth = go.GetComponent<PlayerHealth>();
                if (playerHealth == null)
                {
                    continue;
                }
                if (playerHealth.isDead || playerHealth.isDowned)
                {
                    continue;
                }
                if (closest == null)
                {
                    closest = go;
                    closestDst = Vector2.Distance(closest.transform.position, pos);
                }
                else
                {
                    float dst = Vector2.Distance(go.transform.position, pos);
                    if (dst < closestDst)
                    {
                        closest = go;
                        closestDst = dst;
                    }
                }
            }
            return closest;
        }

        public static bool AreAnyPlayersAlive()
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                PlayerHealth health = go.GetComponent<PlayerHealth>();
                if (!health.isDead && !health.isDowned)
                {
                    return true;
                }
            }
            return false;
        }

        public static GameObject GetHumanPlayer()
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (go.GetComponent<NPCMovement>() == null)
                {
                    return go;
                }
            }
            return null;
        }

        public static bool IsNPCClient()
        {
            return Application.dataPath.Contains("alienfobia_npc");
        }
    }
}
