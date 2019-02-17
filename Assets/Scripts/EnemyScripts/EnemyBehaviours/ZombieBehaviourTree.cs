using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviourTree : MonoBehaviour {

	BehaviourNode behaviourTree;
	BehaviourContext behaviourState;
	CharacterController enemy;
	EnemyController enemyPhysics;
	Animator enemyAnimation;

	void Start(){

		this.enemy = this.GetComponent<CharacterController> ();
		this.enemyPhysics = this.GetComponent<EnemyController>();
		this.enemyAnimation = this.GetComponent<Animator> ();
		this.behaviourTree = createBehaviourTree ();
		this.behaviourState = new BehaviourContext (enemy, enemyPhysics, enemyAnimation); 
	}

	void FixedUpdate(){
		this.behaviourTree.Behave (behaviourState);
	}

	BehaviourNode createBehaviourTree(){

		Sequence patrol = new Sequence ("patrol", new SetRandomPatrolNodes (), new PatrolMovement ());

		return patrol;
	}

}
