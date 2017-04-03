using System.Collections;
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
	private const string LIVE_TEXT = "Lives: ";
	private const string SCORE_TEXT = "Score: ";
	private const string DISTANCE_TEXT = "Distance: ";
	public static int count = 0; //  Static keyword makes this variable a Member of the class, not of any particular instance
	private int lives = 5;
	private int distances = 0;
	private DistanceSystem distancesystem;

	// Use this for initialization
	void Start () {
		lives = Setting.gameSetting.lives;
		SetText (countLives, LIVE_TEXT, lives.ToString ());
		SetText(countText, SCORE_TEXT, count.ToString());
		//	SetText(countDistance, DISTANCE_TEXT, distances.ToString());
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

	void EntPowerUpCollisionHandler(Entity ent, Entity other){
		count += Setting.gameSetting.powerupScorePoint;
		SetText(countText, SCORE_TEXT, count.ToString());
	}

	void EntCrushEntHandler(Entity ent, Entity other){
		other.gameObject.SetActive (false);
		count += Setting.gameSetting.enemyScorePoint;
		SetText(countText, SCORE_TEXT, count.ToString());
	}

	void FlashAndLoseLive(Entity ent, Entity other){
		//Debug.Log ("ent:" + ent.name);
		//Debug.Log ("other:" + other.name);
		//flashing the entity
		Renderer entRenderer = ent.GetComponent<Renderer>();
		ent.GetComponent<Collider>().enabled = false;
		StartCoroutine(Flash(ent, entRenderer, entRenderer.material.color, HitFlash));
		Handheld.Vibrate();
		if (lives - 1 == 0) {
			Time.timeScale = 0;
			distancesystem.distanceIncreasing=false;
			SceneManager.LoadScene (2);
		}
		lives--;
		SetText (countLives, LIVE_TEXT, lives.ToString ());
	}

	IEnumerator Flash(Entity ent, Renderer entRenderer, Color originalColor, Color flashColor){
		for (int i = 0; i < flashTime; i++) {
			entRenderer.material.color = flashColor;
			yield return new WaitForSeconds (.3f);
			entRenderer.material.color = originalColor;
			yield return new WaitForSeconds (.3f);
		}
		ent.GetComponent<Collider> ().enabled = true;
		Debug.Log ("finished flashing!!!");
	}

	void SetText(Text text, string preMsg, string msg){
		text.text = preMsg + msg;
	}
}