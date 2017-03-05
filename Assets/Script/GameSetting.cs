using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "GameSetting", order = 1)]
public class GameSetting : ScriptableObject {
	public enum GameMode{
		EASY,
		MEDIUM,
		HARD,
		TUTORIAL
	};

	public GameMode gameMode = GameMode.TUTORIAL;
	public int lives = 10;
	public float spawnCycle = 0.5f;
	public float objectSpeed = -0.5f;
	public bool enableJump = false;
	public bool enableShake = false;

	public enum SPAWN_TYPE
	{
		POWERUP = 0,
		GATE,
		SHIELD,
		WALKER,
		BARRIER
	};
	private static int POWERUP_INDEX = 0;
	private static int GATE_INDEX = 1;
	private static int SHIELD_INDEX = 2;
	private static int WALKER_INDEX = 3;
	private static int BARRIER_INDEX = 4;

	// 0 Power Up
	// 1 Obstacle
	// 2 Shield
	// 3 Enemy
	// 4 Barrier
	//int[] a = {0, 0, 0, 0, 1, 1 ,1, 1, 4, 3, 3, 3, 3, 2};
	public int[]  a = {(int)SPAWN_TYPE.POWERUP, (int)SPAWN_TYPE.POWERUP};
	//spawn
}
