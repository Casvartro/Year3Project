using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InFiringRange : Leaf {

	//Leaf node that checks whether or not it is directly close to the player for a ranged attack.

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;

		if (enemyContext.enemyInRange(0.0f) && !enemyContext.enemyTooClose()) {

			if (enemyContext.enemySight.enableDebug) {
				Debug.Log ("Player Can be Shot");
			}

			enemyContext.startNode = null;
			return BehaviourStatus.SUCCESS;

		}

		return BehaviourStatus.FAILURE;


	}

	public override void OnReset(){
	}

}