using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class AcceleratingGOGenerator : MonoBehaviour
    {
        public GameObject toBeGenerated;
        public int spawnRange = 1;
        public int secBetweenWaves = 5;
        public int spawnedPerWave = 5;
        public int wavesPerLevel = 4;
        public int increase = 5;
        public int maxSpawns = 30;
        public float playerSafetyDistance = 10f;

        private int currentWave = 1;
        private int currentSpawns;
        private float elapsedTime = 0.0f;
        private float spawnTimer = 0.0f;

        private const int maxSpawnTries = 1000;

        void Start()
        {
            currentSpawns = spawnedPerWave;
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

            UpdateTimers();

            if (ShouldSpawn())
            {
                UpdateSpawnCounts();
                GenerateGameObjects(currentSpawns);
                spawnTimer -= (int)spawnTimer;
            }
        }

        private void UpdateTimers()
        {
            spawnTimer += Time.deltaTime;
            elapsedTime += Time.deltaTime;
        }

        private bool ShouldSpawn()
        {
            return (spawnTimer >= secBetweenWaves);
        }

        private void UpdateSpawnCounts()
        {
            if (currentWave % wavesPerLevel == 0)
            {
                if (currentSpawns < maxSpawns)
                {
                    currentSpawns += increase;
                    Debug.Log("Increasing monster count per wave to " + currentSpawns);
                }
            }
            ++currentWave;
        }

        private void GenerateGameObjects(int count)
        {
            Debug.Log("Spawning wave " + currentWave);
            object[] p = { PhotonNetwork.player.ID };
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i <= count; ++i)
            {
                SpawnWithinRange(p, players);
            }
        }

        private void SpawnWithinRange(object[] playerId, GameObject[] players)
        {
            Vector3 spawnLocation = new Vector3(0f, 0f, 0f);
            int tries = 0;
            do
            {
                spawnLocation.x = transform.position.x + Random.Range(-spawnRange, spawnRange);
                spawnLocation.z = transform.position.z + Random.Range(-spawnRange, spawnRange);
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
}