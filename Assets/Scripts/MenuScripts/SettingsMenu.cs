using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

	public Slider mSlider;
	public AudioSource mSource;
	public Text mText;
	public Slider sSlider;
	public AudioSource sSource;
	public Text sText;

	private float wantedMusicVolume;
	private float wantedSoundFXVolume;

	void Awake () {
		
		if (mSlider != null && PlayerPrefs.HasKey ("musicVolume")) {
			wantedMusicVolume = PlayerPrefs.GetFloat ("musicVolume");
			mSlider.value = wantedMusicVolume;
			mSource.volume = wantedMusicVolume;
			mText.text = Mathf.Round(mSlider.value * 100).ToString();
		} else {
			mSlider.value = mSource.volume;
			mText.text =  Mathf.Round(mSlider.value * 100).ToString ();
		}

		if (sSlider != null && PlayerPrefs.HasKey ("soundFXVolume")) {
			wantedSoundFXVolume = PlayerPrefs.GetFloat ("soundFXVolume");
			sSlider.value = wantedSoundFXVolume;
			sText.text =  Mathf.Round(sSlider.value * 100).ToString ();
			if (sSource != null) {
				sSource.volume = wantedSoundFXVolume;
			}
		} else {
			if (sSource != null) {
				sSlider.value = sSource.volume;
				sText.text = Mathf.Round(sSlider.value * 100).ToString ();
			}
		}

		mSlider.onValueChanged.AddListener (delegate {
			setMusicVolume (mSlider.value);
		});

		sSlider.onValueChanged.AddListener (delegate {
			setSoundFXVolume (sSlider.value);
		});
	}

	public void setMusicVolume(float volume){
		mSource.volume = volume;
		mText.text = Mathf.Round(volume * 100).ToString ();
		PlayerPrefs.SetFloat ("musicVolume", volume);
	}

	public void setSoundFXVolume(float volume){
		sText.text = Mathf.Round(volume * 100).ToString ();
		if (sSource != null) {
			sSource.volume = volume;
		}
		PlayerPrefs.SetFloat ("soundFXVolume", volume);
	}

}
