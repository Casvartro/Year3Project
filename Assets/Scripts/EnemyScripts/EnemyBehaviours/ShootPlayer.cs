using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlayer : Leaf {

	/* Leaf node responsible for shooting the player and setting the enemy weapon state of firing to true.
	 * Fails if the player is out of range, out of sight or too close. Succeeds if the player is killed. */

	private Vector3 playerPosition;

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;


		if (!enemyContext.enemyInRange(0.5f) || !enemyContext.enemyInSight()  || enemyContext.enemyTooClose()) {
			if (enemyContext.enemyAnimation.GetCurrentAnimatorStateInfo (0).IsName ("attack")) {
				enemyContext.enemyAnimation.Play ("idle");
			}
			enemyContext.enemyWeaponController.isFiring = false;
			return BehaviourStatus.FAILURE;
		}

		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		} else {
			enemyContext.enemyWeaponController.isFiring = false;
			return BehaviourStatus.SUCCESS;
		}

		enemyContext.enemyPhysics.enemyRotation (playerPosition);
		if (enemyContext.enemyAnimation.GetCurrentAnimatorStateInfo (0).IsName ("idle") || 
			enemyContext.enemyAnimation.GetCurrentAnimatorStateInfo (0).IsName ("walk") ) {
			enemyContext.enemyAnimation.Play ("attack");
		}

		if (!enemyContext.enemyWeaponController.isFiring) {
			enemyContext.enemyWeaponController.isFiring = true;
		}

		return BehaviourStatus.RUNNING;
	}
		


	public override void OnReset(){
		playerPosition = new Vector3(0, 0, 0);
	}
}
