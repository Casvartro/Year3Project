using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public int enemyScore = 100;
	public float enemySpeed = 5;
	public float enemyRotateSpeed = 5f;
	public float enemyAnimationSpeed = 2.0f;

	private int enemyHealth = 100;
	private float slopeForce = 5;
	private float slopeForceRayLength = 1.5f;

	private WaveController waveController;
	private GameController scoreController;
	private CharacterController enemy;
	private Animator enemyAnimation;
	private NodeController startInfo;
	private enum EnemyState{ ALIVE, DYING, DEAD }
	private EnemyState state;

	void Start(){
		state = EnemyState.ALIVE;
	}

	void Awake () {
		
		waveController = GameObject.FindObjectOfType<WaveController> ();
		scoreController = GameObject.FindObjectOfType<GameController> ();
		this.enemy = this.GetComponent<CharacterController> ();
		this.enemyAnimation = this.GetComponent<Animator> ();
		this.enemyAnimation.speed = enemyAnimationSpeed;
 	}

	void Update(){

		switch(state){

			case EnemyState.ALIVE:
				if (enemyHealth <= 0) {
					waveController.reduceEnemyCount ();
					scoreController.setScoreText (enemyScore);
					state = EnemyState.DYING;
				}
				break;

			case EnemyState.DYING:
				enemyAnimation.Play ("fallingback");
				state = EnemyState.DEAD;
				break;

			case EnemyState.DEAD:
				Destroy (this.gameObject, 5.0f);
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
	}

	//Returns the closest node to the enemy, the start of their path.
	public GameObject getInitialNode(GameObject[] pathNodes){

		GameObject closestNode = null;
		float closestDistance = float.PositiveInfinity;

		foreach (GameObject node in pathNodes) {
			float currentDistance = Vector3.Distance (node.transform.position, this.transform.position);
			if (currentDistance < closestDistance) {
				closestNode = node;
				closestDistance = currentDistance;
			}
		}
		return closestNode;

	}

	//Retrieves the end node of their path the destination.
	public GameObject getEndNode(GameObject[] pathNodes, GameObject startNode){

		GameObject endNode = null;
		bool nodeFound = false;

		while (!nodeFound) {
			endNode = pathNodes [Random.Range (0, pathNodes.Length)];
			if (startNode != endNode) {
				nodeFound = true;
			}
		}

		return endNode;

	}

	//Turns the enemy towards the direction of the next node or target
	public void enemyRotation (Vector3 nodePos){

		Vector3 direction = nodePos - this.enemy.transform.position;
		direction.y = 0;
		Quaternion toRotation = Quaternion.LookRotation (direction);
		this.enemy.transform.rotation = Quaternion.Lerp (this.enemy.transform.rotation, toRotation, enemyRotateSpeed * Time.deltaTime);

	}

	//Moves the enemy to the next target/node
	public void enemyMovement(Vector3 nodePos){

		float angle = 10;
		Vector3 direction = nodePos - this.enemy.transform.position;
		direction.y = 0;
		if (direction.magnitude > .1f && Vector3.Angle(this.enemy.transform.forward, direction) < angle) {
			Vector3 dirOffset = direction.normalized * enemySpeed;

			this.enemy.Move (dirOffset * Time.deltaTime);

			if ((dirOffset.x != 0 || dirOffset.z != -0) && OnSlope (false)) {
				this.enemy.Move (Vector3.down * this.enemy.height / 2 * slopeForce * Time.fixedDeltaTime);
			}
		}
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
}
