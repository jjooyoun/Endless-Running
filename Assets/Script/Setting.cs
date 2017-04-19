using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Setting : Singleton<Setting> {

	//protected Setting () {} // guarantee this will be always a singleton only - can't use the constructor!
	private static readonly string RUNNING_CANVAS_TAG = "RunningCanvas";

	private static readonly int GAME_OVER_SCENE_INDEX = 3;
	private static readonly int LEVEL_COMPLETE_SCENE_INDEX = 5;

	public static readonly string LATEST_SCORE = "LastScore";


	public GameSetting defaultGameSetting;
	public GameSetting[] gameSettings;

	//Runtime value
	public bool isJumpEnable = false;
	public bool isShakeEnable = false;
	public string SettingName = "";
	public GameSetting.GameMode gameMode;
	public int currentLevel = -1;
	//[END] Runtime value

	public static GameSetting gameSetting; //modify over the course of the game

	

	void Start(){
		EventManager.Instance.levelFinishedEvent.AddListener(GoNextLevel);
	}

	void Awake(){
		//Debug.Log("register go next level!!!");
		EventManager.Instance.levelFinishedEvent.AddListener(GoNextLevel);
		if (!gameSetting && defaultGameSetting) {
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
		//make the scene active
		//SceneManager.SetActiveScene(scene);
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

	void Update(){
		isJumpEnable = gameSetting.enableJump;
		isShakeEnable = gameSetting.enableShake;
		gameMode = gameSetting.gameMode;
		SettingName = gameSetting.name;
		currentLevel = gameSetting.currentLevel;

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
		Debug.Log ("currentLevelPlayerPref:" + currentLevelPlayerPref);
		Debug.Log ("currentLevelSetting:" + gameSetting.currentLevel);
		if(gameSetting.currentLevel > currentLevelPlayerPref)
			PlayerPrefs.SetInt("levelReached", gameSetting.currentLevel + 1);
		//Debug.Log("currentLevel:" + gameSetting.currentLevel);
		//StartGame(gameSetting.currentLevel); //-1
		//go back to the level selector
		//Debug.Log("load completelevel");
		//GameObjectUtil.ClearPool();
		LoadLevelCompletescene();
	}

	//call by level selector
	public void StartGame(int index){
		//reset default effect
		PowerUp.hasFire = false;
		PowerUp.hasWater = false;
		PowerUp.hasShield = false;
		setGameSetting(gameSettings[index]);
	}

	//PASS IN SLIDER SO WE HAVE ACCESS TO THE CHANGED VOLUME
	public void OnMasterVolumeChange(Slider slider){
		Setting.gameSetting.soundLevel = slider.value;
		foreach (AudioSource audio in GameObject.FindObjectsOfType<AudioSource>()) {
			//float b4Vol = audio.volume;
			audio.volume = gameSetting.soundLevel;
			//float a3Vol = audio.volume;
			//Debug.Log("audio[" + audio.name + "] : " + b4Vol + "~>" + a3Vol);
		}
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

	public void QuitGame(){
		GameObjectUtil.ClearPool ();
	}

	public static void LoadLevelCompletescene(){
		PlayerPrefs.SetInt("LastScore", ScoreSystem.count);
		SceneManager.LoadScene(LEVEL_COMPLETE_SCENE_INDEX);
	}

	public static void LoadGameOverScene(){
		PlayerPrefs.SetInt("LastScore", ScoreSystem.count);
		SceneManager.LoadScene(GAME_OVER_SCENE_INDEX);
	}
}
