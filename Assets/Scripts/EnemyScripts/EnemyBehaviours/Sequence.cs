using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Composite {

	int currentChild = 0;

	public Sequence(string compositeName, params BehaviourNode[] nodes) : base(compositeName, nodes){

	}

	public override BehaviourStatus OnBehave(BehaviourState state){

		BehaviourStatus currentState = children [currentChild].Behave (state);

		switch (currentState) {
			case BehaviourStatus.SUCCESS:
				currentChild++;
				break;
			case BehaviourStatus.FAILURE:
				return BehaviourStatus.FAILURE;
		}

		if (currentChild >= children.Count) {
			return BehaviourStatus.SUCCESS;
		} else if (currentState == BehaviourStatus.SUCCESS) {
			//If we succeed, don't wait for the next tick to process the next child.
			return OnBehave(state);
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
