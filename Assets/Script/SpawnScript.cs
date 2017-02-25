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


	// 0 Power Up
	// 1 Obstacle
	// 2 Shield
	// 3 Enemy
	int[] a = {1, 1 ,1, 1, 3, 3, 3, 3, 2};

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
            //GameObject tmp = (GameObject)GameObjectUtil.Instantiate(Spawners[spawnerIndex], new Vector3(0, 1, 42));
			GameObject tmp = (GameObject)GameObjectUtil.Instantiate(Spawners[0], new Vector3(0, 1, randomZ));
            //tmp.transform.position = lanes[Random.Range(0, lanes.Length)].position;
			tmp.transform.position = lanes[Random.Range(0, lanes.Length)].position;
			GameObject tmp1 = (GameObject)GameObjectUtil.Instantiate(Spawners[a[spawnerIndex]], new Vector3(0, 1, randomZ));
			tmp1.transform.position = lanes[Random.Range(0, lanes.Length)].position;
			//GameObject tmp2 = (GameObject)GameObjectUtil.Instantiate(Spawners[1], new Vector3(0, 1, 1));
			//tmp2.transform.position = lanes[2].position;
            timeElapsed -= spawnCycle;
        }
    }
}