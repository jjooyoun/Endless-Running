using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour {
	public int lives = 3;
	public GameObject liveSprite;
	public GameObject endGameMsg;
	private GameObject[] liveSprites;


	public void StartSpawnLives(){
		SpriteRenderer liveSpriteRenderer = liveSprite.GetComponent<SpriteRenderer> ();
		//float RBX = ;
		//float x = transform.position.x;
		float x = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f)).x - liveSpriteRenderer.bounds.extents.x;
		float y = transform.position.y;
		liveSprites = new GameObject[lives];
		for (int i = 0; i < lives; i++) {
			liveSprites[i] = GameObject.Instantiate(liveSprite, new Vector3(x - (float)(i*liveSpriteRenderer.bounds.size.x), y, 0.0f), Quaternion.identity);
		}
	}

	public void OnPlayerDie(){
		//take off live
		Destroy(liveSprites[lives-1]);
		lives--;
		if(lives <= 0){
			Debug.Log("game over!!!");
			EnableMsg("You Lose");
			EventDispatcher.Instance.GameOverEvent.Invoke();
			//go back to main scene
		}
	}

	public void OnGameWin(){
		EnableMsg("You Win");
		EventDispatcher.Instance.QuitGameEvent.Invoke();
	}

	void EnableMsg(string msg){
		endGameMsg.GetComponent<Text>().text = msg;
		endGameMsg.SetActive(true);
	}
}
