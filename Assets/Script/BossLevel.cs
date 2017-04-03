using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel : MonoBehaviour {

	public GameObject trump;

	public Transform startMarker;
	public Transform endMarker;
	public float speed = 10.0F;
	private float startTime;
	private float journeyLength;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
	}

	// Update is called once per frame
	void Update () {
		int randIndex = Random.Range (0, 2);
		if (randIndex == 0) {
			//trump.transform.Translate (0.005f, 0, 0);
		}
		else if (randIndex == 1) {
			//trump.transform.Translate (0.005f, 0, 0.005f);
		}

		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);

	}
}
