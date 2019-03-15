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
	private GameController gameStatus;
	private PlayerController player;

	// Use this for initialization
	void Start () {
		currentAmmo = maxAmmo;
		weaponNameText.text = weaponName;
		ammoCountText.text = currentAmmo + " / " + maxAmmo;
		gameStatus = GameObject.Find ("UICanvas").GetComponent<GameController> ();
		player = GameObject.Find ("Player").GetComponent<PlayerController> ();
	}

	void LateUpdate () {
		if (!gameStatus.checkPause ()) {
			if (currentAmmo != 0 && Input.GetMouseButtonDown (0)) {
				fire ();
				currentAmmo = currentAmmo - 1;
				ammoCountText.text = currentAmmo + " / " + maxAmmo;
			}
		}
	}

	private void fire(){
		//Function for creating and firing bullets based on its rigidbody's velocity.

		var bullet = Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
		player.addShotFired ();
		bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * bulletSpeed;
		Destroy (bullet, 5.0f);
	}

	public int getCurrentAmmo(){
		//Returns the current ammo of the weapon.

		return currentAmmo;
	}

	public void setCurrentAmmo(int ammoIncrease){
		//Adds to the current ammo based on the ammo item making sure it does not go past the max.

		currentAmmo = currentAmmo + ammoIncrease;
		if (currentAmmo > maxAmmo) {
			currentAmmo = maxAmmo;
		}
		ammoCountText.text = currentAmmo + " / " + maxAmmo;
	}
}
