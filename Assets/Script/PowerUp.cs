using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class PowerUp : Entity {

	private static float SCALING_FACTOR = 0.15f;
	private static int LEVEL = 5;
	private static float MAX_SCALE = 1.0f + LEVEL*SCALING_FACTOR;
    public static bool hasShield = false;
    private static float OFFSET_SHIELD = 0.15f;

	private static float shieldDownSec = 5.0f;
	//private static int level = 0; for debug purpose


	public enum PowerUpType{
		SCALE_UP = 0,
		SCALE_DOWN,
        SHIELD,
		SHRINK,
		JAPANESE_GAME
	};

	public Material invisibleMaterial;
	private Material oldMaterial;

    public static bool ScaleUp(Transform entTransform)
    {
        if (entTransform.localScale.x > MAX_SCALE)
            return false;
        return Scale(entTransform, PowerUp.SCALING_FACTOR);
    }

    public static bool ScaleDown(Transform entTransform)
    {
        if (entTransform.localScale.x <= 1)
            return false;
        return Scale(entTransform, -PowerUp.SCALING_FACTOR);
    }


	private static void PowerUpScaleUp(Entity ent, Entity powerUp){
		//hide power up
		PowerUp th1s = (PowerUp)powerUp;
		th1s.Invisiblify(true);
		//th1s.gameObject.SetActive (false);
		//scale entity
		ScaleUp (ent.gameObject.transform);
		EventManager.Instance.entPowerupCollisionEvent.Invoke(ent, powerUp);
	}

	private static void PowerUpScaleDown(Entity ent, Entity powerUp){
		PowerUp th1s = (PowerUp)powerUp;
		th1s.gameObject.SetActive (false);
		ScaleDown (ent.gameObject.transform);
		EventManager.Instance.entPowerupCollisionEvent.Invoke(ent, powerUp);
	}

    public static void PowerUpShieldUp(Entity ent, Entity powerUp)
    {
        //already have shield?
        if (!hasShield)
        {
            //Debug.Log("creating sphere");
            ent.child = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ent.child.GetComponent<SphereCollider>().enabled = false;
            ent.child.transform.position = ent.transform.position;
			ent.child.transform.localScale = new Vector3(ent.transform.localScale.x + OFFSET_SHIELD, ent.transform.localScale.y + OFFSET_SHIELD, ent.transform.localScale.z + OFFSET_SHIELD);
            ent.child.transform.parent = ent.transform;//inherit rotation
            //ent.transform.position += new Vector3(0.0f, OFFSET_SHIELD * 0.5f, 0.0f); // floating inside
			ent.child.GetComponent<Renderer>().material = powerUp.GetComponent<Renderer>().material;
			PowerUp th1s = (PowerUp)powerUp;
			th1s.Invisiblify(true);
			hasShield = !hasShield;
			ent.Invoke("ShieldDownWrapper", shieldDownSec);
			EventManager.Instance.entPowerupCollisionEvent.Invoke (ent, powerUp);
			EventManager.Instance.shield.Invoke();
        }
    }

    public static void PowerUpShieldDown(Entity ent)
    {
        if (hasShield)
        {
            //Debug.Log("destroying sphere");
            //GameObjectUtil.Destroy(ent.child);
			Destroy(ent.child); 
			//ent.transform.position -= new Vector3(0.0f, OFFSET_SHIELD * 0.5f, 0.0f); // back to the ground
			hasShield = !hasShield;
        }
    }
   
	private static bool Scale(Transform entTransform, float scalingFactor){
		entTransform.localScale += new Vector3 (scalingFactor, scalingFactor, scalingFactor);
		entTransform.transform.position += new Vector3 (0.0f, scalingFactor * 0.5f, 0.0f);
		return true;
	}
		
	private static Action<Entity, Entity>[] colliderHandlers = {
        PowerUpScaleUp,
        PowerUpScaleDown,
        PowerUpShieldUp,
    };

	public PowerUpType powerUptype = PowerUpType.SCALE_UP;

	public void Invisiblify(bool hide){
		if(hide && invisibleMaterial){
			GetComponent<Renderer>().material = invisibleMaterial;
		}else{
			GetComponent<Renderer>().material = oldMaterial;
		}
		
	}

	void Start(){
		entityType = ENTITY_TYPE.POWER_UP;
		base.Init ();
		oldMaterial = GetComponent<Renderer>().material;
    }

	public static void PowerUpHandler(Entity ent, Entity th1s){
		colliderHandlers [(int)((PowerUp)th1s).powerUptype] (ent, th1s);
	}
		
}