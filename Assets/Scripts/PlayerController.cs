using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float movementSpeed;
	public float jumpSpeed;
	public Vector3 crouchScale;
	public float gravity = 20.0f;
	public Transform cameraPos;
	public float cameraSpeed;

	private float rotY = 0.0f;
	private float rotX = 0.0f;
	private float playerSpeed = 0.0f;
	private float playerJumpSpeed = 0.0f;
	private Vector3 normalScale = new Vector3(1, 1, 1);

	private Vector3 movementDir;
	private CharacterController player;

	// Use this for initialization
	void Start () {
		player = this.GetComponent<CharacterController> ();
		cameraPos.position = new Vector3 (this.transform.position.x, this.transform.position.y - 0.5f,
			this.transform.position.z);
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
		transform.rotation = Quaternion.Euler(0.0f, rotX, 0.0f);
		//Rotates the camera view rather than the actual player to prevet player from trying to move vertically.
		cameraPos.localRotation = Quaternion.Euler (rotY, 0.0f, 0.0f);

	}


	void FixedUpdate () {

		if (player.isGrounded) {

			//Modifies the player speed based on keys pressed down.
			if (!Input.GetButton ("Sprint") || !Input.GetButton("Crouch")) {
				if (this.transform.localScale.y == crouchScale.y){
					this.transform.localScale = normalScale;
				}
				playerSpeed = movementSpeed;
				playerJumpSpeed = jumpSpeed;
			}
				
			if (Input.GetButton ("Sprint")) {
				playerSpeed = movementSpeed * 2;
				playerJumpSpeed = jumpSpeed * 1.5f;
			}

			if (Input.GetButton("Crouch")){
				if (this.transform.localScale.y != crouchScale.y){
					this.transform.localScale = crouchScale;
					this.transform.position = new Vector3 (this.transform.position.x, 
						crouchScale.y, this.transform.position.z);
				}
				playerSpeed = movementSpeed - 0.5f;
			}

			//Updates based on player input.
			movementDir = Vector3.zero;
			movementDir.x = Input.GetAxis ("Horizontal") * playerSpeed;
			movementDir.z = Input.GetAxis ("Vertical") * playerSpeed;

			movementDir = transform.TransformDirection (movementDir);

			if (Input.GetButton ("Jump")) {
				movementDir.y = playerJumpSpeed;
			}


		}
			
		//Gravity is applied to the vertical axis
		movementDir.y = movementDir.y - (gravity * Time.deltaTime);
		//Player is moved
		player.Move (movementDir * Time.deltaTime);
		
	}

}
