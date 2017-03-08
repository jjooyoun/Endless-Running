using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//nothing
[RequireComponent (typeof (Collider))]
public class Entity : MonoBehaviour {
    public GameObject child;
	private AudioSource audioSource;

    System.Guid id = System.Guid.NewGuid();

	public enum ENTITY_TYPE{
		PLAYER,
		ENEMY,		//big to attack
		OBSTACLE, //small to attack
		POWER_UP
	};

	public ENTITY_TYPE entityType =  ENTITY_TYPE.PLAYER;
	public string entityName = "Entity";

	public void Init(){
		//Debug.Log (entityName);
		audioSource = GetComponent<AudioSource> ();
		//Debug.Log ("audioSource:" + audioSource);
//		if (audioSource && audioClip) {
//			//Debug.Log ("name:" + entityName);
//			//Debug.Log ("setting audio clip to :" + audioClip.name);
//			audioSource.clip = audioClip;
//		}
	}

	void Start(){
		
		if (entityType == ENTITY_TYPE.PLAYER && Setting.Instance.gameSetting.gameMode == GameSetting.GameMode.TEST) {
			EventManager.Instance.shield.AddListener (OnShieldUp);
			EventManager.Instance.shieldDownEvent.AddListener (OnShieldDown);
			EventManager.Instance.scaleUpEvent.AddListener (OnScaleUp);
			EventManager.Instance.scaleDownEvent.AddListener (OnScaleDown);
		}
	}

	void OnScaleUp(){
		if (entityType == ENTITY_TYPE.PLAYER && Setting.Instance.gameSetting.gameMode == GameSetting.GameMode.TEST) {
			Debug.Log ("Scale up" + name);
			PowerUp.ScaleUp (transform);
		}
	}

	void OnScaleDown(){
		if (entityType == ENTITY_TYPE.PLAYER && Setting.Instance.gameSetting.gameMode == GameSetting.GameMode.TEST)
			PowerUp.ScaleDown (transform);
	}

	void OnShieldUp(){
		if (entityType == ENTITY_TYPE.PLAYER && Setting.Instance.gameSetting.gameMode == GameSetting.GameMode.TEST) {
			GameObject shield = (GameObject)GameObject.Instantiate (Resources.Load ("Prefabs/Shield"));
			PowerUp.PowerUpShieldUp (this, shield.GetComponent<PowerUp>());
		}
	}

	void OnShieldDown(){
		if (entityType == ENTITY_TYPE.PLAYER && Setting.Instance.gameSetting.gameMode == GameSetting.GameMode.TEST)
			PowerUp.PowerUpShieldDown (this);
	}




	//player v.s other
	private void OnTriggerEnter(Collider other){
		Debug.Log (name + "collided with:" + other.name);
		//player-first
		if (entityType != ENTITY_TYPE.PLAYER) {
			return;
		}
		//send colliding event accordingly
		Entity otherEnt = other.gameObject.GetComponent<Entity>();
		if (otherEnt) {
			if (otherEnt.entityType == ENTITY_TYPE.PLAYER || otherEnt.entityType == entityType) { // what if we have to handle other player ? NOTE to make use of guid later on
				return;
			}
//			Debug.Log (otherEnt.name);
//			Debug.Log ("audio source?:" + otherEnt.audioSource);
//			Debug.Log ("clip?:" + otherEnt.audioClip);
			if (otherEnt.audioSource && otherEnt.audioSource.clip && !otherEnt.audioSource.isPlaying) {
				Debug.Log ("Play clip:" + otherEnt.audioSource.clip.name);
				otherEnt.audioSource.Play ();
			}

            //player collided with powerup
			if (otherEnt.entityType == ENTITY_TYPE.POWER_UP) {
				//Debug.Log ("powerup!!!");
				PowerUp.PowerUpHandler (this, otherEnt); // let powerup fire event
			} else if (otherEnt.entityType == ENTITY_TYPE.ENEMY || otherEnt.entityType == ENTITY_TYPE.OBSTACLE) {
				//has shield just destroying shield without broadcast event
				if (PowerUp.hasShield) {
					Debug.Log ("has Shield");
					PowerUp.PowerUpShieldDown (this);
					//OnShieldDown();
					return;
				}

				if (otherEnt.entityType == ENTITY_TYPE.ENEMY && this.gameObject.transform.localScale.x > otherEnt.gameObject.transform.localScale.x) {
					EventManager.Instance.entEnemyCollisionEvent.Invoke (this, otherEnt);
				} else if (otherEnt.entityType == ENTITY_TYPE.OBSTACLE && this.gameObject.transform.localScale.x < otherEnt.gameObject.transform.localScale.x) {
					EventManager.Instance.entObstacleCollisionEvent.Invoke (this, otherEnt);
				} else {
					//flash lose live
					EventManager.Instance.FlashAndLoseLiveEvent.Invoke (this, otherEnt);
				}
			}
		}
	}
}
