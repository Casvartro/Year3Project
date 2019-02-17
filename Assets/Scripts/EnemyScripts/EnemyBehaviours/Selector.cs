using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Composite {

	int currentChild = 0;

	public Selector (string compositeName, params BehaviourNode[] bNodes) : base (compositeName, bNodes){

	}

	public override BehaviourStatus OnBehave(BehaviourState state){

		if (currentChild >= children.Count) {
			return BehaviourStatus.FAILURE;
		}

		BehaviourStatus currentState = children [currentChild].Behave (state);

		switch (currentState) {
			case BehaviourStatus.SUCCESS:
				return BehaviourStatus.SUCCESS;

			case BehaviourStatus.FAILURE:
				currentChild++;

				//If failure, process the next child node.
				return OnBehave (state);
		}

		return BehaviourStatus.RUNNING;
	}

	public override void OnReset(){

		currentChild = 0;
		foreach (BehaviourNode child in children) {
			child.Reset ();
		}

	}

}
