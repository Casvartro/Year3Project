using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : Sense {

	public int fieldOfView = 45;
	public int viewDistance = 100;
	public Transform headTransform;

	private Transform playerTransform;
	private Vector3 rayDirection;
	private bool playerSeen;

	protected override void Initialize(){
		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
		playerSeen = false;
	}

	protected override bool UpdateSense(){
		elapsedTime += Time.deltaTime;

		if (enableDebug) {
			onDrawGizmos ();
		}

		if (elapsedTime >= detectionRate) {
			playerSeen = detectPlayer ();
		}

		return playerSeen;
	}

	private bool detectPlayer(){
		
		RaycastHit hit;
		rayDirection = playerTransform.position - headTransform.position;
		if((Vector3.Angle(rayDirection, headTransform.forward)) < fieldOfView){
			//Detect if the player is within the field of view.
			if (Physics.Raycast (headTransform.position, rayDirection, out hit, viewDistance)) {
				if (hit.collider.tag == "Player") {
					return true;
				}
			}
		}
		return false;
	}

	public bool getPlayerSeen(){
		return playerSeen;
	}
		

	public float distanceToPlayer(){
		return Vector3.Distance(playerTransform.position, headTransform.position);
	}

	void onDrawGizmos(){

		if (playerTransform == null) {
			return;
		}

		Debug.DrawRay (headTransform.position, playerTransform.position, Color.red);

		Vector3 frontRayPoint = headTransform.position + (headTransform.forward * viewDistance);

		//Approximate perspective visualization
		Vector3 leftRayPoint = frontRayPoint;
		leftRayPoint.x += fieldOfView * 0.5f;

		Vector3 rightRayPoint = frontRayPoint;
		rightRayPoint.x -= fieldOfView * 0.5f;

		Debug.DrawRay(headTransform.position, frontRayPoint, Color.green);
		Debug.DrawRay (headTransform.position, leftRayPoint, Color.green);
		Debug.DrawRay (headTransform.position, rightRayPoint, Color.green);

	}

}
