using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement : MonoBehaviour {
	//level 1
	public int snowBalls = 0;
	public int snowBallAchievement = 5;

	//level 2
	public int walker = 0;
	public int walkerAchievement = 10;

	//level 3
	public int shake = 0;
	public int shakeAchievement = 10;

	//level 4
	public int obstacle = 0;
	public int obstacleAchievement = 10;

	//level 5
	public int jump = 0;
	public int jumpAchievement = 5;

	//level 7
	public int shield = 0;
	public int shieldAchievement = 2;

	public Canvas canvas;
	public Text text;
	public Text instructionText;
	public Image instructionImage;
	public Sprite[] levelInstructionSprites;
	public int spriteIndex = 0;
	public string[] Instructions = {}; // correspond to leveinstructionsprites


	public void NextInstruction(){
		Debug.Log ("current spriteIndex:" + spriteIndex);
		if (Instructions [spriteIndex] == "-1") {
			Time.timeScale = 1;
			ShowInstruction (false);
			Debug.Log ("Im gonna return");
			return;
		}
		instructionImage.sprite = levelInstructionSprites [spriteIndex];
		instructionText.text = Instructions [spriteIndex];
		spriteIndex++;
	}

	public void ShowInstruction(bool enabled){
		if (enabled && !canvas.enabled) {
			Time.timeScale = 0;
			canvas.enabled = true;
		}else if (!enabled && canvas.enabled) {
			canvas.enabled = false;
		}
	}

	// Use this for initialization
	void Start () {
		NextInstruction ();
		EventManager.instance.entPowerupCollisionEvent.AddListener (OnSnowAdded);
		EventManager.instance.entPowerupCollisionEvent.AddListener (OnShieldAdded);
		EventManager.instance.entEnemyCollisionEvent.AddListener (OnWalkerDestroyed);
		EventManager.instance.entObstacleCollisionEvent.AddListener (OnObstacleDestroyed);
		EventManager.instance.shakeEvent.AddListener (OnShake);
		EventManager.instance.swipeUpEvent.AddListener (OnJump);
		Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}



	void OnSnowAdded(Entity ent, Entity other){
		PowerUp pu = (PowerUp)other;
		if (pu.powerUptype == PowerUp.PowerUpType.SCALE_UP) {
			if (snowBalls+1 == snowBallAchievement) {
				EventManager.instance.entPowerupCollisionEvent.RemoveListener (OnSnowAdded);
				text.text = "Level 1 achievement unlocked!!!";
				//Debug.Log ("Level 1 achievement unlocked!!!");
				EventManager.instance.level1AchievementEvent.Invoke();
				ShowInstruction (true);
			}
			snowBalls++;
		}
	}



	void OnWalkerDestroyed(Entity ent, Entity other){
		//Debug.Log ("hello:" + other.name);
		if (other.entityName != "Walker")
			return;
		if (walker+1 == walkerAchievement) {
			EventManager.instance.entEnemyCollisionEvent.RemoveListener (OnWalkerDestroyed);
			text.text = "Level 2 achievement unlocked!!!";
			//Debug.Log ("Level 2 achievement unlocked!!!");
			EventManager.instance.level2AchievementEvent.Invoke();
			ShowInstruction (true);
		}
		walker++;
		
	}

	void OnShake(){
		if (shake + 1 == shakeAchievement) {
			EventManager.instance.shakeEvent.RemoveListener (OnShake);
			text.text = "Level 3 achievement unlocked!!!";
			//Debug.Log ("Level 3 achievement unlocked!!!");
			EventManager.instance.level3AchievementEvent.Invoke();
			ShowInstruction (true);
		}
		shake++;
	}

	void OnObstacleDestroyed(Entity ent, Entity other){
		if (obstacle+1 == obstacleAchievement) {
			EventManager.instance.entEnemyCollisionEvent.RemoveListener (OnObstacleDestroyed);
			text.text = "Level 4 achievement unlocked!!!";
			//Debug.Log ("Level 4 achievement unlocked!!!");
			EventManager.instance.level4AchievementEvent.Invoke();
			ShowInstruction (true);
		}
		obstacle++;

	}

	void OnJump(){
		if (jump + 1 == jumpAchievement) {
			EventManager.instance.swipeUpEvent.RemoveListener (OnJump);
			text.text = "Level 5 achievement unlocked!!!";
			//Debug.Log ("Level 5 achievement unlocked!!!");
			EventManager.instance.level5AchievementEvent.Invoke();
			ShowInstruction (true);
		}
		jump++;
	}

	void OnShieldAdded(Entity ent, Entity other){
		PowerUp pu = (PowerUp)other;
		if (pu.powerUptype == PowerUp.PowerUpType.SHIELD) {
			if (shield+1 == shieldAchievement) {
				EventManager.instance.entPowerupCollisionEvent.RemoveListener (OnShieldAdded);
				text.text = "Level 7 achievement unlocked!!!";
				//Debug.Log ("Level 7 achievement unlocked!!!");
				EventManager.instance.level7AchievementEvent.Invoke();
				ShowInstruction (true);
			}
			shield++;
		}
	}
}
