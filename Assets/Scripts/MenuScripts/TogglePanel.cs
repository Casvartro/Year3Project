using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePanel : MonoBehaviour {

	//Function that toggles the panel on and off based on object interaction.
	public void togglePanelByObject(GameObject panel){

		if (panel.activeSelf) {
			panel.SetActive (false);
		} else {
			panel.SetActive (true);
		}
	}
}
