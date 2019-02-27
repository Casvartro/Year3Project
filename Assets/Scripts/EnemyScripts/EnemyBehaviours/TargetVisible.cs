using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetVisible : Leaf {


	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;

		if (enemyContext.enemyInSight() && !enemyContext.enemyInRange(0.5f)) {

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