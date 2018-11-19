using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed;
	private Rigidbody playerRb;

	// Use this for initialization
	void Start () {
		
		playerRb = GetComponent<Rigidbody> ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void FixedUpdate () {
		
		//Updates based on player input.
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		//Updates current movement and speed.
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		playerRb.velocity = movement * speed;
	
	}
}
