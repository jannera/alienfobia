using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class ServerGOGenerator : MonoBehaviour
    {
        public GameObject toBeGenerated;
        public int spawnRange = 1;
        public int secBetweenWaves = 5;
        public int spawnedPerWave = 5;
        public int wavesPerLevel = 4;
        public int increase = 5;
        public int maxSpawns = 30;

        private int currentWave = 1;
        private int currentSpawns;
        private float elapsedTime = 0.0f;
        private float spawnTimer = 0.0f;

        // Use this for initialization
        void Start()
        {
            currentSpawns = spawnedPerWave;
        }

        // Update is called once per frame
        void Update()
        {
            if (!PhotonNetwork.inRoom)
            {
                return;
            }

            if (!PhotonNetwork.isMasterClient)
            {
                Destroy(this); // removes this component
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
                if (currentWave < maxSpawns)
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
            for (int i = 0; i <= count; ++i)
            {
                SpawnWithinRange(p);
            }
        }

        private void SpawnWithinRange(object[] playerId)
        {
            Vector3 spawnLocation = new Vector3(0f, 0f, 0f);
            spawnLocation.x = transform.position.x + Random.Range(-spawnRange, spawnRange);
            spawnLocation.z = transform.position.z + Random.Range(-spawnRange, spawnRange);

            PhotonNetwork.InstantiateSceneObject(toBeGenerated.name, spawnLocation, Quaternion.identity, 0, playerId);
        }
    }
}