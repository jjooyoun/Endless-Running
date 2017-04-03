using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMusic : MonoBehaviour {

	public float time = 5; //Seconds to read the text
	public AudioSource audioSource;

	public void Init(){
		//Debug.Log (entityName + ":init");
		audioSource = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	IEnumerator Start () {
		Init();
		yield return new WaitForSeconds(time);
		//Destroy(gameObject);
		//audioSource = GetComponent<AudioSource> ();
		audioSource.Play();

		//audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
