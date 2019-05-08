using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameOnClick : MonoBehaviour {

	//Loads scene passed through on click.
	public void LoadByIndex(int sceneIndex){
		SceneManager.LoadScene (sceneIndex);
	}

}
