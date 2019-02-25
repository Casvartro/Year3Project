using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviourTree : MonoBehaviour {

	//Behaviour tree class responsible for the base zombies behaviours.
	//Utilizies composite and leaf nodes for the zombies to operate.

	BehaviourNode behaviourTree;
	BehaviourContext behaviourState;
	CharacterController enemy;
	EnemyController enemyPhysics;
	Animator enemyAnimation;
	Sight enemySight;

	void Start(){

		this.enemy = this.GetComponent<CharacterController> ();
		this.enemyPhysics = this.GetComponent<EnemyController>();
		this.enemyAnimation = this.GetComponent<Animator> ();
		this.enemySight = this.GetComponent<Sight>();
		this.behaviourTree = createBehaviourTree ();
		this.behaviourState = new BehaviourContext (enemy, enemyPhysics, enemyAnimation, enemySight); 
	}

	void FixedUpdate(){
		if (this.enemyPhysics.getEnemyHealth () > 0) {
			this.behaviourTree.Behave (behaviourState);
		}
	}

	BehaviourNode createBehaviourTree(){

		Sequence patrol = new Sequence ("patrol", 
										new NoTargetVisible(), 
										new SetRandomPatrolNodes (), 
										new PatrolMovement ());

		Sequence chase = new Sequence ("chase",
			                 			new TargetVisible (),
			                			new ChasePlayer ());

		Selector patrolOrChase = new Selector ("patrolOrChase", 
			                         			patrol, 
												chase);

		return patrolOrChase;
	}

}
