using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemyWeapon : MonoBehaviour {

	/* Class responsible for firing the soldiers weapon if a boolean is triggered from its behavior tree.
	 * Fires the bullets in a timed interval rather than constantly. */

	public Transform bulletSpawn;
	public GameObject bulletPrefab;
	public float bulletSpeed;
	public bool isFiring;
	private bool firstFire;

	private float timer = 0.0f;
	private int waitTime = 1;
	private Vector3 playerPos;
	private EnemyController enemy;
	private int bulletDamage;

	// Use this for initialization
	void Start () {
		isFiring = false;
		firstFire = true;
		enemy = this.GetComponent<EnemyController> ();
		bulletDamage = enemy.enemyDamage;
	}
	
	// Update is called once per frame
	void Update () {
		if (isFiring) {

			playerPos = GameObject.FindGameObjectWithTag ("Player").transform.position;

			if (firstFire) {
				fire ();
				firstFire = false;
			}
			timer += Time.deltaTime;
			if (timer > waitTime) {
				fire ();
				timer = 0;
			}
		}
	}

	private void fire(){
		//Function for creating and firing bullets based on its rigidbody's velocity.

		GameObject bullet = Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
		EnemyBulletController bControl = bullet.GetComponent<EnemyBulletController> ();
		bControl.enemyDamage = bulletDamage;
		bullet.GetComponent<Rigidbody> ().velocity = (playerPos - bullet.transform.position).normalized * bulletSpeed;
		Destroy (bullet, 5.0f);
	}

		
}
