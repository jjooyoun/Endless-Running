using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOnSlider : MonoBehaviour {
	public Slider slider;
	public AudioSource music;

	public void OnVaueChanged(){
		Debug.Log ("value changed to whatever" + slider.value);

		music.volume = slider.value;
		Setting.gameSetting.soundLevel = music.volume;
	}
}
