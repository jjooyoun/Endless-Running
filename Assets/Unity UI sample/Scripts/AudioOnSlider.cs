using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOnSlider : MonoBehaviour {
	public Slider slider;
	public AudioSource music;
	private float prevVolume;
	// Use this for initialization
	void Start () {
		prevVolume = music.volume;
	}

	// Update is called once per frame
	void Update () {
		Debug.Log ("volume:" + music.volume);
	}

	public void OnVaueChanged(){
		Debug.Log ("value changed to whatever" + slider.value);

		music.volume = slider.value;
		//prevVolume = music.volume;
	}
}
