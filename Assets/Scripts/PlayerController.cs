using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float movementSpeed;
	public float gravity = 20.0f;

	public float cameraSpeed;
	private float rotY = 0.0f;
	private float rotX = 0.0f;

	private Vector3 movementDir;
	private CharacterController player;

	// Use this for initialization
	void Start () {
		player = this.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		//Updates camera rotation based on player input from mouse movement
		rotX += cameraSpeed * Input.GetAxis("Mouse X");
		rotY -= cameraSpeed * Input.GetAxis("Mouse Y");

		if (rotY > 90.0f) {
			rotY = 90.0f;
		} else if (rotY < -90.0f) {
			rotY = -90.0f;
		}

		//Euler angles used to overwrite the rotation value each time it changes to its new current position.
		transform.eulerAngles = new Vector3(rotY, rotX, 0.0f);

	}


	void FixedUpdate () {

		if (player.isGrounded) {
			//Updates based on player input.
			movementDir = Vector3.zero;
			movementDir.x = Input.GetAxis ("Horizontal") * movementSpeed;
			movementDir.z = Input.GetAxis ("Vertical") * movementSpeed;

			movementDir = transform.TransformDirection (movementDir);
		}

		Debug.Log (movementDir);
		//Gravity is applied to the vertical axis
		movementDir.y = movementDir.y - (gravity * Time.deltaTime);

		//Player is moved
		player.Move (movementDir * Time.deltaTime);
		
	}
}
