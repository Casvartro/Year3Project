using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : Leaf {

	//Class responsible for the patrol movements of the enemy characters in the OnBehave.

	private ArrayList patrolPath =  new ArrayList();
	private int pathCounter = 0;

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;

		if (enemyContext.enemySight.getPlayerSeen()) {
			enemyContext.enemyAnimation.Play ("idle");
			return BehaviourStatus.FAILURE;
		}
			

		if (patrolPath.Count == 0) {
			patrolPath = PathFinder.getPath (enemyContext.startNode, enemyContext.endNode);
			Debug.Log (enemyContext.startNode);
			Debug.Log (enemyContext.endNode);
			Debug.Log (patrolPath [1]);
		}

		if (patrolPath.Count > 0) {
				
			NodeController currentNode = (NodeController)patrolPath [pathCounter];

			if (atDestination (enemyContext.enemy.transform, currentNode.transform)) {
				enemyContext.enemyAnimation.Play ("idle");
				pathCounter++;
				currentNode = (NodeController)patrolPath [pathCounter];
				if (currentNode.transform.position == enemyContext.endNode.transform.position) {
					enemyContext.startNode = enemyContext.endNode;
					enemyContext.startInfo = currentNode;
					return BehaviourStatus.SUCCESS;
				}
			}

			enemyContext.enemyPhysics.enemyRotation (currentNode.transform.position);
			if (enemyContext.enemyAnimation.GetCurrentAnimatorStateInfo (0).IsName ("idle")) {
				enemyContext.enemyAnimation.Play ("walk");
			}
			enemyContext.enemyPhysics.enemyMovement (currentNode.transform.position);

		}

		return BehaviourStatus.RUNNING;
	}

	//Checks if the character has reached its destination.
	private bool atDestination(Transform currentPosition, Transform destPosition){

		Vector3 direction = destPosition.position - currentPosition.position;
		direction.y = 0;
		if (direction.magnitude < .2f){
			return true;
		}

		return false;

	}

	//Resets Leaf nodes information.
	public override void OnReset(){
		patrolPath = new ArrayList ();
		pathCounter = 0;
	}

}
