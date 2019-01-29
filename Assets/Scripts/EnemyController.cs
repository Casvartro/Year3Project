using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public int enemyScore = 100;
	private int enemyHealth = 100;
	private WaveController waveController;
	private ScoreController scoreController;

	// Use this for initialization
	void Awake () {
		waveController = GameObject.FindObjectOfType<WaveController> ();
		scoreController = GameObject.FindObjectOfType<ScoreController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (enemyHealth <= 0) {
			waveController.reduceEnemyCount ();
			scoreController.setScoreText (enemyScore);
			Destroy (this.gameObject);
		}
	}

	public void damageTaken(int damage){
		enemyHealth = enemyHealth - damage;
	}

}
