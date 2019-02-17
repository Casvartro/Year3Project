using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {

	public GameObject enemy;
	public Transform[] enemySpawns;
	public Transform playerTransform;

	private int waveNumber = 1;
	private int enemyCount = 0;
	private int[] spawnCheck;

	// Use this for initialization
	void Start () {

		spawnCheck = new int[enemySpawns.Length];
		spawnWaves ();

	}
	
	// Update is called once per frame
	void Update () {
		if(enemyCount == 0){
			spawnCheck = new int[enemySpawns.Length];
			waveNumber = waveNumber + 1;
			spawnWaves ();
		}
	}

	public int getWaveNumber(){
		return waveNumber;
	}

	public int getEnemyCount(){
		return enemyCount;
	}

	public void reduceEnemyCount(){
		enemyCount = enemyCount - 1;
	}

	private void spawnWaves(){

		int multiplier = 0;
		if (waveNumber <= 4) {
			multiplier = waveNumber;
		}
		enemyCount = 5 * multiplier;
		int i = 0;
				
		while (i < enemyCount){
			int rSpawn = Random.Range (0, 25);
			if (spawnCheck [rSpawn] == 0 && distanceToPlayer(enemySpawns[rSpawn].transform) > 75.0f) {
				Instantiate(enemy, enemySpawns[rSpawn].transform.position, enemySpawns[rSpawn].transform.rotation);
				spawnCheck [rSpawn] = 1;
				i = i + 1;
			}
		}
			
	}

	private float distanceToPlayer(Transform enemyTransform){
		return Vector3.Distance (enemyTransform.position, playerTransform.position);
	}
		
}
