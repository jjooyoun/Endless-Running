﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {

	public Text countText;
	private int count = 0;

	// Use this for initialization
	void Start () {
		SetCountText ();
	}

	void OnTriggerEnter(Collider other){
		Entity ent = other.GetComponent<Entity> ();
		if (ent && ent.entityType == Entity.ENTITY_TYPE.ENEMY) {
			//life decrease
		} else {
			count++;
			SetCountText ();
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetCountText() {
		countText.text = "Score: " + count.ToString ();
	}
}
