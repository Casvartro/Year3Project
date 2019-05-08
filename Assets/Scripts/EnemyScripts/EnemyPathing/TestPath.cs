using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPath : MonoBehaviour {

	//Class just used to test if the PathFinder getPath actually worked.

	public GameObject startNode;
	public GameObject endNode;


	// Use this for initialization
	void Start () {
		ArrayList returnedPath = PathFinder.getPath (startNode, endNode);
		for (int i = 0; i < returnedPath.Count; i++) {
			Debug.Log (returnedPath [i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
