using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

	public Text timerLabelText;

	private float gameTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		gameTime += Time.deltaTime;
		int hours = (int)(gameTime / 3600) % 24;
		int minutes = (int)(gameTime / 60) % 60;
		int seconds = (int)(gameTime % 60);
		timerLabelText.text = string.Format ("{0:0} : {1:00} : {2:00}", hours, minutes, seconds);

	}
}
