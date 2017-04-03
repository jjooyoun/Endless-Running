using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour {
	
	public int spawnerTotal = -1;
	public int spawnerDestroyed = 0;

	void Start(){
		EventManager.Instance.spawningNumEvent.AddListener(SetSpawnerNum);
	}

	void OnTriggerEnter (Collider other) {
		//Debug.Log ("hello:" + other.name);
		Entity ent = other.GetComponent<Entity>();
		//Debug.Log("ent:" + ent);
		if (ent) {
			spawnerDestroyed++;
			if( spawnerTotal != -1 && spawnerDestroyed >= spawnerTotal ){
				Debug.Log("level finished!!!");
				EventManager.Instance.levelFinishedEvent.Invoke();
			}
			if(ent.GetComponent<SetRenderQueue>()){
				Debug.Log ("restore!");
				ent.GetComponent<SetRenderQueue> ().startHiding = false;
			}
		}

        GameObjectUtil.Destroy(other.gameObject);
	}

	public void SetSpawnerNum(int spawnerNum){
		spawnerTotal = spawnerNum;
		Debug.Log("spawnerTotal:" + spawnerTotal);
	}

}
