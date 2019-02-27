using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePlayer : Leaf {
	
	Vector3 playerPosition;

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;

		if (!enemyContext.enemyInRange(0.5f)) {
			if (enemyContext.enemyAnimation.GetCurrentAnimatorStateInfo (0).IsName ("attack")) {
				enemyContext.enemyAnimation.Play ("walk");
			}
			return BehaviourStatus.FAILURE;
		}

		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		} else {
			return BehaviourStatus.SUCCESS;
		}

		enemyContext.enemyPhysics.enemyRotation (playerPosition);
		if (enemyContext.enemyAnimation.GetCurrentAnimatorStateInfo (0).IsName ("walk")) {
			enemyContext.enemyAnimation.Play ("attack");
		}
		enemyContext.enemyPhysics.enemyMovement (playerPosition);

		return BehaviourStatus.RUNNING;
	

	}

	public override void OnReset(){
		playerPosition = new Vector3(0, 0, 0);
	}
}
