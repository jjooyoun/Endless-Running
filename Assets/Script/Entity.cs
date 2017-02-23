using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//nothing
[RequireComponent (typeof (Collider))]
public class Entity : MonoBehaviour {

	System.Guid id = System.Guid.NewGuid();

	public enum ENTITY_TYPE{
		PLAYER,
		ENEMY,
		POWER_UP
	};

	public ENTITY_TYPE entityType =  ENTITY_TYPE.PLAYER;

	private void OnTriggerEnter(Collider other){
		//send colliding event accordingly
		Entity otherEnt = other.gameObject.GetComponent<Entity>();
		if (otherEnt) {
			if (otherEnt.entityType == ENTITY_TYPE.PLAYER) { // what if we have to handle other player ? NOTE to make use of guid later on
				return;
			}

			if (otherEnt.entityType == ENTITY_TYPE.POWER_UP) {
				Debug.Log ("powerup!!!");
				EventManager.instance.entPowerupCollisionEvent.Invoke (this, otherEnt);
				((PowerUp)otherEnt).PowerUpHandler(this, otherEnt);

			} else {
				EventManager.instance.entEnemyCollisionEvent.Invoke (this, otherEnt);
			}
		}
	}
}
