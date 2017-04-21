using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour {
	
	public int spawnerTotal = -1;
	public int spawnerDestroyed = 0;
	public bool finishedSpawning = false;

	void Start(){
		EventManager.Instance.finishedSpawningEvent.AddListener(FinishedSpawning);
		EventManager.Instance.spawningNumEvent.AddListener(SetSpawnerNum);
		//EventManager.Instance.totalSpawnerEvent.AddListener(SetTotalSpawners);
		EventManager.Instance.spawnerDestroyedEvent.AddListener (OnSpawnerDestroyed);
	}

	void OnTriggerEnter (Collider other) {
		//Debug.Log ("hello:" + other.name);
		Entity ent = other.GetComponent<Entity>();
		//Debug.Log("ent:" + ent);
		// if (ent) {
		// 	spawnerDestroyed++;
		// 	Entity.DisableMeshCutOut(ent);
		// 	ent.Invisiblify(false);
		// 	CheckNextLevel();
		// 	float percent = (float)spawnerDestroyed/spawnerTotal;
		// 	//Debug.Log("percent:" + percent);
		// 	EventManager.Instance.percentCompleteEvent.Invoke(percent);
		// }
		OnSpawnerDestroyed(ent);
        if(other.gameObject) //sanity check
			GameObjectUtil.Destroy(other.gameObject);
	}


	void CheckNextLevel(){
		if( spawnerTotal != -1 && spawnerDestroyed >= spawnerTotal && finishedSpawning){
			EventManager.Instance.levelFinishedEvent.Invoke();
		}
	}
	void SetSpawnerNum(int spawnerNum){
		spawnerTotal = spawnerNum;
		Debug.Log("spawnerTotal:" + spawnerTotal);
		CheckNextLevel();
	}


	void OnSpawnerDestroyed(Entity ent){
		spawnerDestroyed++;
		if(ent){
			Entity.DisableMeshCutOut(ent);
			ent.Invisiblify(false);
			GameObjectUtil.Destroy(ent.gameObject);
			CheckNextLevel();
		}
		float percent = (float)spawnerDestroyed/spawnerTotal;
		//Debug.Log("percent:" + percent);
		EventManager.Instance.percentCompleteEvent.Invoke(percent);
	}

	void FinishedSpawning(){
		finishedSpawning = true;
		Debug.Log("finished spawning!!!");
		CheckNextLevel();
	}

}
