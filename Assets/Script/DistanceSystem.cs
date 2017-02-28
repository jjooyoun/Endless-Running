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

	void Awake(){
		distance = Vector3.Distance (player.position, transform.position);
		distance = 0;
		t = Time.time;
	}

	void Update(){
		if (distanceIncreasing) {
			score ();
		}
		ellapsed = Time.time-t;
		rellapsed = Mathf.Round (ellapsed);
		Totaltime.text = "Timer: " + rellapsed.ToString ()+"  sec";
		countDistance.text= "Distance: "+ distance.ToString ();
	}
	void score(){
		distance += Vector3.Distance (player.position, transform.position);
	}
//	void OnGUI(){
//		GUI.Label (new Rect (10, 10, 100, 20), distance.ToString ());
//	}

}

