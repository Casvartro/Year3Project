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
	public int maxHealth = 100;
	public Image powerUpPanel;
	public Text powerUpText;
	public int powerMultiplier = 2;
	public enum powerUpState{ NONE, BLUE, RED, GREEN};

	private float rotY = 0.0f;
	private float rotX = 0.0f;
	private int playerHealth = 50;
	private float playerHeight;
	private float slopeForce = 5;
	private float slopeForceRayLength = 1.5f;
	private bool currentlyJumping;
	private bool powerUpOn = false;
	private float powerTime;
	private powerUpState currentPower;
	private Vector3 movementDir;
	private CharacterController player;
	private GameController gameStatus;

	// Use this for initialization
	void Start () {

		currentPower = powerUpState.NONE;
		gameStatus = GameObject.Find ("UICanvas").GetComponent<GameController> ();
		player = this.GetComponent<CharacterController> ();
		playerHeight = player.height;
		playerHealth = maxHealth;
		healthText.text = playerHealth.ToString();
		setHealthTextColor ();

		cameraPos.position = new Vector3 (this.transform.position.x, this.transform.position.y - 0.5f,
			this.transform.position.z);
		
	}
	
	// Update is called once per frame
	void Update () {

		if (!gameStatus.checkPause ()) {
			
			playerCameraSight ();

			healthText.text = playerHealth.ToString();
			setHealthTextColor ();

			if (powerUpOn) {
				activatePowerUp ();
			}
		}

	}

	void FixedUpdate () {

		playerMovement ();
		
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

	private void playerMovement(){
		//Function that handles all player movement

		float height = playerHeight;
		float playerSpeed = movementSpeed;
		if (currentPower == powerUpState.BLUE) {
			playerSpeed *= powerMultiplier; 
		}

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

	private void setHealthTextColor(){
		//Sets players health text color depending on the value.

		if (playerHealth >= 75) {
			healthText.color = Color.green;
		} else if (playerHealth >= 40 && playerHealth < 75) {
			healthText.color = Color.yellow;
		} else {
			healthText.color = Color.red;
		}
	}

	public int getPlayerHealth(){
		//Returns the players current health.

		return playerHealth;
	}

	public void setPlayerHealth(int healthIncrease){
		//Adds health item value to players health setting text as well.#

		playerHealth = playerHealth + healthIncrease;
		if (playerHealth > maxHealth) {
			playerHealth = maxHealth;
		}
		healthText.text = playerHealth.ToString();
		setHealthTextColor ();
	}
		
	public void setPowerUpBar(GameObject powerUp, float powerUpTime, string powerColor){
		//Sets the type of power to the power up bar when collided with

		powerUpPanel.color = powerUp.GetComponent<Renderer> ().material.color;
		powerTime = powerUpTime;
		powerUpOn = true;

		if (powerColor == "red") {
			currentPower = powerUpState.RED;
		} else if (powerColor == "green") {
			currentPower = powerUpState.GREEN;
		} else if (powerColor == "blue") {
			currentPower = powerUpState.BLUE;
		}
	}

	private void activatePowerUp(){
		//Sets timer for player power as well as monitors how long is left.
		
		powerUpText.text = ((int)powerTime).ToString ();
		powerTime -= Time.deltaTime;
		if (powerTime < 0) {
			powerUpPanel.color = Color.black;
			powerUpText.text = "";
			powerUpOn = false;
			currentPower = powerUpState.NONE;
		}
	}
		
	public powerUpState getCurrentPower(){
		//Returns the current power of the player
		return currentPower;
	}

	//Responsible for showing damage when taken from enemy attacks.
	public void takeDamage(int damage){

		playerHealth -= damage;

		if (playerHealth < 0) {
			playerHealth = 0;
		}

	}

}
