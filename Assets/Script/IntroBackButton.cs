using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//this needs to be on the button
public class IntroBackButton : MonoBehaviour {

	public float time = 1.0f;
	private bool isOnFade = false;
	public GameObject backButton;

	// Use this for initialization

	IEnumerator DisappearButtonAfter(float sec){
		Debug.Log ("here?");
		isOnFade = true;
		backButton.SetActive(true);
		yield return new WaitForSeconds (sec);
		Debug.Log ("after wait");
		isOnFade = false;
		backButton.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && !isOnFade) {
			Debug.Log ("mouse click");

			StartCoroutine( DisappearButtonAfter (time));
		}
	}
}
