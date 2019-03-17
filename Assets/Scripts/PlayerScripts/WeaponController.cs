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
	public Camera playerCamera;
	public GameObject crosshair;

	private int currentAmmo;
	private string weaponName = "Test Weapon";
	private GameController gameStatus;
	private PlayerController player;
	private AudioSource shotAudio;
	private float startVolume;

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
	}

	void Update(){

		startVolume = soundSettings.sSlider.value;

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

		Vector3 crosshairPos = new Vector3 (crosshair.transform.position.x, 
			crosshair.transform.position.y + 2.0f, crosshair.transform.position.z);
		Ray ray = playerCamera.ScreenPointToRay (crosshairPos);

		var bullet = Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
		Rigidbody bulletRigidBody = bullet.GetComponent<Rigidbody> ();

		player.addShotFired ();
		playGunAudio ();

		Vector3 direction = (ray.GetPoint(100000.0f) - bullet.transform.position).normalized;
		bulletRigidBody.AddForce(direction * bulletSpeed, ForceMode.Impulse);
		//bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * bulletSpeed;
		Destroy (bullet, 5.0f);

	}

	private void playGunAudio(){

		shotAudio.volume = startVolume;
		shotAudio.Play ();
		StartCoroutine (VolumeFade (shotAudio, 0, 0.5f));

	}

	IEnumerator VolumeFade(AudioSource _AudioSource, float _EndVolume, float _FadeLength)
	{
		float _StartTime = Time.time;
		while (Time.time < _StartTime + _FadeLength){
			float alpha = (_StartTime + _FadeLength - Time.time) / _FadeLength;
			// use the square here so that we fade faster and without popping
			alpha = alpha * alpha; 
			_AudioSource.volume = alpha * startVolume + _EndVolume * (1.0f - alpha);
			yield return null;
		}
		if (_EndVolume == 0) { _AudioSource.UnPause(); }
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
