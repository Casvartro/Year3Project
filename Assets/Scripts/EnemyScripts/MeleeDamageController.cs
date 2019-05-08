using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamageController : MonoBehaviour {

	//Class responsible for keeping track of collisions with the enemy's melee weapon/hands.
	//Inflicts damage to the player when collisions occur.

	private EnemyController enemy;
	private PlayerController player;
	private int meleeDamage;

	// Use this for initialization
	void Start () {
		enemy = this.transform.root.gameObject.GetComponent<EnemyController> ();	
		player = GameObject.Find ("Player").GetComponent<PlayerController> ();
		meleeDamage = enemy.enemyDamage;
	}
	
	void OnTriggerEnter(Collider col){

		if (col.gameObject.CompareTag ("Player")) {
			player.takeDamage (meleeDamage);
		} 

	}

}
