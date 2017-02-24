using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    //public GameObject powerup;
    //public GameObject obstacle;
    public GameObject[] Spawners;
    public Transform[] lanes;
    float timeElapsed = 0;
    float spawnCycle = 0.5f;
    bool spawnPowerup = true;

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
            int spawnerIndex = Random.Range(0, Spawners.Length);
            GameObject tmp = (GameObject)GameObjectUtil.Instantiate(Spawners[spawnerIndex], new Vector3(0, 1, 42));
            tmp.transform.position = lanes[Random.Range(0, lanes.Length)].position;
            timeElapsed -= spawnCycle;
        }
    }
}
