using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoTargetVisible : Leaf {

	//Leaf node that checks whether or not the target is not seen or heard so it can patrol.

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;

		if (!enemyContext.enemyInSight() && !enemyContext.enemyInRange(0.5f) && !enemyContext.playerHeard) {

			if (enemyContext.enemySight.enableDebug) {
				Debug.Log ("Player Not Found");
			}

			return BehaviourStatus.SUCCESS;

		}
			
		return BehaviourStatus.FAILURE;

	}

	public override void OnReset(){
	}

}
