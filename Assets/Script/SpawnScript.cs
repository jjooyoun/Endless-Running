﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    public GameObject obstacle;
	public Transform[] lanes;
    float timeElapsed = 0;
    float spawnCycle = 0.5f;

    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > spawnCycle)
        {
            GameObject tmp = (GameObject)Instantiate(obstacle, new Vector3(0, 1, 42), Quaternion.identity);
			//Vector3 pos = obstacle.transform.position;
			tmp.transform.position = lanes[Random.Range (0, lanes.Length)].position;
            timeElapsed -= spawnCycle;
        }
    }
}
