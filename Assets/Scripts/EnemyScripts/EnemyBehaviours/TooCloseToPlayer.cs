using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooCloseToPlayer : Leaf {

	//Node that checks whether or not it is directly close to the player to back up.

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourContext enemyContext = (BehaviourContext)state;

		if (enemyContext.enemyInSight() && enemyContext.enemyTooClose()) {

			if (enemyContext.enemySight.enableDebug) {
				Debug.Log ("Player Too Close, Backing UP");
			}

			enemyContext.startNode = null;
			return BehaviourStatus.SUCCESS;

		}

		return BehaviourStatus.FAILURE;


	}

	public override void OnReset(){
	}

}
