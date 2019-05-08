using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sense : MonoBehaviour {

	//Base parent script used by the Sight script.

	public bool enableDebug = true;
	public float detectionRate = 1.0f;

	protected float elapsedTime = 0.0f;

	protected virtual void Initialize(){}
	protected virtual bool UpdateSense(){return false;}

	// Use this for initialization
	void Awake () {
		elapsedTime = 0.0f;
		Initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateSense ();
	}
}
