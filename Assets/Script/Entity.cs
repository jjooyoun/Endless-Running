using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//nothing
[RequireComponent (typeof (Collider))]
public class Entity : MonoBehaviour {
    public GameObject child;
    System.Guid id = System.Guid.NewGuid();

	public enum ENTITY_TYPE{
		PLAYER,
		ENEMY,		//big to attack
		OBSTACLE, //small to attack
		POWER_UP
	};

	public ENTITY_TYPE entityType =  ENTITY_TYPE.PLAYER;
	public string entityName = "Entity";

	private void OnTriggerEnter(Collider other){
		//send colliding event accordingly
		Entity otherEnt = other.gameObject.GetComponent<Entity>();
		if (otherEnt) {
			if (otherEnt.entityType == ENTITY_TYPE.PLAYER || otherEnt.entityType == entityType) { // what if we have to handle other player ? NOTE to make use of guid later on
				return;
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
					return;
				}

				if (otherEnt.entityType == ENTITY_TYPE.ENEMY && this.gameObject.transform.localScale.x > otherEnt.gameObject.transform.localScale.x) {
					EventManager.instance.entEnemyCollisionEvent.Invoke (this, otherEnt);
				} else if (otherEnt.entityType == ENTITY_TYPE.OBSTACLE && this.gameObject.transform.localScale.x < otherEnt.gameObject.transform.localScale.x) {
					EventManager.instance.entObstacleCollisionEvent.Invoke (this, otherEnt);
				} else {
					//flash lose live
					EventManager.instance.FlashAndLoseLiveEvent.Invoke (this, otherEnt);
				}
			}
		}
	}
}
