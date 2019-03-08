using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : Sense {

	public int fieldOfView = 45;
	public int viewDistance = 100;

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
		rayDirection = playerTransform.position - transform.position;

		if((Vector3.Angle(rayDirection, transform.forward)) < fieldOfView){
			//Detect if the player is within the field of view.
			if (Physics.Raycast (transform.position, rayDirection, out hit, viewDistance)) {
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
		return Vector3.Distance(playerTransform.position, this.transform.position);
	}

	void onDrawGizmos(){

		if (playerTransform == null) {
			return;
		}

		Debug.DrawLine (transform.position, playerTransform.position, Color.red);

		Vector3 frontRayPoint = transform.position + (transform.forward * viewDistance);

		//Approximate perspective visualization
		Vector3 leftRayPoint = frontRayPoint;
		leftRayPoint.x += fieldOfView * 0.5f;

		Vector3 rightRayPoint = frontRayPoint;
		rightRayPoint.x -= fieldOfView * 0.5f;

		Debug.DrawLine (transform.position, frontRayPoint, Color.green);
		Debug.DrawLine (transform.position, leftRayPoint, Color.green);
		Debug.DrawLine (transform.position, rightRayPoint, Color.green);

	}

}
