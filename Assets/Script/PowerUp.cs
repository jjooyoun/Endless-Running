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

	private static void PowerUpScaleUp(Entity ent, Entity powerUp){
		//hide power up
		PowerUp th1s = (PowerUp)powerUp;
		th1s.gameObject.SetActive (false);
		//scale entity
		ScaleUp (ent.gameObject.transform);
	}

	private static void PowerUpScaleDown(Entity ent, Entity powerUp){
		PowerUp th1s = (PowerUp)powerUp;
		th1s.gameObject.SetActive (false);
		ScaleDown (ent.gameObject.transform);
	}

   
	private static bool Scale(Transform entTransform, float scalingFactor){
		entTransform.localScale += new Vector3 (scalingFactor, scalingFactor, scalingFactor);
		entTransform.transform.position += new Vector3 (0.0f, scalingFactor * 0.5f, 0.0f);
		return true;
	}
		
	private static Action<Entity, Entity>[] colliderHandlers = {
        PowerUpScaleUp,
        PowerUpScaleDown
    };

	public PowerUpType type = PowerUpType.SCALE_UP;

	void Start(){
		entityType = ENTITY_TYPE.POWER_UP;
	}

	public void PowerUpHandler(Entity ent, Entity th1s){
		colliderHandlers [(int)this.type] (ent, th1s);
	}
		
}