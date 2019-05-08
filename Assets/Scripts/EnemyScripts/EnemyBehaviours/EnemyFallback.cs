using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFallback : Leaf {

	/*Leaf node responsible for allowing the soldier to retreat when the player is too close.
	 * If the player is out of range but in sight it succeeds.
	 * If the player is out of range and out of melee range it fails.
	 * Proceeds to retrieve the closest node furthest away from the player and its proceeding neighbor if it needs to move again.
	 * Shoots while retreating as well. */

	private Vector3 playerPosition;
	private GameObject closestNode = null;
	private float angle = 180;

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;

		if (enemyContext.enemyInSight ()) {
			playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		}

		if ((!enemyContext.enemyInRange(0.5f) && !enemyContext.enemyTooClose())) {
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

		if (PathFinder.atDestination (enemyContext.enemy.transform, closestNode.transform.position)) {
			closestNode = PathFinder.getRetreatNeighbor (closestNode, enemyContext.enemy.transform, playerPosition);
			if (closestNode == null) {
				closestNode = PathFinder.getRetreatNode (enemyContext.pathNodes, enemyContext.enemyPhysics.transform, playerPosition);
			}
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

	public override void OnReset(){
		playerPosition = new Vector3(0, 0, 0);
		closestNode = null;
	}

}
