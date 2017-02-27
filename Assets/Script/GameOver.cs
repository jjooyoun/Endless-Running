using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameOver: MonoBehaviour {
	private static int playerScore;  //  A new Static variable to hold our score.
	private static float timer;
	public Text scoreText;
	public Text timeText;

	void Start()
	{
		playerScore = ScoreSystem.count;  //  Update our score from ScoreSystem
		scoreText.text = "Your score: " + playerScore.ToString ();

		timer = DistanceSystem.ellapsed; //  Update the time from DistanceSystem
		timeText.text = "Time elapsed: " + Mathf.Round(timer).ToString () + " sec";

	}


}
