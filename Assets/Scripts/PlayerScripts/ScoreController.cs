using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

	public Text timerLabelText;
	public Text waveNumberText;
	public Text enemyCountText;
	public Text totalScoreText;

	private float gameTime;
	private WaveController waveController;
	private int totalScore;

	// Use this for initialization
	void Awake () {
		waveController = GameObject.FindObjectOfType<WaveController> ();
	}
	
	// Update is called once per frame
	void Update () {

		gameTimer ();
		waveNumberText.text = waveController.getWaveNumber ().ToString ();
		enemyCountText.text = waveController.getEnemyCount ().ToString ();
		totalScoreText.text = totalScore.ToString ();

	}

	public void setScoreText(int enemyScore){
		totalScore = totalScore + enemyScore;
	}

	//Function used for maintaining the game's timer.
	void gameTimer(){
		gameTime += Time.deltaTime;
		int hours = (int)(gameTime / 3600) % 24;
		int minutes = (int)(gameTime / 60) % 60;
		int seconds = (int)(gameTime % 60);
		timerLabelText.text = string.Format ("{0:0} : {1:00} : {2:00}", hours, minutes, seconds);
	}
}
