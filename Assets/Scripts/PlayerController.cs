using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float movementSpeed;
	public float jumpSpeed;
	public float gravity = 20.0f;
	public Transform cameraPos;
	public float cameraSpeed;
	public Text healthText;

	private float rotY = 0.0f;
	private float rotX = 0.0f;
	private int playerHealth = 100;
	private float playerHeight;

	private float slopeForce = 5;
	private float slopeForceRayLength = 1.5f;
	private bool currentlyJumping;

	private Vector3 movementDir;
	private CharacterController player;

	// Use this for initialization
	void Start () {
		player = this.GetComponent<CharacterController> ();
		playerHeight = player.height;
		cameraPos.position = new Vector3 (this.transform.position.x, this.transform.position.y - 0.5f,
			this.transform.position.z);
		healthText.text = playerHealth.ToString();
		healthText.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {

		playerCameraSight ();

	}

	private void playerCameraSight(){
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

		playerMovement ();
		
	}

	private void playerMovement(){
		//Function that handles all player movement

		float height = playerHeight;
		float playerSpeed = movementSpeed;
		float playerJumpSpeed = jumpSpeed;

		if (player.isGrounded) {

			currentlyJumping = false;

			if (Input.GetButton ("Sprint")) {
				playerSpeed = playerSpeed * 2;
				playerJumpSpeed = playerJumpSpeed * 1.5f;
			}

			if (Input.GetButton("Crouch")){
				height = 0.5f * height;
				playerSpeed = playerSpeed - 0.5f;
			}


			//Updates based on player input.
			movementDir = Vector3.zero;
			movementDir.x = Input.GetAxis ("Horizontal");
			movementDir.z = Input.GetAxis ("Vertical");
			//Normalizes the vector to prevent diagnonal input from moving too fast.
			movementDir = movementDir.normalized * playerSpeed;

			movementDir = transform.TransformDirection (movementDir);

			if (Input.GetButton ("Jump")) {
				movementDir.y = playerJumpSpeed;
				currentlyJumping = true;
			}

		}

		setPlayerHeight (height);

		//Gravity is applied to the vertical axis
		movementDir.y = movementDir.y - (gravity * Time.fixedDeltaTime);
		//Player is moved
		player.Move (movementDir * Time.deltaTime);

		if ((movementDir.x != 0 || movementDir.z != -0) && OnSlope (currentlyJumping)) {
			player.Move (Vector3.down * player.height / 2 * slopeForce * Time.fixedDeltaTime);
		}

	}

	private bool OnSlope(bool isJump){
		//Function for checking if the player is moving on a slope.

		if (isJump) {
			return false;
		}

		RaycastHit hit;

		if (Physics.Raycast (this.transform.position, Vector3.down, out hit, player.height / 2 * slopeForceRayLength)) {
			if (hit.normal != Vector3.up) {
				return true;
			}
		}
		return false;

	}

	private void setPlayerHeight(float height){
		//Modifies the players height based on whether or not they are crouching.

		//Crouch/Standup Smoother
		float lastHeight = player.height; 
		player.height = Mathf.Lerp (player.height, height, 5 * Time.fixedDeltaTime);
		//Fixes Y position
		float fixedHeight = (player.height - lastHeight) / 2;
		this.transform.position = new Vector3 (this.transform.position.x,
			this.transform.position.y + fixedHeight, this.transform.position.z);
	}

}
