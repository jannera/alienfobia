using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class SimpleGOGenerator : MonoBehaviour
    {
        public GameObject toBeGenerated;
        public float spawnRange = 20;
        public float playerSafetyDistance = 10f;
        public float secsBetweenSpawn = 10f;
        public float secsBeforeInitialSpawn = 3f;
        public float minAmount = 1f;
        public float maxAmount = 1f;

        private int currentSpawn = 0;

        private float spawnTimer = 0.0f;

        private const int maxSpawnTries = 1000;

        // Use this for initialization
        void Start()
        {
            spawnTimer = secsBeforeInitialSpawn;
        }

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

            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0)
            {
                int count = GetCount();
                GenerateGameObjects(count);
                spawnTimer += secsBetweenSpawn;
            }
        }

        private int GetCount()
        {
            return (int)Random.Range(minAmount, maxAmount);
        }

        private void GenerateGameObjects(int count)
        {
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
                // couldn't find a possible location
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