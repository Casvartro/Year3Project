using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseToPlayer : Leaf {

	//Node that checks whether or not it is directly close to the player for a melee attack.

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;

		if (enemyContext.enemyInRange(0.3f)) {

			if (enemyContext.enemySight.enableDebug) {
				Debug.Log ("Player Near");
			}

			enemyContext.startNode = null;
			return BehaviourStatus.SUCCESS;

		}

		return BehaviourStatus.FAILURE;
		
			
	}

	public override void OnReset(){
	}

}
