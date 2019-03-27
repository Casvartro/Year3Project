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

	private NodeController currentNode = null;
	private float offsetValue = 2f;
	private bool offsetMod = false;
	private Vector3 currPos;
	private float angle = 10;

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;

		if (enemyContext.enemyInSight() || (enemyContext.playerHeard && enemyContext.playerRecentShot)) {
			playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
			if (enemyContext.playerRecentShot) {
				enemyContext.playerRecentShot = false;
			}
		}

		if(!enemyContext.enemyInSight() && !enemyContext.playerHeard && playerPath.Count == 0){
			return BehaviourStatus.FAILURE;
		}

		if (atPlayer (enemyContext.enemy.transform, playerPosition, enemyContext.enemyRange) && enemyContext.enemyInSight()) {
			enemyContext.enemyAnimation.Play ("idle");
			enemyContext.playerHeard = false;
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
						offsetMod = false;
						pathCounter = 0;

						NodeController firstNode = (NodeController)playerPath [pathCounter];
						if (PathFinder.isNodeBehind (firstNode.transform.position, enemyContext.enemy.transform)) {
							pathCounter++;
						}
					}

				}

				if (playerPath.Count > 0) {

					currentNode = (NodeController)playerPath [pathCounter];
					modPositionOffset ();
					targetPosition = currPos;
				

					if (PathFinder.atDestination (enemyContext.enemy.transform, targetPosition)) {

						if (pathCounter == playerPath.Count - 1) {
							playerPath.Clear ();
							if (enemyContext.enemyInSight ()) {
								targetPosition = playerPosition;
							} else {
								enemyContext.playerHeard = false;
								return BehaviourStatus.FAILURE;
							}
						} else {
							pathCounter++;
							currentNode = (NodeController)playerPath [pathCounter];
							offsetMod = false;
							modPositionOffset ();
							targetPosition = currPos;
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

	//Method for calling the functions for rotating and moving the enemy
	private void rotateAndMove(BehaviourContext context, Vector3 position){
		context.enemyPhysics.enemyRotation (position);
		if (context.enemyAnimation.GetCurrentAnimatorStateInfo (0).IsName ("idle")) {
			context.enemyAnimation.Play ("walk");
		}
		context.enemyPhysics.enemyMovement (position, angle);
	}

	private void modPositionOffset(){
		if (!offsetMod) {
			currPos = currentNode.transform.position;
			currPos = new Vector3 (Random.Range (currPos.x - offsetValue, currPos.x + offsetValue), 
				currPos.y, Random.Range (currPos.z - offsetValue, currPos.z + offsetValue));
			offsetMod = true;
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

		offsetMod = false;
		currentNode = null;
		currPos = new Vector3 (0, 0, 0);
	}
}
