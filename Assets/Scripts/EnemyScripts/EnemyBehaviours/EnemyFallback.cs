using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFallback : Leaf {

	private Vector3 playerPosition;
	private GameObject closestNode = null;
	private float angle = 180;

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;

		if (enemyContext.enemyInSight ()) {
			playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		}

		if ((!enemyContext.enemyInRange(0.5f) && !enemyContext.enemyTooClose()) || !enemyContext.enemyInSight()) {
			enemyContext.enemyWeaponController.isFiring = false;
			return BehaviourStatus.FAILURE;
		}

		if((!enemyContext.enemyTooClose() && enemyContext.enemyInSight())){
			enemyContext.enemyWeaponController.isFiring = false;
			return BehaviourStatus.SUCCESS;
		}

		/////////////////////////////////////////////////////////////
		if (closestNode == null) {
			closestNode = PathFinder.getRetreatNode (enemyContext.pathNodes, enemyContext.enemyPhysics.transform, playerPosition);
		}

		if (atDestination (enemyContext.enemy.transform, closestNode.transform)) {
			closestNode = PathFinder.getRetreatNeighbor (closestNode, enemyContext.enemy.transform, playerPosition);
		} 

		enemyContext.enemyPhysics.enemyRotation (playerPosition);
		if (enemyContext.enemyAnimation.GetCurrentAnimatorStateInfo (0).IsName ("idle") ||
			enemyContext.enemyAnimation.GetCurrentAnimatorStateInfo (0).IsName ("attack")) {
			enemyContext.enemyAnimation.Play ("walk");
		}
		enemyContext.enemyPhysics.enemyMovement (closestNode.transform.position, angle);

		if (!enemyContext.enemyWeaponController.isFiring) {
			enemyContext.enemyWeaponController.isFiring = true;
		}
		////////////////////////////////////////////////////////////////

		return BehaviourStatus.RUNNING;
	}


	//Checks if the enemy has reached its destination.
	private bool atDestination(Transform currentPosition, Transform destPosition){

		Vector3 direction = destPosition.position - currentPosition.position;
		direction.y = 0;
		if (direction.magnitude < .2f){
			return true;
		}

		return false;

	}

	public override void OnReset(){
		playerPosition = new Vector3(0, 0, 0);
		closestNode = null;
	}

}
