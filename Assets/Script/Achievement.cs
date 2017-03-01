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

	public Text text;

	// Use this for initialization
	void Start () {
		EventManager.instance.entPowerupCollisionEvent.AddListener (OnSnowAdded);
		EventManager.instance.entPowerupCollisionEvent.AddListener (OnShieldAdded);
		EventManager.instance.entEnemyCollisionEvent.AddListener (OnWalkerDestroyed);
		EventManager.instance.entObstacleCollisionEvent.AddListener (OnObstacleDestroyed);
		EventManager.instance.shakeEvent.AddListener (OnShake);
		EventManager.instance.swipeUpEvent.AddListener (OnJump);
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
		}
		walker++;
		
	}

	void OnShake(){
		if (shake + 1 == shakeAchievement) {
			EventManager.instance.shakeEvent.RemoveListener (OnShake);
			text.text = "Level 3 achievement unlocked!!!";
			//Debug.Log ("Level 3 achievement unlocked!!!");
			EventManager.instance.level3AchievementEvent.Invoke();
		}
		shake++;
	}

	void OnObstacleDestroyed(Entity ent, Entity other){
		if (obstacle+1 == obstacleAchievement) {
			EventManager.instance.entEnemyCollisionEvent.RemoveListener (OnObstacleDestroyed);
			text.text = "Level 4 achievement unlocked!!!";
			//Debug.Log ("Level 4 achievement unlocked!!!");
			EventManager.instance.level4AchievementEvent.Invoke();
		}
		obstacle++;

	}

	void OnJump(){
		if (jump + 1 == jumpAchievement) {
			EventManager.instance.swipeUpEvent.RemoveListener (OnJump);
			text.text = "Level 5 achievement unlocked!!!";
			//Debug.Log ("Level 5 achievement unlocked!!!");
			EventManager.instance.level5AchievementEvent.Invoke();
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
			}
			shield++;
		}
	}
}
