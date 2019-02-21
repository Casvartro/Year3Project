using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeOnClick : MonoBehaviour {

	public void resumeGame(){

		Canvas pauseCanvas = GameObject.Find ("PauseCanvas").GetComponent<Canvas> ();
		pauseCanvas.enabled = false;
		Time.timeScale = 1.0f;

	}

}
