using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CompleteProject
{
    class GOGenerator : PhotonBehaviour
    {
        public float spawnRange = 5;

        public float secsBetweenSpawns = 20;
        public float secsBeforeFirstSpawn = 3;
        
        public float playerSafetyDistance = 2f;

        public Spawn[] spawns = new Spawn[0];

        private float timer;
        private int[] amounts;

        
        

        private const int maxSpawnTries = 1000;

        public void Awake()
        {
            timer = secsBeforeFirstSpawn;
        }

        void Update()
        {
            if (!PhotonNetwork.inRoom)
            {
                return;
            }

            if (!PhotonNetwork.isMasterClient)
            {
                Destroy(this);
                return;
            }

            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                Spawn();
                timer += secsBetweenSpawns;
            }
        }

        private void Spawn()
        {
            object[] p = { PhotonNetwork.player.ID };
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < spawns.Length; i++)
            {
                int amount = UnityEngine.Random.Range(spawns[i].minAmount, spawns[i].maxAmount + 1);
                for (int j = 0; j < amount; j++)
                {
                    SpawnWithinRange(spawns[i].prefab, p, players);
                }
            }
        }

        private void SpawnWithinRange(GameObject toBeGenerated, object[] playerId, GameObject[] players)
        {
            Vector3 spawnLocation = new Vector3(0f, 0f, 0f);
            int tries = 0;
            do
            {
                spawnLocation.x = transform.position.x + UnityEngine.Random.Range(-spawnRange, spawnRange);
                spawnLocation.z = transform.position.z + UnityEngine.Random.Range(-spawnRange, spawnRange);
                tries++;
            } while (TooCloseToPlayers(spawnLocation, players)
                && tries < maxSpawnTries);

            if (tries == maxSpawnTries)
            {
                Debug.LogWarning("Canceled a spawn because was unable to find a location!");
                return;
            }
            PhotonNetwork.InstantiateSceneObject(toBeGenerated.name, spawnLocation, Quaternion.identity, 0, playerId);
        }

        private bool TooCloseToPlayers(Vector3 pos, GameObject[] players)
        {
            foreach (GameObject go in players)
            {
                if ((go.transform.position - pos).magnitude < playerSafetyDistance)
                {
                    return true;
                }
            }
            return false;
        }


    }

    [Serializable]
    public class Spawn
    {
        public int minAmount, maxAmount;
        public GameObject prefab;
    }
}
