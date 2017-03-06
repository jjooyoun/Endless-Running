using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour {

	public AudioClip shieldEffect;
	public AudioClip snowEffect;
	public AudioClip loseEffect;

	private AudioSource source;

	// Use this for initialization
	void Start () {
		EventManager.instance.shield.AddListener(shieldSound);

		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void shieldSound()
	{
		source.PlayOneShot(shieldEffect, 1.0f);
	}


}
