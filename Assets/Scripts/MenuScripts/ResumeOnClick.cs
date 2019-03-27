using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeOnClick : MonoBehaviour {

	public void resumeGame(){

		GameObject pauseCanvas = GameObject.Find ("PauseCanvas");
		pauseCanvas.SetActive (false);
		Time.timeScale = 1.0f;

	}

}
