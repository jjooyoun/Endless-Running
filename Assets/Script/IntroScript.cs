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

	public GameObject bossIntro;

	private Bounds summaryTextBound;
	private float screenY;
	private float extentY;
	public float speedUp = 0.5f;
	public float deltaSpeed = 0.5f;
	//public float normalSpeed = 0.3f;
	private bool startSummary = false;


	private Vector3 touchPosition;
	private float swipeResistanceX = 50.0f;
	private float swipeResistanceY = 50.0f;


	/// <summary>
	/// BACK BUTTON VARIABLE AREA
	/// </summary>
	public float timeTilButtonDisappear = 1.0f;
	private bool isOnFade = false;
	public GameObject backButton;

	IEnumerator DisappearButtonAfter(float sec){
		Debug.Log ("here?");
		isOnFade = true;
		backButton.SetActive(true);
		yield return new WaitForSeconds (sec);
		Debug.Log ("after wait");
		isOnFade = false;
		backButton.SetActive (false);
	}
	/// <summary>
	/// BACK BUTTON VARIABLE AREA
	/// </summary>

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
		startSummary = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (startSummary == true) {
			summary.transform.Translate (0, speedUp * Time.deltaTime, 0);
			bossIntro.transform.Translate(0, (speedUp + 5.0f) * Time.deltaTime, 0);
		}

		if (Input.GetMouseButtonDown (0) && !isOnFade) {
			//Debug.Log ("mouse click");
			//speedUp*=2.0f;

			StartCoroutine( DisappearButtonAfter (timeTilButtonDisappear));
		}

		if (Input.GetMouseButtonDown(0))
		{
			touchPosition = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(0))
		{
			Vector2 deltaSwipe = touchPosition - Input.mousePosition;

			if (Mathf.Abs(deltaSwipe.x) > swipeResistanceX) 
			{
				// Swipe on Xaxis
			}
			if (deltaSwipe.y < swipeResistanceY) 
			{
				
				speedUp+=deltaSpeed;
				

			}

			if (deltaSwipe.y > swipeResistanceY) 
			{
				
				speedUp-=deltaSpeed;

			}
		}
	}


	void DoActivateTrigger(){
		//SceneManager.LoadScene (0);
	}
}
