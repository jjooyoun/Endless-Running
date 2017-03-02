using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    //public GameObject powerup;
    //public GameObject obstacle;
    public GameObject[] Spawners;
    public Transform[] lanes;
	public bool spawning = false;
    float timeElapsed = 0;
    float spawnCycle = 0.5f;

	private int centerLaneIndex = 2;

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
	int[]  a = {POWERUP_INDEX, POWERUP_INDEX};

	void Start () {
		EventManager.instance.level1AchievementEvent.AddListener (unlockLev2);
		//EventManager.instance.level2AchievementEvent.AddListener (unlockLev3);
		EventManager.instance.level3AchievementEvent.AddListener (unlockLev4);
		//EventManager.instance.level4AchievementEvent.AddListener (unlockLev5);
		EventManager.instance.level5AchievementEvent.AddListener (unlockLev7);
		//EventManager.instance.level7AchievementEvent.AddListener(unlockLev8);
	}

	void unlockLev2() {
		a = new int[] {POWERUP_INDEX, WALKER_INDEX};
	}

	void unlockLev4()
	{
		a = new int[] { POWERUP_INDEX, WALKER_INDEX, GATE_INDEX };
	}

	void unlockLev7() {
		a = new int[] { POWERUP_INDEX, WALKER_INDEX, GATE_INDEX, SHIELD_INDEX };
	}

    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > spawnCycle)
        {
            /*if (spawnPowerup)
            {
                Debug.Log("spawning powerup");
                // GameObject tmp = (GameObject)Instantiate(obstacle, new Vector3(0, 1, 42), Quaternion.identity);
                GameObject tmp = (GameObject)GameObjectUtil.Instantiate(powerup, new Vector3(0, 1, 42));
                //Vector3 pos = obstacle.transform.position;
                tmp.transform.position = lanes[Random.Range(0, lanes.Length)].position;
            } else
            {
                Debug.Log("Spawning obstacle");
                // GameObject tmp = (GameObject)Instantiate(obstacle, new Vector3(0, 1, 42), Quaternion.identity);
                GameObject tmp = (GameObject)GameObjectUtil.Instantiate(obstacle, new Vector3(0, 1, 42));
                //Vector3 pos = obstacle.transform.position;
                tmp.transform.position = lanes[Random.Range(0, lanes.Length)].position;
            }
            timeElapsed -= spawnCycle;
            spawnPowerup = !spawnPowerup;*/
            //int spawnerIndex = Random.Range(0, Spawners.Length);

			int spawnerIndex = Random.Range(0, a.Length);
			int randomZ = Random.Range (2, 36);
			Vector3 spawnerPos = new Vector3 (0, 1, randomZ);//Spawners[a[spawnerIndex]].transform.position;//new Vector3 (0, 1, randomZ);
			//Debug.Log("pos  = :" + spawnerPos);
			Vector3 lanePos = lanes[Random.Range(0, lanes.Length)].position;


            //GameObject tmp = (GameObject)GameObjectUtil.Instantiate(Spawners[spawnerIndex], new Vector3(0, 1, 42));
			//GameObject tmp = (GameObject)GameObjectUtil.Instantiate(Spawners[0], new Vector3(0, 1, randomZ));
            //tmp.transform.position = lanes[Random.Range(0, lanes.Length)].position;
			//tmp.transform.position = lanes[Random.Range(0, lanes.Length)].position;
			GameObject tmp1 = (GameObject)GameObjectUtil.Instantiate(Spawners[a[spawnerIndex]], spawnerPos);
			if (spawnerIndex == 8) {
				tmp1.transform.position = lanes [centerLaneIndex].position;
			} else
				tmp1.transform.position = lanes[Random.Range(0, lanes.Length)].position;
			//GameObject tmp2 = (GameObject)GameObjectUtil.Instantiate(Spawners[1], new Vector3(0, 1, 1));
			//tmp2.transform.position = lanes[2].position;
            timeElapsed -= spawnCycle;
        }
    }
}