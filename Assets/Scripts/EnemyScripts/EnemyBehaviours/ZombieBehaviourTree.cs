using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviourTree : MonoBehaviour {

	/* Class responsible for the zombie's behaviour tree and initalizing the behaviour context
	 * for the zombie as well retrieving all of its properties for use. */


	public float enemyRange;
	public BehaviourContext behaviourState;
	private BehaviourNode behaviourTree;
	private CharacterController enemy;
	private EnemyController enemyPhysics;
	private Animator enemyAnimation;
	private Sight enemySight;

	void Start(){

		this.enemy = this.GetComponent<CharacterController> ();
		this.enemyPhysics = this.GetComponent<EnemyController>();
		this.enemyAnimation = this.GetComponent<Animator> ();
		this.enemySight = this.GetComponent<Sight>();
		this.behaviourTree = createBehaviourTree ();
		this.behaviourState = new BehaviourContext (enemy, enemyPhysics, enemyAnimation, 
			enemySight, enemyRange, null, 0.0f); 
	}

	void FixedUpdate(){
		if (this.enemyPhysics.getEnemyHealth () > 0) {
			this.behaviourTree.Behave (behaviourState);
		}
	}

	//Behaviour tree creates of the 3 sequences that are available to the zombie. All sequences are ran by the selector returned.
	BehaviourNode createBehaviourTree(){

		Sequence patrol = new Sequence ("patrol", 
										new NoTargetVisible(), 
										new SetRandomPatrolNodes (), 
										new PatrolMovement ());

		Sequence chase = new Sequence ("chase",
			                 			new TargetVisible (),
			                			new ChasePlayer ());

		Sequence attack = new Sequence ("attack",
			                 			 new CloseToPlayer (),
			                 			 new MeleePlayer ());

		Selector patrolOrChaseOrAttack = new Selector ("patrolOrChaseOrAttack", 
			                         			patrol, 
												chase,
												attack);

		return patrolOrChaseOrAttack;
	}

}
