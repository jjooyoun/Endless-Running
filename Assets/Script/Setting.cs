using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : /*Singleton<Setting>*/ MonoBehaviour {

	//protected Setting () {} // guarantee this will be always a singleton only - can't use the constructor!

	public GameSetting defaultGameSetting;
	public GameSetting[] gameSettings;

	//Runtime value
	public bool isJumpEnable = false;
	public bool isShakeEnable = false;
	public string SettingName = "";
	public GameSetting.GameMode gameMode;
	
	//[END] Runtime value

	public static GameSetting gameSetting; //modify over the course of the game

	void Start(){
		EventManager.Instance.levelFinishedEvent.AddListener(GoNextLevel);
	}

	void Awake(){
		//Debug.Log("register go next level!!!");
		EventManager.Instance.levelFinishedEvent.AddListener(GoNextLevel);
		if (!gameSetting && defaultGameSetting) {
			//PlayerPrefs.DeleteAll(); // use this to test unlock level
			if (defaultGameSetting.gameMode != GameSetting.GameMode.TUTORIAL) {
				EventManager.Instance.stage1.Invoke ();
			}
			setGameSetting (defaultGameSetting);
		}
	}

	void OnEnable(){
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable(){
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
		Debug.Log ("finished loading scene:" + scene.name);
		if (gameSetting) {
			foreach (AudioSource audio in GameObject.FindObjectsOfType<AudioSource>()) {
				audio.enabled = gameSetting.enableSound;
				if(audio.enabled){
					audio.volume = gameSetting.soundLevel;
				}
			}
		}
	}

	public void Update(){
		isJumpEnable = gameSetting.enableJump;
		isShakeEnable = gameSetting.enableShake;
		gameMode = gameSetting.gameMode;
		SettingName = gameSetting.name;

		if(Input.GetKeyDown(KeyCode.U)){//test
			if(Setting.gameSetting.isPaused)
				Setting.ResumeGame ();
			EventManager.Instance.levelFinishedEvent.Invoke();
		}
	}

	public void StartGame(){
		setGameSetting(gameSettings [1]);	
		EventManager.Instance.stage1.Invoke ();
	}
	/* USE IN MAIN MENU CLICK */
	public void StartTutorial(){
		setGameSetting (gameSettings [0]);
	}

	public void StartDefault(){
		if (!gameSetting)
			setGameSetting(gameSettings [0]);
	}
	/* USE IN MAIN MENU CLICK */

	//clone instead of referencing directly
	//should not pass in gSetting null
	static void setGameSetting(GameSetting gSetting){
		//copy transferrable values
		float soundLevel = gSetting.soundLevel;
		//copy sound level
		if(gameSetting){ //not null means it is set from main menu
			soundLevel = gameSetting.soundLevel;
		}
		gameSetting = Object.Instantiate(gSetting) as GameSetting;
		gameSetting.soundLevel = soundLevel;//abide by the value set by mainmenu if needed
		Debug.Log("gamePausedStart?:" + gameSetting.isPaused);
		//is game pause?
		if(gameSetting.isPaused){
			PauseGame();
		}
		
	}

	//run time edit
	public static bool SetShake(bool flag){
		bool oldflag = flag;
		gameSetting.enableShake = flag;
		return oldflag;
	}

	public static bool SetJump(bool flag){
		bool oldFlag = flag;
		gameSetting.enableJump = flag;
		return oldFlag;
	}

	public void GoNextLevel(){
		GameObjectUtil.ClearPool ();
		int currentLevelPlayerPref = PlayerPrefs.GetInt("levelReached");
		if(currentLevelPlayerPref < gameSetting.currentLevel)
			PlayerPrefs.SetInt("levelReached", gameSetting.currentLevel);
		Debug.Log("currentLevel:" + gameSetting.currentLevel);
		StartGame(gameSetting.currentLevel); //-1
		//go back to the level selector
		SceneManager.LoadScene (1);
	}

	//call by level selector
	public void StartGame(int index){
		setGameSetting(gameSettings[index]);
	}

	public static void PauseGame(){
		gameSetting.isPaused = true;
		Time.timeScale = 0.0f;
		EventManager.Instance.pauseEvent.Invoke(); //obstacle pause
	}

	public static void ResumeGame(){
		gameSetting.isPaused = false;
		Time.timeScale = 1.0f;
		EventManager.Instance.resumeEvent.Invoke(); //obstacle resume
	}
}
