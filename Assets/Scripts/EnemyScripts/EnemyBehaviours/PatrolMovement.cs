using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : Leaf {

	//Class responsible for the patrol movements of the enemy characters in the OnBehave.

	private ArrayList patrolPath =  new ArrayList();
	private int pathCounter = 0;
	private float angle = 10;
	private NodeController currentNode = null;
	private float offsetValue = 2.0f;
	private bool offsetMod = false;
	private Vector3 currPos;

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;

		if (enemyContext.enemySight.getPlayerSeen() || enemyContext.enemyInRange (0.5f) || enemyContext.playerHeard) {
			return BehaviourStatus.FAILURE;
		}
			

		if (patrolPath.Count == 0) {
			patrolPath = PathFinder.getPath (enemyContext.startNode, enemyContext.endNode);
		}

		if (patrolPath.Count > 0) {

			currentNode = (NodeController)patrolPath [pathCounter];
			modPositionOffset ();

			if (PathFinder.atDestination (enemyContext.enemy.transform, currPos)) {
				enemyContext.enemyAnimation.Play ("idle");
				pathCounter++;
				currentNode = (NodeController)patrolPath [pathCounter];
				offsetMod = false;
				modPositionOffset ();
				if (currentNode.transform.position == enemyContext.endNode.transform.position) {
					enemyContext.startNode = enemyContext.endNode;
					enemyContext.startInfo = currentNode;
					return BehaviourStatus.SUCCESS;
				}
			}

			enemyContext.enemyPhysics.enemyRotation (currPos);
			if (enemyContext.enemyAnimation.GetCurrentAnimatorStateInfo (0).IsName ("idle")) {
				enemyContext.enemyAnimation.Play ("walk");
			}
			enemyContext.enemyPhysics.enemyMovement (currPos, angle);

		}

		return BehaviourStatus.RUNNING;
	}

	private void modPositionOffset(){
		if (!offsetMod) {
			currPos = currentNode.transform.position;
			currPos = new Vector3 (Random.Range (currPos.x - offsetValue, currPos.x + offsetValue), 
				currPos.y, Random.Range (currPos.z - offsetValue, currPos.z + offsetValue));
			offsetMod = true;
		}
	}

	//Resets Leaf nodes information.
	public override void OnReset(){
		patrolPath = new ArrayList ();
		pathCounter = 0;
		offsetMod = false;
		currentNode = null;
		currPos = new Vector3 (0, 0, 0);
	}

}
