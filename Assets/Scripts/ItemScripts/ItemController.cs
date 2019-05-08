using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

	/* Class responsible for the indvidual items values and collision detection.
	 	Checks to see if the collider is the player.
		If it is a health item then it checks to see if player is at max health to restore health.
		If not the player passes through without destroing.
		Same works for the ammo items but with the players current ammo.
		Power Up functionality destroys the object if in contact with the player and sets the status in the UI component.*/
	
	public int healthValue = 25;
	public int ammoValue = 10;
	public float powerTime = 30.0f;
	public float rotateSpeed = 25;
	public string itemColor;

	private int currentAmmo;
	private int playerHealth;

	void Update(){
		this.transform.Rotate (Vector3.up, rotateSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider col){
		
		if (col.gameObject.CompareTag ("Player")) {
			if (this.gameObject.CompareTag ("healthItem")) {
				
				PlayerController playerController = col.GetComponent<PlayerController> ();
				playerHealth = playerController.getPlayerHealth ();
				if (playerHealth < playerController.maxHealth) {
					playerController.setPlayerHealth (healthValue);
					Destroy (this.gameObject);
				} else {
					Physics.IgnoreCollision (col, GetComponent<Collider> ());			
				}

			} else if (this.gameObject.CompareTag ("ammoItem")) {
				
				WeaponController weaponController = GameObject.Find ("Weapon").GetComponent<WeaponController> ();
				currentAmmo = weaponController.getCurrentAmmo ();
				if (currentAmmo < weaponController.maxAmmo) {
					weaponController.setCurrentAmmo (ammoValue);
					Destroy (this.gameObject);
				} else {
					Physics.IgnoreCollision (col, GetComponent<Collider> ());	
				}

			} else if(this.gameObject.CompareTag("powerItem")){
				
				PlayerController playerController = col.GetComponent<PlayerController> ();
				playerController.setPowerUpBar (this.gameObject, powerTime, itemColor);
				Destroy (this.gameObject);

			}

		}

	}

}
