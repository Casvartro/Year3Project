﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourContext : BehaviourState {

	//Context specific to this instance of the enemy holding its current behaviour and personal information.

	public CharacterController enemy;
	public EnemyController enemyPhysics;
	public Animator enemyAnimation;
	public GameObject[] pathNodes;
	public GameObject startNode = null;
	public NodeController startInfo = null;
	public GameObject endNode = null;
	public Sight enemySight;
	public float enemyRange;
	public IDictionary<string, List<GameObject>> planeNodes;
	public FireEnemyWeapon enemyWeaponController;
	public float closeRange;

	public bool playerHeard = false;
	public bool playerRecentShot = false;

	public BehaviourContext(CharacterController enemy, EnemyController enemyPhy, Animator enemyAn, Sight sight, 
		float range, FireEnemyWeapon eWC, float eCloseRange){
		this.enemy = enemy;
		this.enemyPhysics = enemyPhy;
		this.enemyAnimation = enemyAn;
		this.enemySight = sight;
		this.enemyRange = range;
		this.enemyWeaponController = eWC;
		this.closeRange = eCloseRange;
		pathNodes = GameObject.FindGameObjectsWithTag("PathNode");
		planeNodes = PathFinder.getPathNodePlanes (pathNodes);
	}

	public bool enemyInSight(){
		return enemySight.getPlayerSeen ();
	}

	public bool enemyInRange(float modifier){
		return enemySight.distanceToPlayer () < enemyRange + modifier;
	}

	public bool enemyTooClose(){
		return enemySight.distanceToPlayer () < closeRange;
	}
		
}
