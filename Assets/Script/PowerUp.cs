using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : Entity {
	private static float SCALING_FACTOR = 0.15f;
	private static int LEVEL = 5;
	private static float MAX_SCALE = 1.0f + LEVEL*SCALING_FACTOR;
	//private static int level = 0; for debug purpose

	public enum PowerUpType{
		SCALE_UP = 0,
		SCALE_DOWN,
		SHRINK,
		JAPANESE_GAME
	};

    public static bool ScaleUp(Transform entTransform)
    {
        if (entTransform.localScale.x > MAX_SCALE)
            return true;
        
        return Scale(entTransform, PowerUp.SCALING_FACTOR);
    }

    public static bool ScaleDown(Transform entTransform)
    {
        if (entTransform.localScale.x <= 1)
            return true;
        return Scale(entTransform, -PowerUp.SCALING_FACTOR);
    }

    private static bool PowerUpScaleUp(Entity ent, Collider other){
		//hide power up
		PowerUp th1s = (PowerUp)ent;
		th1s.gameObject.SetActive (false);
		//scale entity
		return ScaleUp (other.gameObject.transform);
	}

	private static bool PowerUpScaleDown(Entity ent, Collider other){
		PowerUp th1s = (PowerUp)ent;
		th1s.gameObject.SetActive (false);
		return ScaleDown (other.gameObject.transform);
	}

   
	private static bool Scale(Transform entTransform, float scalingFactor){
		entTransform.localScale += new Vector3 (scalingFactor, scalingFactor, scalingFactor);
		entTransform.transform.position += new Vector3 (0.0f, scalingFactor * 0.5f, 0.0f);
		return true;
	}
		
	private static Func<Entity, Collider,bool>[] colliderHandlers = {
        PowerUpScaleUp,
        PowerUpScaleDown
    };

	public PowerUpType type = PowerUpType.SCALE_UP;

	void Start(){
		entityType = ENTITY_TYPE.POWER_UP;
	}

	void OnTriggerEnter(Collider other){
		//Debug.Log ("OnTriggerEnter:" + other.name);
		Entity ent = other.GetComponent<Entity> ();
		//Debug.Log ("ent:" + ent);
		if(ent){
			colliderHandlers [(int)this.type] (this, other);
        }
	}
		
}