using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {

	/*Class responsible for spawning the enemy waves as well as keeping track of the actual wave information.
	 * After wave 2 it increases the proability of modified enemies spawning by increasing the powerEnemyChance.
	 * After 5 waves the game will end with the gameStatus variable set to true. */

	public GameObject baseZombie;
	public GameObject[] powerZombies;

	public GameObject baseSoldier;
	public GameObject[] powerSoldiers;

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
				if (waveNumber == 5) {
					gameStatus.gameWon = true;
				} else {
					if (gameStatus.playerNoDamageStreak) {
						gameStatus.modifierCount += 1;
					}

					spawnCheck = new int[enemySpawns.Length];
					waveNumber = waveNumber + 1;
					if (waveNumber >= 3) {
						powerEnemyChance += 15.0f;
					}
					spawnWaves ();
				}
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
		if (waveNumber == 1 || waveNumber == 2) {
			multiplier = 2;
		} else if (waveNumber < 5) {
			multiplier = waveNumber;
		} else {
			multiplier = 4;
		}

		enemyCount = 5 * multiplier;

		int i = 0;
		while (i < enemyCount){
			int rSpawn = Random.Range (0, 25);
			if (spawnCheck [rSpawn] == 0 && distanceToPlayer(enemySpawns[rSpawn].transform) > 50.0f) {
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

	//Function responsible for choosing the enemy (soldier or zombie) the player will face.
	//Also checks to see if it is a modified version that will be spawned from the powerEnemyChance.

	private void chooseEnemy(){

		if (waveNumber == 1) {
			if (Random.value < powerEnemyChance) {
				enemy = powerZombies [Random.Range (0, powerZombies.Length)];
			} else {
				enemy = baseZombie;
			}
		} else if (waveNumber == 2){
			if (Random.value < powerEnemyChance) {
				enemy = powerSoldiers[Random.Range (0, powerSoldiers.Length)];
			} else {
				enemy = baseSoldier;
			}
		} else {
			if (Random.value < powerEnemyChance) {
				if (Random.value < 0.5f) {
					enemy = powerZombies [Random.Range (0, powerZombies.Length)];
				} else {
					enemy = powerSoldiers[Random.Range (0, powerSoldiers.Length)];
				}
			} else {
				if (Random.value < 0.5f) {
					enemy = baseZombie;
				} else {
					enemy = baseSoldier;
				}
			}
		}

	}

}
