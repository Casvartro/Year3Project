using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	//Class that Controls the properties of the individual bullets.
	//Applies PowerUP modifier if the player has it active to the bullet damage and calls the BulletCollision class to detect
	//if a player or enemy is in collision range.

	public int defaultDamage = 100;

	private int bulletDamage;
	private PlayerController player;
	private Vector3 previousPosition;
	private Rigidbody bulletRB;

	void Start(){
		bulletDamage = defaultDamage;
		previousPosition = transform.position;
		player = GameObject.Find ("Player").GetComponent<PlayerController> ();
		bulletRB = this.GetComponent<Rigidbody> ();
	}

	void Update(){

		if (player.getCurrentPower () == PlayerController.powerUpState.RED) {
			bulletDamage *= player.powerMultiplier; 
		} else {
			bulletDamage = defaultDamage;
		}

		previousPosition = BulletCollision.bulletCollisionCast (gameObject, transform, previousPosition, bulletRB, bulletDamage, "player");

	}
		
	//Base collider detection used as backup as well as for destroying bullet when colliding with a wall or floor.
	void OnCollisionEnter(Collision col){
		
		if (col.gameObject.tag == "enemy") {
			player.addShotHitTarget ();
			EnemyController enemy = col.gameObject.GetComponent<EnemyController> ();
			enemy.damageTaken (bulletDamage);
		}
		Destroy (gameObject);
	}

}
