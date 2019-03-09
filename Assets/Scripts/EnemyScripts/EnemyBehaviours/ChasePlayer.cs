using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : Leaf {

	//Node that chases the player until it is right next to it for a melee attack.
	private Vector3 playerPosition;
	private Vector3 targetPosition;

	private RaycastHit playerHit;
	private RaycastHit enemyHit;
	private ArrayList playerPath = new ArrayList();
	private int pathCounter = 0;
	private GameObject startNode;
	private GameObject endNode;
	private string currentPlaneName = null;

	public override BehaviourStatus OnBehave(BehaviourState state){
		
		BehaviourContext enemyContext = (BehaviourContext)state;

		if (enemyContext.enemySight.getPlayerSeen ()) {

			playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
	
		}

		if(!enemyContext.enemyInSight() && playerPath.Count == 0){
			return BehaviourStatus.FAILURE;
		}
			
		if (atPlayer (enemyContext.enemy.transform, playerPosition, enemyContext.enemyRange)) {
			return BehaviourStatus.SUCCESS;
		}

		if (Physics.Raycast (playerPosition, Vector3.down, out playerHit, 10.0f) && Physics.Raycast (enemyContext.enemy.transform.position, Vector3.down, out enemyHit, 10.0f)) {

			if (playerHit.collider.name == enemyHit.collider.name) {

				targetPosition = playerPosition;
				rotateAndMove (enemyContext, targetPosition);

			} else {

				if (playerHit.collider.name != currentPlaneName) {

					startNode = PathFinder.getPlaneNode (enemyContext.planeNodes, enemyContext.enemy.transform.position, enemyHit);
					endNode = PathFinder.getPlaneNode (enemyContext.planeNodes, playerPosition, playerHit);
					currentPlaneName = playerHit.collider.name;

					if (startNode.transform == endNode.transform) {
						if (playerPath.Count > 0) {
							playerPath.Clear ();
						}
						targetPosition = playerPosition;
					} else {

						playerPath = PathFinder.getPath (startNode, endNode);
						pathCounter = 0;

						NodeController firstNode = (NodeController)playerPath [pathCounter];
						if (isNodeBehind (firstNode.transform, enemyContext.enemy.transform)) {
							pathCounter++;
						}
					}

				}

				if (playerPath.Count > 0) {

					NodeController currentNode = (NodeController)playerPath [pathCounter];
					targetPosition = currentNode.transform.position;

					if (atDestination (enemyContext.enemy.transform, currentNode.transform)) {
						if (atDestination (enemyContext.enemy.transform, endNode.transform)) {
							playerPath.Clear ();
							if (enemyContext.enemyInSight ()) {
								targetPosition = playerPosition;
							} else {
								return BehaviourStatus.FAILURE;
							}
						} else {
							pathCounter++;
							currentNode = (NodeController)playerPath [pathCounter];
							targetPosition = currentNode.transform.position;
						}

					}
				}

				rotateAndMove (enemyContext, targetPosition);
			}
		}
 		
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

	//Checks if character has reached node destination
	private bool atDestination(Transform currentPosition, Transform destPosition){

		Vector3 direction = destPosition.position - currentPosition.position;
		direction.y = 0;
		if (direction.magnitude < .2f){
			return true;
		}

		return false;

	}
		

	//Method for calling the functions for rotating and moving the enemy
	private void rotateAndMove(BehaviourContext context, Vector3 position){
		context.enemyPhysics.enemyRotation (position);
		if (context.enemyAnimation.GetCurrentAnimatorStateInfo (0).IsName ("idle")) {
			context.enemyAnimation.Play ("walk");
		}
		context.enemyPhysics.enemyMovement (position);
	}

	//Checks if the node is behind the enemy so it doesnt double back.
	private bool isNodeBehind(Transform targetPos, Transform sourcePos){

		Vector3 toTarget = (targetPos.position - sourcePos.position).normalized;
		if (Vector3.Dot(toTarget, sourcePos.forward) > 0) {
			return false;
		} else {
			return true;
		}

	}
		
	public override void OnReset(){
		playerPosition = new Vector3(0, 0, 0);
		targetPosition = new Vector3 (0, 0, 0);

		playerPath.Clear ();
		pathCounter = 0;
		startNode = null;
		endNode = null;
		currentPlaneName = null;
	
	}
}
