using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "GameSetting", order = 1)]
public class GameSetting : ScriptableObject {
	public enum GameMode{
		GAME,
		TUTORIAL,
		TEST
	};

	public GameMode gameMode = GameMode.TUTORIAL;
	public int lives = 10;

	[System.Serializable]
	public class ScoreEntry{
		
		public string entName;
		public int score;
		public ScoreEntry(string entName, int score){ this.score = score; this.entName = entName;}
	};

	public ScoreEntry[] ScoreLUT = {
		new ScoreEntry("PowerUp",1),
		new ScoreEntry("EarthPowerUp",1),
		new ScoreEntry("FirePowerUp", 1),
		new ScoreEntry("WaterPowerUp", 1),
		new ScoreEntry("EarthPowerUpFloating", 2),
		new ScoreEntry("FirePowerUpFloating", 2),
		new ScoreEntry("WaterPowerUpFloating", 2),
		new ScoreEntry("Barrier", 1),
		new ScoreEntry("Walker", 1),
		new ScoreEntry("Tank", 1),
		new ScoreEntry("RepGunship", 2),
		new ScoreEntry("TIE_Fighter", 2),
		new ScoreEntry("Trump", 3),
	};
	
	public float spawnCycle = 0.5f;
	public float objectSpeed = -20.0f;

	public float soundLevel = 1.0f;
	public bool enableJump = false;
	public bool enableShake = false;
	public bool enableSound = false; //so you could listen to your favorite music in the bg
	public int currentLevel = 1;
	public bool isPaused = false;

	//KEY - MAPPING
	public KeyCode JUMP_KEY = KeyCode.Space;
	public KeyCode LEFT_KEY = KeyCode.A;
	public KeyCode RIGHT_KEY = KeyCode.D;

	public KeyCode SCALE_UP_KEY = KeyCode.Q;
	public KeyCode SCALE_DOWN_KEY = KeyCode.E;
	public KeyCode SHIELD_UP_KEY = KeyCode.R;
	public KeyCode SHIELD_DOWN_KEY = KeyCode.T;
	public KeyCode SHAKE_KEY = KeyCode.S;
	public KeyCode CONTINUE_KEY = KeyCode.F;
	//spawn
	//Csv files
	public TextAsset spawnCsvFile;
}
