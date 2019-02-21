using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	public int defaultDamage = 100;

	private int bulletDamage;
	private PlayerController player;

	void Start(){
		bulletDamage = defaultDamage;
		player = GameObject.Find ("Player").GetComponent<PlayerController> ();
	}
		

	void OnCollisionEnter(Collision col){

		if (player.getCurrentPower () == PlayerController.powerUpState.RED) {
			bulletDamage *= player.powerMultiplier; 
		} else {
			bulletDamage = defaultDamage;
		}

		if (col.gameObject.tag == "enemy") {
			EnemyController enemy = col.gameObject.GetComponent<EnemyController> ();
			enemy.damageTaken (bulletDamage);
			Debug.Log ("Bullet Damage: " + bulletDamage);
		}
		Destroy (gameObject);
	}

}
