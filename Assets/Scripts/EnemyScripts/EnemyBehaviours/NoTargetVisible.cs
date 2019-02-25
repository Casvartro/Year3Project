using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoTargetVisible : Leaf {

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;
		if (!enemyContext.enemySight.getPlayerSeen ()) {
			if (enemyContext.enemySight.enableDebug) {
				Debug.Log ("Player Not Found");
			}
			return BehaviourStatus.SUCCESS;
		} else {
			return BehaviourStatus.FAILURE;
		}

	}

	public override void OnReset(){

	}

}
