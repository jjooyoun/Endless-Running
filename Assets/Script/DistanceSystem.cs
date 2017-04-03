using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceSystem : MonoBehaviour {
	
	public Transform player;

	public Text countDistance;
	public Text Totaltime;

	public float distance;
	public float t;
	public float ellapsed;
	public float rellapsed;

	public bool distanceIncreasing;

	public float measurementSysDiv = 100.00f;

	public bool gamePaused = false;

	void Start(){
		EventManager.Instance.pauseEvent.AddListener(OnGamePause);
		EventManager.Instance.resumeEvent.AddListener(OnGameResume);
	}

	void Awake(){
		distance = Vector3.Distance (player.position, transform.position);
		distance = 0;
		t = Time.time;
	}

	void Update(){
		if(gamePaused)
			return;
		if (distanceIncreasing) {
			score ();
		}
		ellapsed = Time.time-t;
		rellapsed = Mathf.Round (ellapsed);
		Totaltime.text = "Timer: " + rellapsed.ToString ()+"  sec";
		countDistance.text= "Dist: "+ (distance/measurementSysDiv).ToString ("0.00");//2 decimal places
	}
	void score(){
		distance += Vector3.Distance (player.position, transform.position);
	}

	void OnGamePause(){
		Debug.Log("distance pause!");
		gamePaused = true;
	}

	void OnGameResume(){
		Debug.Log("distance resume!");
		gamePaused = false;
	}
//	void OnGUI(){
//		GUI.Label (new Rect (10, 10, 100, 20), distance.ToString ());
//	}

}

