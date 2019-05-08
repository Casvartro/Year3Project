using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : Leaf {

	/*Leaf action responsible for chasing the player until it is right next to it for a melee attack. */

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
	private bool chaseHearing = false;
	private bool hearPath = false;

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;

		//Checks if the player is in sight, or the player has fired a shot that is has heard recently to chase.
		if (enemyContext.enemyInSight ()) {
			playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		} else if ( enemyContext.playerHeard && enemyContext.playerRecentShot){
			enemyContext.playerRecentShot = false;
			RaycastHit hit;
			playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
			Vector3 rayDirection = playerPosition - enemyContext.enemy.transform.position;
			if (Physics.Raycast(enemyContext.enemy.transform.position, rayDirection, out hit, enemyContext.enemySight.viewDistance)) {
				if (hit.collider.tag == "Player") {
					chaseHearing = true;
				} else {
					chaseHearing = false;
					hearPath = true;
				}
			}
		}

		//Fails if the player is not in sight, and it has not been heard after it has finished its path.
		if(!enemyContext.enemyInSight() && !enemyContext.playerHeard && playerPath.Count == 0){
			enemyContext.playerHeard = false;
			return BehaviourStatus.FAILURE;
		}

		//If it has reached the players position and the player is in sight it successful, else it fails if it reaches the last known
		//destination with no player.
		if (atPlayer (enemyContext.enemy.transform, playerPosition, enemyContext.enemyRange) && enemyContext.enemyInSight ()) {
			enemyContext.enemyAnimation.Play ("idle");
			enemyContext.playerHeard = false;
			return BehaviourStatus.SUCCESS;
		} else if(PathFinder.atDestination (enemyContext.enemy.transform, playerPosition) && !enemyContext.enemyInSight()){
			chaseHearing = false;
			enemyContext.playerHeard = false;
			return BehaviourStatus.FAILURE;
		}

		if (Physics.Raycast (playerPosition, Vector3.down, out playerHit, 30.0f) && Physics.Raycast (enemyContext.enemy.transform.position, Vector3.down, out enemyHit, 10.0f)) {

			//Checks if the player and enemy are on the same plane making sure they are heard or insight.
			//If so it moves to the player position.
			if (playerHit.collider.name == enemyHit.collider.name && (enemyContext.enemyInSight () || chaseHearing)) {

				if (playerPath.Count > 0) {
					playerPath.Clear ();
				}

				targetPosition = playerPosition;
		
			//If not it calculates a path to the player so it avoids walls and running into obstacles.
			} else if (playerHit.collider.name != currentPlaneName && (enemyContext.enemyInSight () || (!chaseHearing && hearPath))) {

				startNode = PathFinder.getPlaneNode (enemyContext.planeNodes, enemyContext.enemy.transform.position, enemyHit);
				endNode = PathFinder.getPlaneNode (enemyContext.planeNodes, playerPosition, playerHit);
				currentPlaneName = playerHit.collider.name;
				hearPath = false;

				if (startNode != null && endNode != null) {
					if (startNode.transform == endNode.transform) {
						if (playerPath.Count > 0) {
							playerPath.Clear ();
						}
						targetPosition = playerPosition;
					} else {

						playerPath = PathFinder.getPath (startNode, endNode);
						offsetMod = false;
						pathCounter = 0;

						//Makes sure it does not turn around to go to a node behind it.
						NodeController firstNode = (NodeController)playerPath [pathCounter];
						if (PathFinder.isNodeBehind (firstNode.transform.position, enemyContext.enemy.transform)) {
							pathCounter++;
						}
					}
				} else {
					targetPosition = playerPosition;
				}

			}

			//Part of the behaviour for moving through the path nodes and processing the queue.
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

	//Function responsible for getting a random offset of a nodes position to prevent enemy collision.
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
		chaseHearing = false;
		hearPath = false;
	}
}
