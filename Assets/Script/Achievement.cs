using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Achievement : MonoBehaviour {

	public bool testEnv = false;
	//level 1 : LEFT,RIGHT ENABLE, disable jump, shake
	public int snowBalls = 0;
	public int snowBallAchievement = 5;

	//level 2
	public int walker = 0;
	public int walkerAchievement = 10;

	//level 3 : ENABLE shake
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
	public Text count;
	public int total;
	public int counter;

	public Button nextButton;
	public Text instructionText;
	public Image instructionImage;
	public Sprite[] levelInstructionSprites;
	public int spriteIndex = 0;
	public string[] Instructions = {}; // correspond to leveinstructionsprites

	public bool isPaused = false;

	public void NextInstruction(){
		//Debug.Log ("current spriteIndex:" + spriteIndex);
		if (spriteIndex == Instructions.Length - 1 || testEnv) {
			if(isPaused)
				ResumeGame ();
			canvas.enabled = false;
			return;
		}

		if (Instructions [spriteIndex] == "-1") { // none sprite
			spriteIndex++;
			ShowInstruction (false);
			//Debug.Log ("Im gonna return");
			return;
		}
		instructionImage.sprite = levelInstructionSprites [spriteIndex];
		instructionText.text = Instructions [spriteIndex];
		spriteIndex++;
	}

	public void ShowInstruction(bool enabled){
		if (spriteIndex == Instructions.Length - 1 || testEnv) {
			if(isPaused)
				ResumeGame ();
			canvas.enabled = false;
			return;
		}
		if (enabled && !canvas.enabled) {
			PauseGame ();
			canvas.enabled = true;
		}else if (!enabled && canvas.enabled) {
			ResumeGame ();
			canvas.enabled = false;
		}
	}

	public void ResumeGame(){
		Debug.Log ("resume game");
		Time.timeScale = 1;
		EventManager.Instance.resumeEvent.Invoke ();
		isPaused = false;
	}

	public void PauseGame(){
		Debug.Log ("pause game");
		Time.timeScale = 0;
		EventManager.Instance.pauseEvent.Invoke ();
		isPaused = true;
	}

	// Use this for initialization
	void Start () {
		Debug.Log ("gamesetting:" + Setting.Instance.gameSetting.gameMode);
		if (Setting.Instance.gameSetting.gameMode != GameSetting.GameMode.TUTORIAL) {
			//EventManager.Instance.level5AchievementEvent.Invoke ();
			ShowInstruction (false);
			//InputManager.SetJump(Setting.Instance.gameSetting.enableJump);
			//InputManager.SetShakeable (Setting.Instance.gameSetting.enableShake);
			count.enabled = false;
			Debug.Log ("done");
			return;
		}
		if (!testEnv) {
			PauseGame();
		}
		EventManager.Instance.entPowerupCollisionEvent.AddListener (OnSnowAdded);
		EventManager.Instance.entPowerupCollisionEvent.AddListener (OnShieldAdded);
		EventManager.Instance.entEnemyCollisionEvent.AddListener (OnWalkerDestroyed);
		EventManager.Instance.entObstacleCollisionEvent.AddListener (OnObstacleDestroyed);
		EventManager.Instance.shakeOutputEvent.AddListener (OnShake);
		EventManager.Instance.swipeUpEvent.AddListener (OnJump);

		NextInstruction ();
		ShowInstruction (true);


	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (Setting.Instance.gameSetting.CONTINUE_KEY)) {
			nextButton.onClick.Invoke ();
		}
		if (snowBalls < snowBallAchievement) {
			total = snowBallAchievement;
			counter = snowBalls;
		} else if (walker < walkerAchievement) {
			total = walkerAchievement;
			counter = walker;
		} else if (shake < shakeAchievement) {
			total = shakeAchievement;
			counter = shake;
		} else if (jump < jumpAchievement) {
			total = jumpAchievement;
			counter = jump;
		} else if (shield < shieldAchievement) {
			total = shieldAchievement;
			counter = shield;
		}

		count.text = total - counter + " remaining before next level";
	}



	void OnSnowAdded(Entity ent, Entity other){
		Debug.Log ("snow added!!!");
		PowerUp pu = (PowerUp)other;
		if (pu.powerUptype == PowerUp.PowerUpType.SCALE_UP) {
			if (snowBalls+1 == snowBallAchievement) {
				EventManager.Instance.entPowerupCollisionEvent.RemoveListener (OnSnowAdded);
				text.text = "Level 1 unlocked!!!";
				//Debug.Log ("Level 1 achievement unlocked!!!");
				EventManager.Instance.level1AchievementEvent.Invoke();
				NextInstruction ();
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
			EventManager.Instance.entEnemyCollisionEvent.RemoveListener (OnWalkerDestroyed);
			text.text = "Level 2 unlocked!!!";
			//Debug.Log ("Level 2 achievement unlocked!!!");
			//InputManager.SetShakeable(true);
			Setting.Instance.SetShake(true);
			EventManager.Instance.level2AchievementEvent.Invoke();
			NextInstruction ();
			ShowInstruction (true);
		}
		walker++;
		
	}

	void OnShake(){
		if (shake + 1 == shakeAchievement) {
			EventManager.Instance.shakeEvent.RemoveListener (OnShake);
			text.text = "Level 3 unlocked!!!";
			//Debug.Log ("Level 3 achievement unlocked!!!");
			EventManager.Instance.level3AchievementEvent.Invoke();
			NextInstruction ();
			ShowInstruction (true);
		}
		shake++;
	}

	void OnObstacleDestroyed(Entity ent, Entity other){
		if (obstacle+1 == obstacleAchievement) {
			EventManager.Instance.entEnemyCollisionEvent.RemoveListener (OnObstacleDestroyed);
			text.text = "Level 4 unlocked!!!";
			//Debug.Log ("Level 4 achievement unlocked!!!");
			//InputManager.SetJump(true);
			Setting.Instance.SetJump(true);
			EventManager.Instance.level4AchievementEvent.Invoke();
			NextInstruction ();
			ShowInstruction (true);
		}
		obstacle++;

	}

	void OnJump(){
		if (jump + 1 == jumpAchievement) {
			EventManager.Instance.swipeUpEvent.RemoveListener (OnJump);
			text.text = "Level 5 unlocked!!!";
			//Debug.Log ("Level 5 achievement unlocked!!!");
			EventManager.Instance.level5AchievementEvent.Invoke();
			NextInstruction ();
			ShowInstruction (true);
		}
		jump++;
	}

	void OnShieldAdded(Entity ent, Entity other){
		PowerUp pu = (PowerUp)other;
		if (pu.powerUptype == PowerUp.PowerUpType.SHIELD) {
			if (shield+1 == shieldAchievement) {
				EventManager.Instance.entPowerupCollisionEvent.RemoveListener (OnShieldAdded);
				text.text = "Level 7 unlocked!!!";
				//Debug.Log ("Level 7 achievement unlocked!!!");
				EventManager.Instance.level7AchievementEvent.Invoke();
				ShowInstruction (true);
				NextInstruction ();

				//reload
				//Setting.instance.StartGame ();
				//SceneManager.LoadScene (1);
				//ShowInstruction (false);
			}
			shield++;
		}
	}
}
