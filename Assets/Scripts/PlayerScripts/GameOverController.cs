using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameOverController : MonoBehaviour {

	public Text timeText;
	public Text accuracyText;
	public Text scoreText;
	public Text rankText;

	private float accuracy;
	private string time;
	private int score;

	// Use this for initialization
	void Start () {

		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		time = PlayerStats.getTimerText ();
		score = int.Parse(PlayerStats.getScoreText ());
		calculateAccuracy ();
		timeText.text = "Time : " + time;
		accuracyText.text = "Accuracy : " + accuracy.ToString() + "%";
		scoreText.text = "Score : " + score;

		rankText.text = calculateRank ();

	}

	private void calculateAccuracy(){
		float shotsFired = PlayerStats.getShotsFired ();
		float shotsHitTarget = PlayerStats.getShotsHitTarget ();

		if (shotsFired == 0) {
			accuracy = 0.0f;
		} else {
			accuracy = Mathf.Round ((shotsHitTarget / shotsFired) * 100);
		}
	}

	private string calculateRank(){

		int rankValue = 0;

		if (score < 100) {
			rankValue += 1;
		} else if (score >= 100 && score < 1000) {
			rankValue += 2;
		} else if (score >= 1000 & score < 5000) {
			rankValue += 3;
		} else if (score >= 5000 & score < 10000) {
			rankValue += 4;
		} else if (score >= 10000 & score < 20000) {
			rankValue += 5;
		} else if (score >= 20000) {
			rankValue += 6;
		}

		if (accuracy < 10.0f) {
			rankValue += 1;
		} else if (accuracy >= 10.0f && accuracy < 25.0f) {
			rankValue += 2;
		} else if (accuracy >= 25.0f & accuracy < 40.0f) {
			rankValue += 3;
		} else if (accuracy >= 40.0f & accuracy < 55.0f) {
			rankValue += 4;
		} else if (accuracy >= 55.0f & accuracy < 70.0f) {
			rankValue += 5;
		} else if (accuracy >= 70.0f) {
			rankValue += 6;
		}

		if (rankValue <= 2) {
			return "F";
		} else if (rankValue > 2 && rankValue <= 4) {
			return "D";
		} else if (rankValue > 4 && rankValue <= 6) {
			return "C";
		} else if (rankValue > 6 && rankValue <= 8) {
			return "B";
		} else if (rankValue > 8 && rankValue <= 10) {
			return "A";
		} else if (rankValue > 10) {
			return "S";
		}

		return "";
	}

}
