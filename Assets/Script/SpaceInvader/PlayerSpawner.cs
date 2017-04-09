using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
	public GameObject spawner;
	public Player player;

	public void OnSpawn(){
		float spawnX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2.0f, 0.0f, 0.0f)).x;
		float spawnY = transform.position.y;
		GameObject go = Ent2D.Create2DGameObject(spawner, spawnX, spawnY);
        go.name = "player";
        player = go.GetComponent<Player> ();
        player.Init ();
	}

	public void OnGameOver(){
		player.OnGameOver();
	}

	public void OnPlayerDie(){
		player.OnRespawn();
	}
}
