using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {

	public GameObject baseZombie;
	public GameObject[] powerZombies;
	public Transform[] enemySpawns;
	public Transform playerTransform;
	public int waveNumber = 1;
	public float powerEnemyChance = 0.15f;
	public bool wavesOn = true;

	private GameObject enemy;
	private int enemyCount = 0;
	private int[] spawnCheck;
	private GameController gameStatus;

	// Use this for initialization
	void Start () {

		gameStatus = GameObject.Find ("UICanvas").GetComponent<GameController> ();

		spawnCheck = new int[enemySpawns.Length];
		if (wavesOn) {
			spawnWaves ();
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (wavesOn) {
			if (enemyCount == 0) {

				if (gameStatus.playerNoDamageStreak) {
					gameStatus.modifierCount += 1;
				}

				spawnCheck = new int[enemySpawns.Length];
				waveNumber = waveNumber + 1;
				powerEnemyChance += 0.05f;
				spawnWaves ();
			}
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
				chooseEnemy ();
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

	private void chooseEnemy(){

		if (waveNumber == 1) {
			enemy = baseZombie;
		} else {
			if (Random.value < powerEnemyChance) {
				enemy = powerZombies [Random.Range (0, powerZombies.Length)];
			} else {
				enemy = baseZombie;
			}
		}

	}

}
