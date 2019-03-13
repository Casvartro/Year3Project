using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBehaviourTree : MonoBehaviour {

	public float enemyRange;
	public float enemyCloseRange;
	private BehaviourNode behaviourTree;
	private BehaviourContext behaviourState;
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
