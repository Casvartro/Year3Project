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
	private string currentPlaneName;
	//private GameObject currentEndNode;

	public override BehaviourStatus OnBehave(BehaviourState state){
		
		BehaviourContext enemyContext = (BehaviourContext)state;

		if (enemyContext.enemySight.getPlayerSeen ()) {

			playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
			if (Physics.Raycast (playerPosition, Vector3.down, out playerHit, 10.0f)) {
				//currentEndNode = PathFinder.getPlaneNode (enemyContext.planeNodes, playerPosition, playerHit);
				currentPlaneName = playerHit.collider.name;
			}

		}

		if(!enemyContext.enemyInSight() || enemyContext.enemyInRange(0.5f)){
			return BehaviourStatus.FAILURE;
		}
			
		if (atPlayer (enemyContext.enemy.transform, playerPosition, enemyContext.enemyRange)) {
			return BehaviourStatus.SUCCESS;
		}

		if (Physics.Raycast (playerPosition, Vector3.down, out playerHit, 10.0f) && Physics.Raycast (enemyContext.enemy.transform.position, Vector3.down, out enemyHit, 10.0f)) {

			if (playerHit.collider.name == enemyHit.collider.name) {

				Debug.Log ("Moving Towards player on same plane");
				targetPosition = playerPosition;
				rotateAndMove (enemyContext, targetPosition);

			} else {

				Debug.Log ("Player not on the same plane");

				Debug.Log (playerHit.collider.name + " != " + currentPlaneName);
				if (playerHit.collider.name != currentPlaneName) {

					startNode = PathFinder.getPlaneNode (enemyContext.planeNodes, enemyContext.enemy.transform.position, enemyHit);
					endNode = PathFinder.getPlaneNode (enemyContext.planeNodes, playerPosition, playerHit);
					currentPlaneName = playerHit.collider.name;

					Debug.Log ("StartNode: " + startNode);
					Debug.Log ("EndNode: " + endNode);

					if (startNode.transform == endNode.transform) {
						if (playerPath.Count > 0) {
							playerPath.Clear ();
						}
						targetPosition = playerPosition;
					} else {

						playerPath = PathFinder.getPath (startNode, endNode);
						pathCounter = 0;
					}

				}

				if (playerPath.Count > 0) {

					NodeController currentNode = (NodeController)playerPath [pathCounter];
					targetPosition = currentNode.transform.position;

					Debug.Log ("CurrentNode: " + currentNode);

					if (atDestination (enemyContext.enemy.transform, currentNode.transform)) {
						if (atDestination (enemyContext.enemy.transform, endNode.transform)) {
							playerPath.Clear ();
							targetPosition = playerPosition;
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

	private bool atDestination(Transform currentPosition, Transform destPosition){

		Vector3 direction = destPosition.position - currentPosition.position;
		direction.y = 0;
		if (direction.magnitude < .2f){
			return true;
		}

		return false;

	}
		

	private void rotateAndMove(BehaviourContext context, Vector3 position){
		context.enemyPhysics.enemyRotation (position);
		if (context.enemyAnimation.GetCurrentAnimatorStateInfo (0).IsName ("idle")) {
			context.enemyAnimation.Play ("walk");
		}
		context.enemyPhysics.enemyMovement (position);
	}
		
	public override void OnReset(){
		playerPosition = new Vector3(0, 0, 0);
		targetPosition = new Vector3 (0, 0, 0);

		playerPath.Clear ();
		pathCounter = 0;
		startNode = null;
		endNode = null;
		currentPlaneName = null;
		//currentEndNode = null;
	}
}
