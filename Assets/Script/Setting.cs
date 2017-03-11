using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : Singleton<Setting> {

	protected Setting () {} // guarantee this will be always a singleton only - can't use the constructor!

	public GameSetting defaultGameSetting;
	public GameSetting[] gameSettings;

	//public GameSetting defaultGameSetting;
	//[SerializeField]
	public static GameSetting gameSetting;
	//public static Setting instance;

	void Awake(){
		if (!gameSetting && defaultGameSetting) {
			Debug.Log ("here?");
			if (defaultGameSetting.gameMode != GameSetting.GameMode.TUTORIAL) {
				Debug.Log ("not tut");
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
			}
		}
	}

//	public void Update(){
//		Debug.Log ("game Mode:" + gameSetting.gameMode);
//	}

	public void StartGame(){
		setGameSetting(gameSettings [1]);	
		EventManager.Instance.stage1.Invoke ();
	}

	public void StartTutorial(){
		setGameSetting (gameSettings [0]);
	}

	public void StartDefault(){
		if (!gameSetting)
			setGameSetting(gameSettings [0]);
	}

	void setGameSetting(GameSetting gSetting){
		gameSetting = gSetting;
	}

	//run time edit
	public bool SetShake(bool flag){
		bool oldflag = flag;
		gameSetting.enableShake = flag;
		return oldflag;
	}

	public bool SetJump(bool flag){
		bool oldFlag = flag;
		gameSetting.enableJump = flag;
		return oldFlag;
	}
}
