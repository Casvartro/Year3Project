using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePlayer : Leaf {

	//Node responsible for handling and triggering the melee attack as well as setting the animation 
	//state to attack or walk.

	private Vector3 playerPosition;
	private float angle = 10;

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
		if (enemyContext.enemyAnimation.GetCurrentAnimatorStateInfo (0).IsName ("idle")) {
			enemyContext.enemyAnimation.Play ("attack");
		}
		enemyContext.enemyPhysics.enemyMovement (playerPosition, angle);

		return BehaviourStatus.RUNNING;
	

	}

	public override void OnReset(){
		playerPosition = new Vector3(0, 0, 0);
	}
}
