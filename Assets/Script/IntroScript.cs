using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour {

	public float time = 1; //Seconds to read the text
	//public AudioSource audioSource;

	public Text intro;
	public Text title;
	public GameObject summary;
	private Bounds summaryTextBound;
	private float screenY;
	private float extentY;
	public float speedUp = 0.3f;
	// Use this for initialization
	IEnumerator Start () {
		screenY = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f)).y;
		title.enabled = false;
		//summary.SetActive(false);
		yield return new WaitForSeconds(3);
		intro.enabled = false;
		title.enabled = true;
		yield return new WaitForSeconds(3);
		title.enabled = false;
		yield return new WaitForSeconds(time);
		//summary.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		summary.transform.Translate(0, speedUp, 0);
	}


	void DoActivateTrigger(){
		SceneManager.LoadScene (0);
	}
}
