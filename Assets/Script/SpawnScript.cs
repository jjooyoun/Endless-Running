using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    public GameObject obstacle;

    float timeElapsed = 0;
    float spawnCycle = 0.5f;

    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > spawnCycle)
        {
            GameObject temp;
            temp = (GameObject)Instantiate(obstacle, new Vector3(0, 1, 42), Quaternion.identity);
            Vector3 pos = temp.transform.position;
            temp.transform.position = new Vector3(Random.Range(-3, 4), pos.y, pos.z);
            timeElapsed -= spawnCycle;
        }
    }
}
