using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourContext : BehaviourState {

	//Context specific to this instance of the enemy and its current behaviour and personal information.

	public CharacterController enemy;
	public EnemyController enemyPhysics;
	public Animator enemyAnimation;
	public GameObject[] pathNodes;
	public GameObject startNode = null;
	public NodeController startInfo = null;
	public GameObject endNode = null;
	public Sight enemySight;
	public float enemyRange;

	public BehaviourContext(CharacterController enemy, EnemyController enemyPhy, Animator enemyAn, Sight sight, float range){
		this.enemy = enemy;
		this.enemyPhysics = enemyPhy;
		this.enemyAnimation = enemyAn;
		this.enemySight = sight;
		this.enemyRange = range;
		pathNodes = GameObject.FindGameObjectsWithTag("PathNode");

	}

	public bool enemyInSight(){
		return enemySight.getPlayerSeen ();
	}

	public bool enemyInRange(float modifier){
		return enemySight.distanceToPlayer () < enemyRange + modifier;
	}
		
}
