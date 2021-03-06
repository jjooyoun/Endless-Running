﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreSystem : MonoBehaviour {

	public Color HitFlash;
	public int flashTime;
	public float distance;
	public Transform player;

	public Text countText;
	public Text countLives;
	public Text countDistance;

	public float flashVar1;
	public float flashVar2;
	public GameObject Progress;
	private const string LIVE_TEXT = "Lives: ";
	private const string SCORE_TEXT = "Score: ";
	private const string DISTANCE_TEXT = "Distance: ";
	public static int count = 0; //  Static keyword makes this variable a Member of the class, not of any particular instance
	private Dictionary<string, int> ScoreLUT = new Dictionary<string, int>();
	private int lives = 10;
	private int distances = 0;
	private DistanceSystem distancesystem;

	void SetUpScoreLUT(){
		foreach(GameSetting.ScoreEntry se in Setting.gameSetting.ScoreLUT){
			ScoreLUT.Add(se.entName, se.score);
		}
		
	}
	

	// Use this for initialization
	void Start () {
		lives = Setting.gameSetting.lives;
		SetText (countLives, LIVE_TEXT, lives.ToString ());
		SetText(countText, SCORE_TEXT, count.ToString());
		SetUpScoreLUT();
		//	SetText(countDistance, DISTANCE_TEXT, distances.ToString());
		EventManager.Instance.percentCompleteEvent.AddListener(UpdateProgressBar);
		//listen to event
		EventManager.Instance.entPowerupCollisionEvent.AddListener (EntPowerUpCollisionHandler);
		EventManager.Instance.entObstacleCollisionEvent.AddListener (EntCrushEntHandler);
		EventManager.Instance.entEnemyCollisionEvent.AddListener (EntCrushEntHandler);
		EventManager.Instance.FlashAndLoseLiveEvent.AddListener (FlashAndLoseLive);
		distancesystem = FindObjectOfType<DistanceSystem>();
	}


	//	void Awake(){
	//		distance = Vector3.Distance (player.position, transform.position);
	//	}

	//	void Update(){
	//		score ();
	//	}

	//	void score(){
	//		distance = Vector3.Distance (player.position, transform.position);
	//		SetText(countDistance, DISTANCE_TEXT, distances.ToString());
	//		countDistance.text= distance.ToString ();
	//	}

	void UpdateScore(Entity ent){
		//count += Setting.gameSetting.ScoresLUT[ent.entityName];
		count += ScoreLUT[ent.entityName];
		SetText(countText, SCORE_TEXT, count.ToString());
	}

	//SHOULD CONSIDER REPLACING THESE NEXT TWO FNS WITH ONE?
	void EntPowerUpCollisionHandler(Entity ent, Entity other){
		UpdateScore(other);
	}

	void EntCrushEntHandler(Entity ent, Entity other){
		UpdateScore(other);
	}

	void FlashAndLoseLive(Entity ent, Entity other){
		StartFlashWrapper(ent);
		Handheld.Vibrate();
		if (lives - 1 == 0) { 
			//Time.timeScale = 0;//POTENTIAL BUG HERE. NOTORIOUS BLACK SCREEN ? LEAVE NOTES AND WILL TEST
			distancesystem.distanceIncreasing=false;
			//SceneManager.LoadScene (3);
			Setting.StaticQuitGame();
			Setting.LoadGameOverScene();
		}
		lives--;
		SetText (countLives, LIVE_TEXT, lives.ToString ());
	}

	public void StartFlashWrapper(Entity ent){
		Renderer entRenderer = ent.GetComponent<Renderer>();
		if (!entRenderer)
			entRenderer = ent.GetComponentInChildren<Renderer> ();
		ent.GetComponent<Collider>().enabled = false;
		StartCoroutine(Flash(ent, entRenderer, entRenderer.material.color, HitFlash));
	}

	IEnumerator Flash(Entity ent, Renderer entRenderer, Color originalColor, Color flashColor){
		for (int i = 0; i < flashTime; i++) {
			entRenderer.material.color = flashColor;
			yield return new WaitForSeconds (flashVar1);
			entRenderer.material.color = originalColor;
			yield return new WaitForSeconds (flashVar2);
		}
		ent.GetComponent<Collider> ().enabled = true;
		Debug.Log ("finished flashing!!!");
		if (ent.entityName == Entity.BOSS_NAME) {
			ent.GetComponent<BossLevel> ().startMoving = true;
			ent.GetComponent<Rigidbody>().isKinematic = true;
		}
	}

	void SetText(Text text, string preMsg, string msg){
		text.text = preMsg + msg;
	}

	void UpdateProgressBar(float percent){
		Progress.GetComponent<Renderer>().material.SetFloat("_Progress", percent);
	}
}