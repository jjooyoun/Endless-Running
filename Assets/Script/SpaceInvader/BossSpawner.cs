using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour {
	public GameObject spawner;
	public float DistDivider = 6;
	public bool shootable = false;
	public int rowShootable = 3;
	public string AlienType;
	public float diameterX = 5.0f;
	public float diameterY = 5.0f;
	public int deads = 0;
	// Use this for initialization

	private Boss bossRef;
	void Spawn(Vector3 Location){
		GameObject bossGo = Instantiate (spawner,Location,Quaternion.identity);
		Boss boss = bossGo.GetComponent<Boss> ();
		boss.Init(Location.x, Location.x + diameterX, Location.y + diameterY/2.0f, Location.y - diameterY/2.0f);
		boss.shootable = true;
		boss.startMoving = true;
		bossRef = boss;
		//Debug.Log ("Boss spawn");
	}

//	void Start(){
//		Spawn (transform.position);
//	}

	public void OnPossibleSpawn(int row){
		if(rowShootable == row){
			Spawn (transform.position);
		}
	}

	public void OnGameOver(){
		Destroy(bossRef.gameObject);
	}
}
