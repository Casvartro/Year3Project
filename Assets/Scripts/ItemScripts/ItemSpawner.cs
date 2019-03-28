using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpawner : MonoBehaviour {

	public GameObject healthCratePrefab;
	public GameObject ammoCratePrefab;
	public GameObject[] powerUpPrefabs;

	public int powerUpLimit = 1;
	public float powerChance = 0.15f;
	public Toggle powerEnable;

	private GameObject[] itemSpawnArray;
	private List<GameObject> itemObjectList;
	 
	// Use this for initialization
	void Start () {
		itemSpawnArray = GameObject.FindGameObjectsWithTag("Item");
		itemObjectList = new List<GameObject>();
		InvokeRepeating ("spawnItems", 5.0f, 20.0f);
	}

	void Update(){
		if (powerEnable.isOn) {
			powerChance = 0.99f;
		} else {
			powerChance = 0.15f;
		}
	}

	private void spawnItems(){
		//Function responsible for randomly spawning items at the designated spawns.

		GameObject item;
		Transform itemSpawnLocation;
		bool isPowerSpawned = false;

		if (itemObjectList.Count != 0) {
			foreach (GameObject itemObject in itemObjectList){
				Destroy (itemObject);
			}
			itemObjectList.Clear ();
		}
	
		foreach (GameObject itemSpawn in itemSpawnArray) {

			itemSpawnLocation = itemSpawn.transform;
			float spawnChance = Random.value;

			if (!isPowerSpawned && spawnChance < powerChance) {
				isPowerSpawned = true;
				item = Instantiate (powerUpPrefabs [Random.Range (0, powerUpPrefabs.Length)],
					itemSpawnLocation.position, itemSpawnLocation.rotation);
			} else {
				spawnChance = Random.value;
				if (spawnChance < 0.5f) {
					item = Instantiate (healthCratePrefab, 
						itemSpawnLocation.position, itemSpawnLocation.rotation);
				
				} else {
					item = Instantiate (ammoCratePrefab, 
						itemSpawnLocation.position, itemSpawnLocation.rotation);
				}
			}

			itemObjectList.Add (item);
		}
	}

}
