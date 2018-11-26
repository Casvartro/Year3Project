using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed;
	public int maxAmmo;
	public Text weaponNameText;
	public Text ammoCountText;

	private int currentAmmo;
	private string weaponName = "Test Weapon";

	// Use this for initialization
	void Start () {
		currentAmmo = maxAmmo;
		weaponNameText.text = weaponName;
		ammoCountText.text = currentAmmo + " / " + maxAmmo;
	}

	void LateUpdate () {
		if (currentAmmo != 0) {
			if (Input.GetMouseButtonDown (0)) {
				fire ();
				currentAmmo = currentAmmo - 1;
				ammoCountText.text = currentAmmo + " / " + maxAmmo;
			}
		}
	}

	void fire(){

		var bullet = Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
		bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * bulletSpeed;
		Destroy (bullet, 5.0f);
	}
}
