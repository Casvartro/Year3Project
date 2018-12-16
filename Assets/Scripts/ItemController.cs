using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

	public GameObject healthCratePrefab;
	public GameObject ammoCratePrefab;
	 
	// Use this for initialization
	void Start () {
		spawnItems ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	private void spawnItems(){

		GameObject[] itemArray = GameObject.FindGameObjectsWithTag("Item");;
		foreach (GameObject item in itemArray) {
			Debug.Log (item);
		}
	}

}
