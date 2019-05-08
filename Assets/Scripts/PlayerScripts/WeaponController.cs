using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour {

	/* Class responsible for managing the values and functions of the player's weapon.
	 * Handles the shooting and melee attacks, the gun shot audio as well as the notification that 
	 * alerts enemies that the player has fired their weapon if nearby. */

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed;
	public int maxAmmo;
	public Text weaponNameText;
	public Text ammoCountText;
	public Camera playerCamera;
	public GameObject crosshair;

	private int currentAmmo;
	private string weaponName = "Test Weapon";
	private GameController gameStatus;
	private PlayerController player;

	private AudioSource shotAudio;
	private float startVolume;

	private bool melee;
	private Collider meleeCollider;


	private SettingsMenu soundSettings;

	// Use this for initialization
	void Start () {

		currentAmmo = maxAmmo;
		weaponNameText.text = weaponName;
		ammoCountText.text = currentAmmo + " / " + maxAmmo;

		gameStatus = GameObject.Find ("UICanvas").GetComponent<GameController> ();
		player = GameObject.Find ("Player").GetComponent<PlayerController> ();

		shotAudio = this.GetComponent<AudioSource> ();
		startVolume = shotAudio.volume;
		soundSettings = GameObject.Find ("UICanvas").GetComponent<SettingsMenu> ();

		melee = false;
		meleeCollider = this.GetComponent<BoxCollider> ();

	}

	void Update(){

		startVolume = soundSettings.sSlider.value;

	}

	void LateUpdate () {
		if (!gameStatus.checkPause ()) {
			if (currentAmmo != 0 && Input.GetMouseButtonDown (0) && !melee) {
				fire ();
				currentAmmo = currentAmmo - 1;
				ammoCountText.text = currentAmmo + " / " + maxAmmo;
			}

			if (Input.GetMouseButtonDown (1) && !melee) {
				StartCoroutine(meleeAttack ());
			}
		}
	}

	//Function ran in a coroutine that changes the transform of the weapon to act as a melee attack.
	private IEnumerator meleeAttack(){
		melee = true;
		this.transform.localEulerAngles = new Vector3 (Mathf.Round(-2.0f), Mathf.Round(-45.0f), Mathf.Round(1.0f));
		this.transform.localPosition = new Vector3 (0.2f, -0.3f, 1.0f);
		this.transform.localScale = new Vector3 (0.15f, 0.2f, 1.4f);
		meleeCollider.enabled = true;
		yield return new WaitForSeconds (0.4f);
		this.transform.localEulerAngles = new Vector3 (Mathf.Round(-2.0f), Mathf.Round(-3.0f), Mathf.Round(1.0f));
		this.transform.localPosition = new Vector3 (0.2f, -0.3f, 0.6f);
		this.transform.localScale = new Vector3 (0.15f, 0.2f, 0.5f);
		meleeCollider.enabled = false;
		melee = false;
	}

	private void fire(){
		//Function for creating and firing bullets based on its rigidbody's velocity.

		Vector3 crosshairPos = new Vector3 (crosshair.transform.position.x, 
			crosshair.transform.position.y + 2.0f, crosshair.transform.position.z);
		Ray ray = playerCamera.ScreenPointToRay (crosshairPos);

		var bullet = Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
		Rigidbody bulletRigidBody = bullet.GetComponent<Rigidbody> ();

		player.addShotFired ();
		playGunAudio ();
		informEnemyOfAudio (player.transform.position, 50);

		Vector3 direction = (ray.GetPoint(100000.0f) - bullet.transform.position).normalized;
		bulletRigidBody.AddForce(direction * bulletSpeed, ForceMode.Impulse);
		//bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * bulletSpeed;
		Destroy (bullet, 5.0f);

	}

	//Function that plays the gun shot audio clip and calls a coroutine that fades the volume down from 100 to 0 rather than instantly.
	private void playGunAudio(){

		shotAudio.volume = startVolume;
		shotAudio.Play ();
		StartCoroutine (VolumeFade (shotAudio, 0, 0.5f));

	}

	//Function found online that handles the fade out of the volume.
	//community.gamedev.tv/t/audio-popping-on-call-to-stop/48825/2
	IEnumerator VolumeFade(AudioSource _AudioSource, float _EndVolume, float _FadeLength)
	{
		float _StartTime = Time.time;
		while (Time.time < _StartTime + _FadeLength){
			float alpha = (_StartTime + _FadeLength - Time.time) / _FadeLength;
			alpha = alpha * alpha; 
			_AudioSource.volume = alpha * startVolume + _EndVolume * (1.0f - alpha);
			yield return null;
		}
		if (_EndVolume == 0) { _AudioSource.UnPause(); }
	}

	//Function that uses OverlapSphere to alert any enemies nearby of the gunshot audio.
	private void informEnemyOfAudio(Vector3 center, float radius){
		
		Collider[] hitColliders = Physics.OverlapSphere(center, radius);
		int i = 0;
		while (i < hitColliders.Length){
			if (hitColliders [i].tag == "enemy") {
				EnemyController enemy = hitColliders [i].gameObject.GetComponent<EnemyController> ();
				if (enemy.enemyType == "Zombie") {
					ZombieBehaviourTree enemyBehaviour = hitColliders [i].gameObject.GetComponent<ZombieBehaviourTree> ();
					enemyBehaviour.behaviourState.playerHeard = true;
					enemyBehaviour.behaviourState.playerRecentShot = true;
				} else if (enemy.enemyType == "Soldier") {
					SoldierBehaviourTree enemyBehaviour = hitColliders [i].gameObject.GetComponent<SoldierBehaviourTree> ();
					enemyBehaviour.behaviourState.playerHeard = true;
					enemyBehaviour.behaviourState.playerRecentShot = true;
				}
			}
			i++;
		}
	
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
