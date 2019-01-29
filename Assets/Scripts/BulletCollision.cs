using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour {

	public int bulletDamage = 100;

	void OnCollisionEnter(Collision col){
		Destroy (gameObject);
	}

	void OnTriggerEnter(Collider hit){
		if (hit.gameObject.tag == "enemy") {
			EnemyController enemy = hit.gameObject.GetComponent<EnemyController> ();
			enemy.damageTaken (bulletDamage);
		}
	}

}
