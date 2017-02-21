using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {

	public Text countText;
	public Text countLives;
	private const string LIVE_TEXT = "Lives: ";
	private const string SCORE_TEXT = "Score: ";
	private int count = 0;
	private int lives = 3;

	// Use this for initialization
	void Start () {
		SetText (countLives, LIVE_TEXT, lives.ToString ());
		SetText(countText, SCORE_TEXT, count.ToString());

	}

	void OnTriggerEnter(Collider other){
		Entity ent = other.GetComponent<Entity> ();
		if (ent && ent.entityType == Entity.ENTITY_TYPE.ENEMY) {
			//life decrease
			if (lives - 1 == 0) {
				Time.timeScale = 0;
			}
			lives--;
			SetText (countLives, LIVE_TEXT, lives.ToString ());

		} else {
			count++;
			//SetCountText ("Score:");
			SetText(countText, SCORE_TEXT, count.ToString());
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}


		
	void SetText(Text text, string preMsg, string msg){
		text.text = preMsg + msg;
	}
}
