using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour {

	//Class responsible for inflicting damage to the player when the weapons bullet collides.
	//Uses the BulletCollision class for detecting if it has collided with a player if the bullet was too fast.
	//Also checks to see if it has collided with anything that isnt an item or another enemy to destroy itself.

	public int enemyDamage;

	private Vector3 previousPosition;
	private Rigidbody bulletRB;

	private PlayerController player;

	void Start(){
		previousPosition = transform.position;
		bulletRB = this.GetComponent<Rigidbody> ();
	}

	void Update(){
		previousPosition = BulletCollision.bulletCollisionCast (gameObject, transform, previousPosition, bulletRB, enemyDamage, "enemy");
	}

	void OnCollisionEnter(Collision col){

		if (col.gameObject.tag == "Player") {
			PlayerController player = col.gameObject.GetComponent<PlayerController> ();
			player.takeDamage (enemyDamage);
		}

		if(col.gameObject.tag != "healthItem" || col.gameObject.tag != "ammoItem" 
			|| col.gameObject.tag != "powerItem" || col.gameObject.tag != "enemy" ) {
			Destroy (gameObject);
		}

	}
		

}
