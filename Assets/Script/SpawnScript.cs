using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnScript : MonoBehaviour {
	public CSVParse csvParser;
    //public GameObject powerup;
    //public GameObject obstacle;
    public GameObject[] Spawners;
    public Transform[] lanes;
	public bool spawning = false;
    float timeElapsed = 0;
    float spawnCycle = 20f;
	//public bool isPopDone = false;

	private int centerLaneIndex = 2;

	private static int POWERUP_INDEX = 0;
	private static int GATE_INDEX = 1;
	private static int SHIELD_INDEX = 2;
	private static int WALKER_INDEX = 3;
	private static int BARRIER_INDEX = 4;

	// 0 Power Up
	// 1 Gate
	// 2 Shield
	// 3 Enemy
	// 4 Barrier
	//int[] a = {0, 0, 0, 0, 1, 1 ,1, 1, 4, 3, 3, 3, 3, 2};
	int[]  a = {POWERUP_INDEX, POWERUP_INDEX};


	public static Stack<int> PowerUpSt = new Stack<int> ();
	public static Stack<int> GateSt = new Stack<int> ();
	public static Stack<int> ShieldSt = new Stack<int>();
	public static Stack<int> WalkerSt = new Stack<int>();
	public static Stack<int> BarrierSt = new Stack<int>();
	public static Stack<int> objectPool = new Stack<int> ();

	public bool tutorialMode = true;

	//make sure it receive the event
	void OnEnable(){
		//Debug.Log ("SpawnScript onENable()");
		//EventManager.Instance.stage1.AddListener (goStage1);
	}

//	void OnDisable(){
//		EventManager.Instance.stage1.RemoveListener (goStage1);
//	}

//	void Awake(){
//		Debug.Log ("SpawnScript wake()");
//		//EventManager.Instance.stage1.AddListener (goStage1);
//	}

	int currentRow = 0;

	void SpawnNext(List<CSVParse.Row> rows, int rowIndex){
		CSVParse.Row row = rows [rowIndex];
		for(int i = 0; i < row.lanes.Length;i++){
			int laneIndex = i;
			int spawnType = row.lanes [i];
			Debug.Log ("laneIndex:" + laneIndex);
			Debug.Log ("spawnType:" + spawnType);
			if (spawnType != -1) {
				GameObject tmp1 = (GameObject)GameObjectUtil.Instantiate (Spawners [spawnType], Vector3.zero);
				tmp1.transform.position = lanes [laneIndex].position;
				SetEntAudioVolume (tmp1, Setting.gameSetting.soundLevel);
			}

		}
	}

	void Start () {
		Debug.Log ("SpawnScript start()");
		spawnCycle = Setting.gameSetting.spawnCycle;
		//Debug.Log ("spawnCycle:" + spawnCycle);
		EventManager.Instance.level1AchievementEvent.AddListener (unlockLev2);
		//EventManager.Instance.level2AchievementEvent.AddListener (unlockLev3);
		EventManager.Instance.level3AchievementEvent.AddListener (unlockLev4);
		//EventManager.Instance.level4AchievementEvent.AddListener (unlockLev5);
		EventManager.Instance.level5AchievementEvent.AddListener (unlockLev7);
		tutorialMode = Setting.gameSetting.gameMode == GameSetting.GameMode.TUTORIAL;
		//Debug.Log ("Spawn Script done!!");
		//a = csvParser.Load();
		//a = Setting.gameSetting.a;
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
			}

		}

		//Debug.Log ("pre-install:" + objectPool.Count);
		tutorialMode = false; 

	}

	void unlockLev2() {
		a = new int[] {POWERUP_INDEX, WALKER_INDEX};
	}

	void unlockLev4()
	{
		a = new int[] { POWERUP_INDEX, WALKER_INDEX, GATE_INDEX };
	}

	void unlockLev7() {
		//Debug.Log ("here?");
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
    void Update()
    {
		if (!spawning || !csvParser.isLoaded)
			return;
        timeElapsed += Time.deltaTime;

		// Tutorial Mode
		if (timeElapsed > spawnCycle && tutorialMode == true) {

			int spawnerIndex = UnityEngine.Random.Range (0, a.Length);
			int randomZ = UnityEngine.Random.Range (2, 36);
			Vector3 spawnerPos = new Vector3 (0, 1, randomZ);//Spawners[a[spawnerIndex]].transform.position;//new Vector3 (0, 1, randomZ);
			//Debug.Log("pos  = :" + spawnerPos);
			Vector3 lanePos = lanes [UnityEngine.Random.Range (0, lanes.Length)].position;

			GameObject tmp1 = (GameObject)GameObjectUtil.Instantiate (Spawners [a [spawnerIndex]], spawnerPos);
			if (spawnerIndex == 4) {
				tmp1.transform.position = lanes [centerLaneIndex].position;
			} else
				tmp1.transform.position = lanes [UnityEngine.Random.Range (0, lanes.Length)].position;
			
			//GameObject tmp2 = (GameObject)GameObjectUtil.Instantiate(Spawners[1], new Vector3(0, 1, 1));
			//tmp2.transform.position = lanes[2].position;
			
			SetEntAudioVolume(tmp1, Setting.gameSetting.soundLevel); //1.0f is the default
			timeElapsed -= spawnCycle;


		}
		// Game Mode
		/*else if (timeElapsed > spawnCycle && tutorialMode == false) {
			//Debug.Log ("game mode");
			if (objectPool.Count > 0) {
				Debug.Log ("spawning sth:");
				int spawnIndex = objectPool.Pop ();

				int randomZ = Random.Range (2, 36);
				Vector3 spawnerPos = new Vector3 (0, 1, randomZ);

				GameObject tmp1 = (GameObject)GameObjectUtil.Instantiate (Spawners [spawnIndex], spawnerPos);
				if (spawnIndex == 4) {
					tmp1.transform.position = lanes [centerLaneIndex].position;
				} else
					tmp1.transform.position = lanes [Random.Range (0, lanes.Length)].position;
				SetEntAudioVolume(tmp1, Setting.gameSetting.soundLevel);
				if (objectPool.Count == 0) {
					Debug.Log ("end game");
				}
			}

			timeElapsed -= spawnCycle;
		}*/

		if (timeElapsed > spawnCycle * currentRow && tutorialMode == false) {
			//Debug.Log("numRows:" + csvParser.NumRows());
			if (currentRow == csvParser.NumRows()) { // finished spawning
				return;
			}
			SpawnNext (csvParser.rowList, currentRow);
			currentRow++;

			Debug.Log ("currentRow:" + currentRow);

		}
    }
}