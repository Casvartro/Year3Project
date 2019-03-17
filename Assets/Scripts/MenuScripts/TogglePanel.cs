using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePanel : MonoBehaviour {

	public void togglePanelByObject(GameObject panel){

		if (panel.activeSelf) {
			panel.SetActive (false);
		} else {
			panel.SetActive (true);
		}
	}
}
