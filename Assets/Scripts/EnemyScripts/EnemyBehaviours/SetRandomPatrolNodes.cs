using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomPatrolNodes : Leaf {

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;
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
