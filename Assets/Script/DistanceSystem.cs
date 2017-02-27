using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DistanceSystem : MonoBehaviour {
	
	public Transform player;

	public Text countDistance;

	public float distance;

	void Awake(){
		distance = Vector3.Distance (player.position, transform.position);
		//distance = 0;
	}

	void Update(){
		score ();
	}
	void score(){
		distance += Vector3.Distance (player.position, transform.position);
		countDistance.text= "Distance: "+ distance.ToString ();
	}
//	void OnGUI(){
//		GUI.Label (new Rect (10, 10, 100, 20), distance.ToString ());
//	}
}

