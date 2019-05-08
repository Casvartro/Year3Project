using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBehaviourTree : MonoBehaviour {

	/* Class responsible for the soldier's behaviour tree and initalizing the behaviour context
	 * for the soldier as well retrieving all of its properties for use. */

	public float enemyRange;
	public float enemyCloseRange;
	public BehaviourContext behaviourState;
	private BehaviourNode behaviourTree;
	private CharacterController enemy;
	private EnemyController enemyPhysics;
	private Animator enemyAnimation;
	private Sight enemySight;
	private FireEnemyWeapon enemyWeaponController;

	void Start(){

		this.enemy = this.GetComponent<CharacterController> ();
		this.enemyPhysics = this.GetComponent<EnemyController>();
		this.enemyAnimation = this.GetComponent<Animator> ();
		this.enemySight = this.GetComponent<Sight>();
		this.behaviourTree = createBehaviourTree ();
		this.enemyWeaponController = this.GetComponent<FireEnemyWeapon> ();
		this.behaviourState = new BehaviourContext (enemy, enemyPhysics, enemyAnimation, 
			enemySight, enemyRange, enemyWeaponController, enemyCloseRange); 
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (this.enemyPhysics.getEnemyHealth () > 0) {
			this.behaviourTree.Behave (behaviourState);
		}
	}

	//Behaviour tree creates of the 4 sequences that are available to the soldier. All sequences are ran by the selector returned.
	BehaviourNode createBehaviourTree(){
		
		Sequence patrol = new Sequence ("patrol", 
										new NoTargetVisible(), 
										new SetRandomPatrolNodes (), 
										new PatrolMovement ());

		Sequence chase = new Sequence ("chase",
										new TargetVisible (),
										new ChasePlayer ());

		Sequence attack = new Sequence ("attack",
										new InFiringRange(),
										new ShootPlayer ());

		Sequence retreat = new Sequence ("retreat",
			                   			 new TooCloseToPlayer (),
			                  			 new EnemyFallback ());
			
		
		Selector patrolOrChaseOrAttackOrRetreat = new Selector ("patrolOrChaseOrAttackOrRetreat", 
			patrol, 
			chase,
			attack,
			retreat);
		
		return patrolOrChaseOrAttackOrRetreat;

	}

}
