using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviourStatus {

	FAILURE,
	SUCCESS,
	RUNNING

}

/* /////////////////////////////////////////////////////////// */


public abstract class BehaviourState{

}

/* /////////////////////////////////////////////////////////// */

public abstract class BehaviourNode{

	protected bool debug = false;

	public bool starting = true;
	public int ticks = 0;

	public static List<string> debugTypeBlacklist = new List<string> () {
		"Selector",
		"Sequence",
		"Repeater",
		"Inverter",
		"Succeeder"
	};

	public virtual BehaviourStatus Behave(BehaviourState state){

		BehaviourStatus currentState = OnBehave (state);

		if (debug && !debugTypeBlacklist.Contains (GetType ().Name)) {
			
			string result = "N/A";
			switch (currentState){
				case BehaviourStatus.SUCCESS:
					result = "success";
					break;
				case BehaviourStatus.FAILURE:
					result = "failure";
					break;
				case BehaviourStatus.RUNNING:
					result = "running";
					break;
			}

			Debug.Log ("Behaving: " + GetType ().Name + " - " + result);
		}

		ticks++;
		starting = false;

		if (currentState != BehaviourStatus.RUNNING) {
			Reset ();
		}

		return currentState;
	}

	public abstract BehaviourStatus OnBehave (BehaviourState state);

	public void Reset(){
		starting = true;
		ticks = 0; 
		OnReset ();
	}

	public abstract void OnReset ();
}

/* /////////////////////////////////////////////////////////// */

public abstract class Composite : BehaviourNode{

	protected List<BehaviourNode> children = new List<BehaviourNode>();

	public string compositeName;

	public  Composite(string name, params BehaviourNode[] bNodes){
		compositeName = name;
		children.AddRange (bNodes);
	}

	public override BehaviourStatus Behave(BehaviourState state){

		bool shouldLog = debug && ticks == 0 ? true : false;
		if (shouldLog) {
			Debug.Log ("Running behaviour list:  " + compositeName);
		}

		BehaviourStatus currentState = base.Behave (state);

		if (debug && currentState != BehaviourStatus.RUNNING) {
			Debug.Log ("Behaviour list" + compositeName + " returned: " + currentState.ToString ());
		}

		return currentState;

	}

}

/* /////////////////////////////////////////////////////////// */

public abstract class Leaf : BehaviourNode {

}

public abstract class Decorator: BehaviourNode {

	protected BehaviourNode child;

	public Decorator(BehaviourNode bNode){
		child = bNode;
	}

}