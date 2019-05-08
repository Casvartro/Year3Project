using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetVisible : Leaf {

	//Leaf Node that checks if the target is visible or if it has been heard so it can chase.

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;

		if ((enemyContext.enemyInSight() || enemyContext.playerHeard) && !enemyContext.enemyInRange(0.5f)) {

			if (enemyContext.enemySight.enableDebug) {
				Debug.Log ("Player Found");
			}

			enemyContext.startNode = null;
			return BehaviourStatus.SUCCESS;

		} 

		return BehaviourStatus.FAILURE;

	}

	public override void OnReset(){

	}

}