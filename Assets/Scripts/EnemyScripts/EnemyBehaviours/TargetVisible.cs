using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetVisible : Leaf {

	public float range = 2.0f;

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;
		if (enemyContext.enemySight.getPlayerSeen () && enemyContext.enemySight.distanceToPlayer() > range) {
			if (enemyContext.enemySight.enableDebug) {
				Debug.Log ("Player Found");
			}
			enemyContext.startNode = null;
			return BehaviourStatus.SUCCESS;
		} else {
			return BehaviourStatus.FAILURE;
		}

	}

	public override void OnReset(){

	}

}