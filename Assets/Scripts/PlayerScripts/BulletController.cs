using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	//Class responsible for inflicting damage to the enemies when the weapons bullet collides.

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
		

	void OnCollisionEnter(Collision col){
		
		if (col.gameObject.tag == "enemy") {
			player.addShotHitTarget ();
			EnemyController enemy = col.gameObject.GetComponent<EnemyController> ();
			enemy.damageTaken (bulletDamage);
		}
		Destroy (gameObject);
	}

}
