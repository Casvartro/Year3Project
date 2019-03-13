using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemyWeapon : MonoBehaviour {

	public Transform bulletSpawn;
	public GameObject bulletPrefab;
	public float bulletSpeed;
	public bool isFiring;
	private bool firstFire;

	private float timer = 0.0f;
	private int waitTime = 1;
	private Vector3 playerPos;

	// Use this for initialization
	void Start () {
		isFiring = false;
		firstFire = true;
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

		var bullet = Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
		bullet.GetComponent<Rigidbody> ().velocity = (playerPos - bullet.transform.position).normalized * bulletSpeed;
		Destroy (bullet, 5.0f);
	}

		
}
