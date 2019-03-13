using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour {

	//Class responsible for inflicting damage to the player when the weapons bullet collides.

	public int defaultDamage = 20;

	private int bulletDamage;
	private PlayerController player;

	void Start(){
		bulletDamage = defaultDamage;
	}


	void OnCollisionEnter(Collision col){


		if (col.gameObject.tag == "Player") {
			PlayerController player = col.gameObject.GetComponent<PlayerController> ();
			player.takeDamage (bulletDamage);
		}
		Destroy (gameObject);
	}

}
