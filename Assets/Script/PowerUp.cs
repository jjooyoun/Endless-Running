using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PowerUp : Entity {
	private float scalingFactor = 0.25f;

	public enum PowerUpType{
		SCALE = 0,
		SHRINK,
		JAPANESE_GAME
	};

	static bool ScaleFunc(Entity ent, Collider other){
		PowerUp th1s = (PowerUp)ent;
		Debug.Log ("scaling!!!");
		if (other.transform.localScale.x > 2.0f){
			return true;
		}
		th1s.gameObject.SetActive (false);
		other.gameObject.transform.localScale += new Vector3 (th1s.scalingFactor, th1s.scalingFactor, th1s.scalingFactor);
		return true;
	}
		
	private static Func<Entity, Collider,bool>[] colliderHandlers = {
		ScaleFunc
	};

	public PowerUpType type = PowerUpType.SCALE;

	void Start(){
		entityType = ENTITY_TYPE.POWER_UP;
	}

	void OnTriggerEnter(Collider other){
		Debug.Log ("OnTriggerEnter:" + other.name);
		Entity ent = other.GetComponent<Entity> ();
		Debug.Log ("ent:" + ent);
		if(ent){
			colliderHandlers [(int)this.type] (this, other);
		}
	}
}
