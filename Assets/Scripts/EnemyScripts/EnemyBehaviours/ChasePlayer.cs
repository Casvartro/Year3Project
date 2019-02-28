using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : Leaf {

	//Node that chases the player until it is right next to it for a melee attack.

	Vector3 playerPosition;

	public override BehaviourStatus OnBehave(BehaviourState state){
		
		BehaviourContext enemyContext = (BehaviourContext)state;

		if (enemyContext.enemySight.getPlayerSeen ()) {

			playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;

		}
		if(!enemyContext.enemyInSight() || enemyContext.enemyInRange(0.5f)){
			return BehaviourStatus.FAILURE;
		}
			
		if (atPlayer (enemyContext.enemy.transform, playerPosition, enemyContext.enemyRange)) {
			return BehaviourStatus.SUCCESS;
		}

		enemyContext.enemyPhysics.enemyRotation (playerPosition);
		if (enemyContext.enemyAnimation.GetCurrentAnimatorStateInfo (0).IsName ("idle")) {
			enemyContext.enemyAnimation.Play ("walk");
		}
		enemyContext.enemyPhysics.enemyMovement (playerPosition);
 		
		return BehaviourStatus.RUNNING;
	}

	//Checks if the character has reached its destination.
	private bool atPlayer(Transform currentPosition, Vector3 destPosition, float range){

		float distance = Vector3.Distance (destPosition, currentPosition.position);
		if (distance < range ){
			return true;
		}
		return false;

	}

	public override void OnReset(){
		playerPosition = new Vector3(0, 0, 0);
	}
}
