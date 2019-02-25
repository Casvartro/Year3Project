using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : Leaf {

	Vector3 playerPosition;

	public override BehaviourStatus OnBehave(BehaviourState state){
		
		BehaviourContext enemyContext = (BehaviourContext)state;

		if (enemyContext.enemySight.getPlayerSeen ()) {

			playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;

		} else if(!enemyContext.enemySight.getPlayerSeen() 
			&& atPlayer(enemyContext.enemy.transform, playerPosition)){

			return BehaviourStatus.FAILURE;
		
		}
			
		if (atPlayer (enemyContext.enemy.transform, playerPosition)) {
			enemyContext.enemyAnimation.Play ("idle");
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
	private bool atPlayer(Transform currentPosition, Vector3 destPosition){

		Vector3 direction = destPosition - currentPosition.position;
		direction.y = 0;
		if (direction.magnitude < 2f){
			return true;
		}

		return false;

	}

	public override void OnReset(){
		playerPosition = new Vector3(0, 0, 0);
	}
}
