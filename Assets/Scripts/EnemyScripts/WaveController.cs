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

	//Returns the wave number.
	public int getWaveNumber(){
		return waveNumber;
	}

	//Returns the number of enemnies remaining.
	public int getEnemyCount(){
		return enemyCount;
	}

	//Reduces the enemy count when an enemy is destroyed.
	public void reduceEnemyCount(){
		enemyCount = enemyCount - 1;
	}

	//In charge of spawning the enemies every wave.
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

	//Returns the distance from the spawn point to the player to prevent spawning too close.
	private float distanceToPlayer(Transform enemyTransform){
		return Vector3.Distance (enemyTransform.position, playerTransform.position);
	}
		
}
