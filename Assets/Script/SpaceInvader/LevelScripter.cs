using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handle different level event */
public class LevelScripter : MonoBehaviour {
	public AudioClip mainMenuClip;
	public AudioClip bossClip;
	
	public AudioClip WinClip;
	public AudioClip DieClip;

	public AudioClip[] RandomGameLevels;

	private AudioSource audioSource;

	void Start(){
		audioSource = GetComponent<AudioSource>();
		PlayMainMenuMusic();
	}

	public void PlayMainMenuMusic(){
		PlayClip(audioSource, mainMenuClip, true);
	}

	public void PlayBossClip(){
		PlayClip(audioSource, bossClip, true);
	}

	public void PlayWinClip(){
		PlayClip(audioSource, WinClip, true);
	}

	public void PlayDieClip(){
		PlayClip(audioSource, DieClip, true);
	}

	public void PlayGameClip(){
		PlayClip(audioSource, RandomGameLevels[Random.Range(0,RandomGameLevels.Length - 1)],true);
	}

	

	public void StartGame(){
		//simply invoke the event
		Debug.Log("StartGame!!!");
		EventDispatcher.Instance.GameStartEvent.Invoke();
	}

	public void QuitGame(){
		Debug.Log("Quit Game!!!");
		#if UNITY_EDITOR
        	UnityEditor.EditorApplication.isPlaying = false;
		#else
        	Application.Quit ();
		#endif
	}
	

	void PlayClip(AudioSource audioSource, AudioClip clip, bool loop = false, float volume = 1.0f){
		if(audioSource && clip){
			audioSource.clip = clip;
			audioSource.volume = volume;
			audioSource.loop = loop;
			audioSource.Play();
		}
	}

}
