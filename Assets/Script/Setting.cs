using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : Singleton<Setting> {

	protected Setting () {} // guarantee this will be always a singleton only - can't use the constructor!

	public GameSetting defaultGameSetting;
	public GameSetting[] gameSettings;

	public bool isJumpEnable = false;
	public bool isShakeEnable = false;
	public int currentLevel = 1;

	//public GameSetting defaultGameSetting;
	//[SerializeField]
	public static GameSetting gameSetting;
	//public static Setting instance;

	void Awake(){
		if (!gameSetting && defaultGameSetting) {
			//PlayerPrefs.DeleteAll(); // use this to test unlock level
			//Debug.Log ("here?");
			if (defaultGameSetting.gameMode != GameSetting.GameMode.TUTORIAL) {
				//Debug.Log ("not tut");
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
			//Debug.Log("music level:" + gameSetting.soundLevel);
			foreach (AudioSource audio in GameObject.FindObjectsOfType<AudioSource>()) {
				audio.enabled = gameSetting.enableSound;
				if(audio.enabled){
					//Debug.Log("audio:" + audio.name);
					//Debug.Log("volume:" + audio.volume);
					audio.volume = gameSetting.soundLevel;
				}
			}
		}
	}

	public void Update(){
		isJumpEnable = gameSetting.enableJump;
		isShakeEnable = gameSetting.enableShake;
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
	void setGameSetting(GameSetting gSetting){
		//copy transferrable values
		float soundLevel = gSetting.soundLevel;
		//copy sound level
		if(gameSetting){ //not null means it is set from main menu
			soundLevel = gameSetting.soundLevel;
			//Debug.Log("copy soundLevel:" + soundLevel);
		}
		//Debug.Log("gsetting.soundLevel:" + gSetting.soundLevel);
		gameSetting = Object.Instantiate(gSetting) as GameSetting;
		gameSetting.soundLevel = soundLevel;//abide by the value set by mainmenu if needed
		//Debug.Log("setGameSetting::game mode:" + gameSetting.gameMode);
		//Debug.Log("final sound level:" + gameSetting.soundLevel);
		
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
		currentLevel++;
		PlayerPrefs.SetInt("levelReached", currentLevel);
		Debug.Log("currentLevel:" + currentLevel);
		//go back to the level selector
		SceneManager.LoadScene (1);
	}

	//call by level selector
	public void StartGame(int index){
		setGameSetting(gameSettings[index]);
	}
}
