using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour {

	public float time = 1; //Seconds to read the text
	//public AudioSource audioSource;

	public Text intro;
	public Text title;
	public GameObject summary;




	// Use this for initialization
	IEnumerator Start () {
		
		title.enabled = false;
		summary.SetActive(false);
		yield return new WaitForSeconds(3);
		intro.enabled = false;
		title.enabled = true;
		yield return new WaitForSeconds(3);
		title.enabled = false;
		yield return new WaitForSeconds(time);
		summary.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		summary.transform.Translate(0, 0.005f, 0);
		
	}
}
