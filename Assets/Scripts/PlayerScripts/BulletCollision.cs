using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour {

	public int bulletDamage = 100;

	void OnCollisionEnter(Collision col){
		Destroy (gameObject);
		if (col.gameObject.tag == "enemy") {
			EnemyController enemy = col.gameObject.GetComponent<EnemyController> ();
			enemy.damageTaken (bulletDamage);
		}
	}

}
