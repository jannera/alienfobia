using UnityEngine;
using System.Collections;

public class ServerGOGenerator : MonoBehaviour
{		
		public GameObject toBeGenerated;		
		public int spawnRange = 1;
		public int secBetweenWaves = 5;
		public int spawnedPerWave = 10;
		public int maxwaves = 10;
		
		private float currentTime = 0.0f;
		private int spawnedWaves = 0;

		// Use this for initialization
		void Start ()
		{
        
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (!PhotonNetwork.inRoom) {
						return;
				}
        
				if (!PhotonNetwork.isMasterClient) {
						Destroy (this);
						return;
				}

				currentTime += Time.deltaTime;
				Debug.Log("Current time " + currentTime) ;
				
				if (currentTime >= secBetweenWaves) {
					GenerateGameObjects(spawnedPerWave);	
					currentTime -= (int)currentTime;								
				}
		}
	
		private void GenerateGameObjects (int count)
		{
			if (spawnedWaves <= maxwaves) {
				Debug.Log("Spawning wave " + spawnedWaves) ;
				object[] p = { PhotonNetwork.player.ID };
				for (int i = 0; i <= count; ++i) {
					SpawnWithinRange(p);
				}
				++spawnedWaves;	
			} else {
				Debug.Log("No more spawns");
			}
		}

		private void SpawnWithinRange(object[] playerId) {
			Vector3 spawnLocation = new Vector3(0f, 0f, 0f);			
			spawnLocation.x = transform.position.x + Random.Range(-spawnRange, spawnRange);
			spawnLocation.z = transform.position.z + Random.Range(-spawnRange, spawnRange);
			
			PhotonNetwork.InstantiateSceneObject (toBeGenerated.name, spawnLocation, Quaternion.identity, 0, playerId);							
		}
}
