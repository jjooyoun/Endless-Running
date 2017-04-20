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
		EventManager.Instance.spawnerDestroyedEvent.AddListener (OnSpawnerDestroyed);
	}

	void OnTriggerEnter (Collider other) {
		//Debug.Log ("hello:" + other.name);
		Entity ent = other.GetComponent<Entity>();
		//Debug.Log("ent:" + ent);
		if (ent) {
			spawnerDestroyed++;
			Entity.DisableMeshCutOut(ent);
			ent.Invisiblify(false);
			CheckNextLevel();
			EventManager.Instance.percentCompleteEvent.Invoke((float)spawnerDestroyed/spawnerTotal);
		}
        GameObjectUtil.Destroy(other.gameObject);
	}


	public void CheckNextLevel(){
		if( spawnerTotal != -1 && spawnerDestroyed >= spawnerTotal && finishedSpawning){
			EventManager.Instance.levelFinishedEvent.Invoke();
		}
	}
	public void SetSpawnerNum(int spawnerNum){
		spawnerTotal = spawnerNum;
		Debug.Log("spawnerTotal:" + spawnerTotal);
		CheckNextLevel();
	}

	public void OnSpawnerDestroyed(Entity ent){
		spawnerDestroyed++;
		Entity.DisableMeshCutOut(ent);
		PowerUp pu = ent.GetComponent<PowerUp>();
		if(pu){
			pu.Invisiblify(false);
		}
		GameObjectUtil.Destroy(ent.gameObject);
		CheckNextLevel();
		EventManager.Instance.percentCompleteEvent.Invoke((float)spawnerDestroyed/spawnerTotal);
	}

	public void FinishedSpawning(){
		finishedSpawning = true;
		Debug.Log("finished spawning!!!");
		CheckNextLevel();
	}

}
