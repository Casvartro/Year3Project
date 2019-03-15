using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats {

	private static string scoreText, timerText;
	private static float shotsFired, shotsHitTarget;

	public static string getScoreText(){
		return scoreText;
	}

	public static void setScoreText(string value){
		scoreText = value;
	}

	public static string getTimerText(){
		return timerText;
	}

	public static void setTimerText(string value){
		timerText = value;
	}

	public static float getShotsFired(){
		return shotsFired;
	}

	public static void setShotsFired(float value){
		shotsFired = value;
	}

	public static float getShotsHitTarget(){
		return shotsHitTarget;
	}

	public static void setShotsHitTarget(float value){
		shotsHitTarget = value;
	}
}
