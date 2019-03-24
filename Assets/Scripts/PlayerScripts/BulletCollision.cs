using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision {

	public static Vector3 bulletCollisionCast(GameObject bullet, Transform transform, Vector3 previousPosition, Rigidbody bulletRB, int damage, string name){
		RaycastHit hitInfo;
		Vector3 thisPosition = transform.position;
		Vector3 frameDirection = bulletRB.velocity.normalized;
		float frameSize = (thisPosition - previousPosition).magnitude;

		if (frameSize > 0.1) {
			if (Physics.Raycast(previousPosition, frameDirection, out hitInfo, frameSize)) {
				if (name == "enemy") {
					if (hitInfo.collider.tag == "Player") {
						PlayerController player = hitInfo.collider.gameObject.GetComponent<PlayerController> ();
						player.takeDamage (damage);
						GameObject.Destroy (bullet);
					}
				} else if (name == "player") {
					if (hitInfo.collider.tag == "enemy") {
						PlayerController player = GameObject.Find ("Player").GetComponent<PlayerController> ();
						player.addShotHitTarget ();
						EnemyController enemy = hitInfo.collider.gameObject.GetComponent<EnemyController> ();
						enemy.damageTaken (damage);
						GameObject.Destroy (bullet);
					}
				}
			}else {
				previousPosition = thisPosition;
			}
		}

		return previousPosition;
	}
}
