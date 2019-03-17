using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

	public Slider mSlider;
	public AudioSource mSource;
	public Slider sSlider;
	public AudioSource sSource;

	private float wantedMusicVolume;
	private float wantedSoundFXVolume;

	void Awake () {
		
		if (mSlider != null && PlayerPrefs.HasKey ("musicVolume")) {
			wantedMusicVolume = PlayerPrefs.GetFloat ("musicVolume");
			mSlider.value = wantedMusicVolume;
			mSource.volume = wantedMusicVolume;
		} else {
			mSlider.value = mSource.volume;
		}

		if (sSlider != null && PlayerPrefs.HasKey ("soundFXVolume")) {
			wantedSoundFXVolume = PlayerPrefs.GetFloat ("soundFXVolume");
			sSlider.value = wantedSoundFXVolume;
			if (sSource != null) {
				sSource.volume = wantedSoundFXVolume;
			}
		} else {
			if (sSource != null) {
				sSlider.value = sSource.volume;
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
		PlayerPrefs.SetFloat ("musicVolume", volume);
	}

	public void setSoundFXVolume(float volume){
		if (sSource != null) {
			sSource.volume = volume;
		}
		PlayerPrefs.SetFloat ("soundFXVolume", volume);
	}

}
