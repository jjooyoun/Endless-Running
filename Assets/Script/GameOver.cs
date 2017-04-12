﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOver: MonoBehaviour {
    private static int playerScore;  //  A new Static variable to hold our score
    public Text scoreText;
    void Start()	
    {
        playerScore = PlayerPrefs.GetInt("LastScore", 0);  //  Update our score		
        scoreText.text = "Your score: " + playerScore.ToString ();
        Debug.Log("Score:" + playerScore);
    }
}