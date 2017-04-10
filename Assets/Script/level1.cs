using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class level1: MonoBehaviour {
	private static int playerScore;  //  A new Static variable to hold our score.
	public Text scoreText;

	void Start()
	{
		playerScore = ScoreSystem.count;  //  Update our score
		scoreText.text = "Your score: " + playerScore.ToString ();
	}
}
