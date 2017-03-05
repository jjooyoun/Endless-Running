using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour {
	public GameSetting[] gameSettings;

	[SerializeField]
	public static GameSetting gameSetting;
	public static Setting instance;

	void Awake(){
		DontDestroyOnLoad(gameObject);
		if(Setting.instance==null){
			Setting.instance=this;
		} else {
			//Debug.LogWarning(“A previously awakened Settings MonoBehaviour exists!” + gameObject);
			Debug.LogWarning("A previously awakened Settings MonoBehaviour exists!" + gameObject);
		}
		if(Setting.gameSetting ==null){
			Setting.gameSetting=gameSettings[0];
		}
	}

	public void StartGame(){
		setGameSetting(gameSettings [1]);	
	}

	public void StartTutorial(){
		setGameSetting (gameSettings [0]);
	}

	void setGameSetting(GameSetting gSetting){
		gameSetting = gSetting;
	}
}
