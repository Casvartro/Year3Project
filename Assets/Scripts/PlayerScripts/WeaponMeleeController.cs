using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMeleeController : MonoBehaviour {

	public int defaultMeleeDamage;

	private int meleeDamage;
	private PlayerController player;

	// Use this for initialization
	void Start () {
		meleeDamage = defaultMeleeDamage;
		player = GameObject.Find ("Player").GetComponent<PlayerController> ();

	}
	
	void OnCollisionEnter(Collision col){

		if (player.getCurrentPower () == PlayerController.powerUpState.RED) {
			meleeDamage *= player.powerMultiplier; 
		} else {
			meleeDamage = defaultMeleeDamage;
		}

		if (col.gameObject.tag == "enemy") {
			EnemyController enemy = col.gameObject.GetComponent<EnemyController> ();
			enemy.damageTaken (meleeDamage);
			Debug.Log ("melee Damage: " + meleeDamage);
		}
	
	}


}
