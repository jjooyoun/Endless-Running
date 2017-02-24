using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {

	public Color HitFlash;
	public int flashTime;

	public Text countText;
	public Text countLives;
	private const string LIVE_TEXT = "Lives: ";
	private const string SCORE_TEXT = "Score: ";
	private int count = 0;
	private int lives = 10;

	// Use this for initialization
	void Start () {
		SetText (countLives, LIVE_TEXT, lives.ToString ());
		SetText(countText, SCORE_TEXT, count.ToString());
        //listen to event
		EventManager.instance.entPowerupCollisionEvent.AddListener (EntPowerUpCollisionHandler);
		EventManager.instance.entEnemyCollisionEvent.AddListener (EntEnemyCollisionHandler);
	}
		

	void EntPowerUpCollisionHandler(Entity ent, Entity other){
		count++;
		SetText(countText, SCORE_TEXT, count.ToString());
	}

	void EntEnemyCollisionHandler(Entity ent, Entity other){
		//flashing the entity
		Renderer entRenderer = ent.GetComponent<Renderer>();
		ent.GetComponent<Collider>().enabled = false;
        StartCoroutine(Flash(ent, entRenderer, entRenderer.material.color, HitFlash));
		if (lives - 1 == 0) {
			Time.timeScale = 0;
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
