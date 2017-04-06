using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnScript : MonoBehaviour {
	public CSVParse csvParser;
    public GameObject[] Spawners;
    public Transform[] lanes;
	public bool spawning = false;
    float timeElapsed = 0;
    float spawnCycle = 20f;
	private int centerLaneIndex = 2;

	private static int POWERUP_INDEX = 0;
	private static int GATE_INDEX = 1;
	private static int SHIELD_INDEX = 2;
	private static int WALKER_INDEX = 3;
	private static int BARRIER_INDEX = 4;
	private static int VOLCANO_INDEX = 5;

	//int[] a = {0, 0, 0, 0, 1, 1 ,1, 1, 4, 3, 3, 3, 3, 2};
	int[]  a = {POWERUP_INDEX, POWERUP_INDEX};

	public static Stack<int> objectPool = new Stack<int> ();

	public bool tutorialMode = true;

	int currentRow = 0;

	

	void Start () {
		Debug.Log ("SpawnScript start()");
		spawnCycle = Setting.gameSetting.spawnCycle;
		//Debug.Log ("spawnCycle:" + spawnCycle);
		EventManager.Instance.level1AchievementEvent.AddListener (unlockLev2);
		EventManager.Instance.level2AchievementEvent.AddListener (unlockLev3);
		EventManager.Instance.level3AchievementEvent.AddListener (unlockLev4);
		//EventManager.Instance.level4AchievementEvent.AddListener (unlockLev5);
		EventManager.Instance.level5AchievementEvent.AddListener (unlockLev7);
		tutorialMode = Setting.gameSetting.gameMode == GameSetting.GameMode.TUTORIAL;
		//EventManager.Instance.level7AchievementEvent.AddListener(unlockLev8);
		//EventManager.Instance.stage1.AddListener (goStage1);
	}

	void goStage1(){
		Debug.Log ("Go Stage 1");
		int PowerUpNum = 30;
		int GateNum = 10;
		int ShieldNum = 5;
		int WalkerNum = 20;
		int BarrierNum = 20;
		int VolcanoNum = 10;

		// Populate the stacks
			
		while (PowerUpNum > 0 || GateNum > 0 || ShieldNum > 0 || WalkerNum > 0 || BarrierNum > 0) {
			int randIndex = UnityEngine.Random.Range (0, 5);
			if (randIndex == 0 && PowerUpNum > 0) {
				objectPool.Push (POWERUP_INDEX);
				PowerUpNum--;
			} else if (randIndex == 1 && GateNum > 0) {
				objectPool.Push (GATE_INDEX);
				GateNum--;
			} else if (randIndex == 2 && ShieldNum > 0) {
				objectPool.Push (SHIELD_INDEX);
				ShieldNum--;
			} else if (randIndex == 3 && WalkerNum > 0) {
				objectPool.Push (WALKER_INDEX);
				WalkerNum--;
			} else if (randIndex == 4 && BarrierNum > 0) {
				objectPool.Push (BARRIER_INDEX);
				BarrierNum--;
			} else if (randIndex == 5 && VolcanoNum > 0){
				objectPool.Push (VOLCANO_INDEX);
				VolcanoNum--;
			}

		}

		//Debug.Log ("pre-install:" + objectPool.Count);
		tutorialMode = false; 

	}

	void unlockLev2() {
		a = new int[] {WALKER_INDEX, POWERUP_INDEX};
	}

	void unlockLev3(){
		a = new int[]{WALKER_INDEX, POWERUP_INDEX, VOLCANO_INDEX};
	}

	void unlockLev4()
	{
		a = new int[] { POWERUP_INDEX, WALKER_INDEX, GATE_INDEX };
	}

	void unlockLev7() {
		a = new int[] { POWERUP_INDEX, WALKER_INDEX, GATE_INDEX, SHIELD_INDEX, BARRIER_INDEX};
	}

	//ducho
	//e.g: 0.8f is the new default
	//default value(from prefabs) is 0.8f
	//->new value should be : 0.64f --> 0.8f*0.8f
	//RIGHT? is my math right ?
	void SetEntAudioVolume(GameObject go, float vol){
		Entity ent = go.GetComponent<Entity>();
		if(ent){
			float newVol = ent.GetComponent<AudioSource>().volume;
			newVol *= vol;
			ent.GetComponent<AudioSource>().volume = newVol;
		}
	}

	void Spawn(int laneIndex, int spawnerIndex){
		if(laneIndex == BARRIER_INDEX){
			laneIndex = centerLaneIndex;
		}
		GameObject tmp1 = Spawners[spawnerIndex];
		float y = tmp1.transform.position.y; //keep the y position
		Vector3 spawnPos = lanes [laneIndex].position;
		spawnPos = new Vector3(spawnPos.x, y, spawnPos.z);
		tmp1 = (GameObject)GameObjectUtil.Instantiate (Spawners [spawnerIndex], spawnPos); //Spawners[a[spawnIndex]]
		SetEntAudioVolume (tmp1, Setting.gameSetting.soundLevel);
	}

	void SpawnNext(List<CSVParse.Row> rows, int rowIndex){
		CSVParse.Row row = rows [rowIndex];
		for(int i = 0; i < row.lanes.Length;i++){
			int laneIndex = i;
			int spawnType = row.lanes [i];
			//Debug.Log ("laneIndex:" + laneIndex);
			//Debug.Log ("spawnType:" + spawnType);
			if (spawnType != -1) {
				Spawn(laneIndex, spawnType);
			}

		}
	}

    void Update()
    {
		if (!spawning || ( !tutorialMode && !csvParser.isLoaded))
			return;
        timeElapsed += Time.deltaTime;

		// Tutorial Mode
		if (timeElapsed > spawnCycle && tutorialMode == true) {
			int spawnerIndex = a[UnityEngine.Random.Range (0, a.Length)];
			int randomZ = UnityEngine.Random.Range (2, 36);
			int laneIndex = UnityEngine.Random.Range (0, lanes.Length);
			Spawn(laneIndex, spawnerIndex);
			timeElapsed -= spawnCycle;
		}

		if (timeElapsed > spawnCycle * currentRow && tutorialMode == false) {
			//Debug.Log("numRows:" + csvParser.NumRows());
			if (currentRow == csvParser.NumRows()) { // finished spawning
				return;
			}
			SpawnNext (csvParser.rowList, currentRow);
			currentRow++;
			//Debug.Log ("currentRow:" + currentRow);

		}
    }
}