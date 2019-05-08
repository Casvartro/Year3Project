using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	/*Class responsible for holding the values and information for the invidiual enemies. 
	 * Holds the movement physics and functions as well as rotating the model towards its target.
	 * Holds the functions for flashing the model red when damage is recieved as well as updates the health 
	 * if damage is taken and sets the enemy in a state of alive, dying or dead. */

	public int enemyHealth = 100;
	public int enemyScore = 100;
	public float enemySpeed = 5;
	public float enemyRotateSpeed = 5f;
	public float enemyAnimationSpeed = 2.0f;
	public int enemyDamage = 3;
	public string enemyColor;
	public string enemyType;
	public float gravity = 20.0f;
	public float vertSpeed = 0.0f;
	public float flashTime;

	private CollisionFlags collisionFlag;

	private int startHealth;
	private int startSpeed;
	private int startAnimationSpeed;
	private int startDamage;

	private float slopeForce = 5;
	private float slopeForceRayLength = 1.5f;

	private SkinnedMeshRenderer[] renderers;
	private ArrayList originalColor = new ArrayList();

	private WaveController waveController;
	private GameController scoreController;
	private CharacterController enemy;
	private FireEnemyWeapon enemyWeapon;
	private Animator enemyAnimation;
	private NodeController startInfo;
	private enum EnemyState{ ALIVE, DYING, DEAD }
	private EnemyState state;

	private float timeInAir = 0.0f;
	private float deathTimer = 5.0f;

	void Start(){
		state = EnemyState.ALIVE;
	}

	void Awake () {

		getOriginalColors ();

		waveController = GameObject.FindObjectOfType<WaveController> ();
		scoreController = GameObject.FindObjectOfType<GameController> ();
		this.enemy = this.GetComponent<CharacterController> ();
		this.enemyAnimation = this.GetComponent<Animator> ();
		this.enemyAnimation.speed = enemyAnimationSpeed;

		if (enemyType == "Soldier") {
			enemyWeapon = this.GetComponent<FireEnemyWeapon> ();
		}

		modEnemyStats ();

 	}

	void Update(){

		//Monitors if the enemy is alive, dead or dying with an enumerator.
		//Also kills if the enemy if it falls off the map by checking how long it has not been grounded.

		switch(state){

			case EnemyState.ALIVE:

				applyGravity ();

				if (!enemy.isGrounded) {
					timeInAir += Time.deltaTime;
					if (timeInAir >= deathTimer) {
						damageTaken (enemyHealth);
					}
				} else {
					timeInAir = 0.0f;
				}

				if (enemyHealth <= 0) {
					waveController.reduceEnemyCount ();
					scoreController.setScoreText (enemyScore);
					state = EnemyState.DYING;
				}
				break;

			case EnemyState.DYING:
			
				this.enemyAnimation.speed = 1.5f;
				if (enemyType == "Zombie") {
					enemyAnimation.Play ("fallingback");
					//Turns off the colliders on the body when the zombie is in its death animation.
					Collider[] colChildren = this.GetComponentsInChildren<Collider>();
					foreach (Collider col in colChildren) {
						col.enabled = false;
					}
				} else {
					enemyAnimation.Play ("idle");
					this.enemyWeapon.isFiring = false;
					this.transform.rotation = Quaternion.Euler(this.transform.rotation.z, this.transform.rotation.y, -90);
				}
				state = EnemyState.DEAD;
				break;

			case EnemyState.DEAD:
				Destroy (this.gameObject, 2.0f);
				break;
		}

	}
		

	//Returns the enemy health.
	public int getEnemyHealth(){
		return enemyHealth;
	}

	//Subtracts damage to the enemy based on the projectile they recieved.
	public void damageTaken(int damage){
		enemyHealth = enemyHealth - damage;
		flashRed ();
	}

	//Stores the original color of the model for the reset.
	private void getOriginalColors(){

		renderers = this.GetComponentsInChildren<SkinnedMeshRenderer> ();
		foreach(SkinnedMeshRenderer rend in renderers){
				originalColor.Add(rend.material.color);
		}
		originalColor.Reverse ();

	}

	//Flashes the enemy's body red when hit and then resets if it is still alvive to its original color shortly after.
	private void flashRed(){

		Color flashColor;

		if (enemyColor == "red") {
			flashColor = Color.blue;
		} else {
			flashColor = Color.red;
		}

		foreach(SkinnedMeshRenderer rend in renderers){
			rend.material.color = flashColor;
		}
		if (state == EnemyState.ALIVE) {
			Invoke ("ResetColor", flashTime);
		}
	}

	//Resets the colors on the enemy's model to its original form.
	private void ResetColor(){
		foreach(SkinnedMeshRenderer rend in renderers){
			try{
				rend.material.color = (Color) originalColor [0];
				originalColor.RemoveAt (0);
			} catch{
			}
		}
	}
		
	//Turns the enemy towards the direction of the next node or target
	public void enemyRotation (Vector3 nodePos){

		Vector3 direction = nodePos - this.enemy.transform.position;
		direction.y = 0;
		Quaternion toRotation = Quaternion.LookRotation (direction);
		this.enemy.transform.rotation = Quaternion.Lerp (this.enemy.transform.rotation, toRotation, enemyRotateSpeed * Time.deltaTime);

	}

	//Moves the enemy to the next target/node
	public void enemyMovement(Vector3 nodePos, float angle){

		Vector3 direction = nodePos - this.enemy.transform.position;
		direction.y = 0;
		if (direction.magnitude > .1f && Vector3.Angle(this.enemy.transform.forward, direction) < angle) {
			Vector3 dirOffset = direction.normalized * enemySpeed;
			dirOffset.y = vertSpeed;

			collisionFlag = this.enemy.Move (dirOffset * Time.deltaTime);

			if ((dirOffset.x != 0 || dirOffset.z != -0) && OnSlope (false)) {
				collisionFlag = this.enemy.Move (Vector3.down * this.enemy.height / 2 * slopeForce * Time.fixedDeltaTime);
			}
		}
	}
		
	//Function responsible for applying gravity to the enemy model.
	private void applyGravity(){
		if (isGrounded ()) {
			vertSpeed = 0.0f;
		} else {
			vertSpeed -= gravity * Time.deltaTime;
		}
	}

	private bool isGrounded(){
		return (this.collisionFlag & CollisionFlags.Below) != 0;
	}

	private bool OnSlope(bool isJump){
		//Function for checking if the enemy is moving on a slope.

		if (isJump) {
			return false;
		}

		RaycastHit hit;

		if (Physics.Raycast (this.transform.position, Vector3.down, out hit, this.enemy.height / 2 * slopeForceRayLength)) {
			if (hit.normal != Vector3.up) {
				return true;
			}
		}
		return false;

	}

	//Function responsible for scaling up the enemies damage and health.
	private void modEnemyStats(){

		if (waveController.waveNumber > 2) {
			this.enemyDamage *= waveController.waveNumber/2;
			this.enemyHealth *= waveController.waveNumber/2;
		}
	}
		
}
