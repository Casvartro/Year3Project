using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeOnClick : MonoBehaviour {

	//Function responsible for resuming the game, unfreezing the time and disabling the pause canvas when resume is clicked.
	public void resumeGame(){

		GameObject pauseCanvas = GameObject.Find ("PauseCanvas");
		pauseCanvas.SetActive (false);
		Time.timeScale = 1.0f;

	}

}
