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

	public int volcano = 0;
	public int volcanoAchievement = 10;
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

	public GameObject instructionPanel;
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
	public bool isInstructionShownAtPause = false;
	private bool jumpState = false;

	public void NextInstruction(){
		//Debug.Log ("current spriteIndex:" + spriteIndex);
		if (spriteIndex == Instructions.Length - 1 || testEnv) {
			//done tutorial
			Setting.Instance.GoNextLevel();
			if(Setting.gameSetting.isPaused)
				Setting.ResumeGame ();
			instructionPanel.SetActive(false);
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
			if(Setting.gameSetting.isPaused)
				Setting.ResumeGame ();
			instructionPanel.SetActive(false);
			return;
		}
		if (enabled && !instructionPanel.activeSelf) {
			Setting.PauseGame ();
			instructionPanel.SetActive(true);
		}else if (!enabled && instructionPanel.activeSelf) {
			Setting.ResumeGame ();
			instructionPanel.SetActive(false);
		}
	}

	public void MenuPauseGame(){
		isInstructionShownAtPause = instructionPanel.activeSelf;
		Setting.PauseGame();
		instructionPanel.SetActive(false);// i manage my own state
	}

	public void MenuResumeGame(){
		instructionPanel.SetActive(isInstructionShownAtPause);
		if(!isInstructionShownAtPause)
			Setting.ResumeGame();
	}

	// Use this for initialization
	void Start () {
		Debug.Log ("gamesetting:" + Setting.gameSetting.gameMode);
		if (Setting.gameSetting.gameMode != GameSetting.GameMode.TUTORIAL) {
			ShowInstruction (false);
			count.enabled = false;
			return;
		}
		if (!testEnv) {
			Setting.PauseGame();
		}
		EventManager.Instance.entPowerupCollisionEvent.AddListener (OnSnowAdded);
		EventManager.Instance.entPowerupCollisionEvent.AddListener (OnShieldAdded);
		EventManager.Instance.entEnemyCollisionEvent.AddListener (OnWalkerDestroyed);
		EventManager.Instance.entEnemyCollisionEvent.AddListener (OnVolcanoCollision);
		EventManager.Instance.entObstacleCollisionEvent.AddListener (OnObstacleDestroyed);
		EventManager.Instance.shakeOutputEvent.AddListener (OnShake);
		EventManager.Instance.swipeUpEvent.AddListener (OnJump);

		NextInstruction ();
		ShowInstruction (true);


	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("jump:" + Setting.gameSetting.enableJump);
		if (Input.GetKeyDown (Setting.gameSetting.CONTINUE_KEY)) {
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
		}else if (volcano < volcanoAchievement){
			total = volcanoAchievement;
			counter = volcano;
		} else if (obstacle < obstacleAchievement){
			total = obstacleAchievement;
			counter = obstacle;
		} 
		else if (jump < jumpAchievement) {
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
			//Setting.SetShake(true);
			EventManager.Instance.level2AchievementEvent.Invoke();
			NextInstruction ();
			ShowInstruction (true);
		}
		walker++;
		
	}

	void OnVolcanoCollision(Entity ent, Entity other){
		//Debug.Log ("hello:" + other.name);
		if (other.entityName != "Volcano")
			return;
		if (volcano+1 == volcanoAchievement) {
			EventManager.Instance.entEnemyCollisionEvent.RemoveListener (OnVolcanoCollision);
			text.text = "Level 3 unlocked!!!";
			//Debug.Log ("Level 2 achievement unlocked!!!");
			//Setting.SetShake(true);
			EventManager.Instance.level3AchievementEvent.Invoke();
			NextInstruction ();
			ShowInstruction (true);
		}
		volcano++;
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
		Debug.Log("obstacle!!");
		if(other.entityName != "Gate"){
			Debug.Log("not obstacle gate");
			return;
		}
		if (obstacle+1 == obstacleAchievement) {
			EventManager.Instance.entObstacleCollisionEvent.RemoveListener (OnObstacleDestroyed);
			text.text = "Level 4 unlocked!!!";
			//Debug.Log ("Level 4 achievement unlocked!!!");
			Setting.SetJump(true);
			jumpState = true;
			Debug.Log("jump:" + Setting.gameSetting.enableJump);
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
			}
			shield++;
		}
	}
}
