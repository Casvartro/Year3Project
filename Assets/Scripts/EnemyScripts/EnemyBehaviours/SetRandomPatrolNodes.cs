using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomPatrolNodes : Leaf {

	//Leaf node responsible for getting the start node and the end node responsible for the enemy
	//patrolling.

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;

		if (enemyContext.enemyInSight () || enemyContext.enemyInRange (0.5f)) {
			return BehaviourStatus.FAILURE;
		}

		if (enemyContext.startNode == null) {
			enemyContext.startNode = enemyContext.enemyPhysics.getInitialNode (enemyContext.pathNodes);
			enemyContext.startInfo = enemyContext.startNode.GetComponent<NodeController> ();
		}
		enemyContext.endNode = enemyContext.enemyPhysics.getEndNode (enemyContext.pathNodes, enemyContext.startNode);
		return BehaviourStatus.SUCCESS;

	}

	public override void OnReset(){
	}

}
