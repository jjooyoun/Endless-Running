using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class PowerUp : Entity {
	public ParticleSystem PowerupFX;
	private static float SCALING_FACTOR = 0.15f;
	private static int LEVEL = 5;
	public static float MAX_SCALE = 1.0f + LEVEL*SCALING_FACTOR;
    public static bool hasShield = false;

	public static bool hasWater = false;

	public static bool hasFire = false;

    private static float OFFSET_SHIELD = 0.15f;

	private static float shieldDownSec = 5.0f;
	//private static int level = 0; for debug purpose


	public enum PowerUpType{
		SCALE_UP = 0,
		SCALE_DOWN,
		FIRE,
        SHIELD,
		WATER,
		SHRINK,
		JAPANESE_GAME
	};

	public Material invisibleMaterial;
	private Material oldMaterial;


	private static bool Scale(Transform entTransform, float scalingFactor){
		entTransform.localScale += new Vector3 (scalingFactor, scalingFactor, scalingFactor);
		entTransform.transform.position += new Vector3 (0.0f, scalingFactor * 0.5f, 0.0f);
		return true;
	}

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
		//Invisiblify(true);
		//th1s.gameObject.SetActive (false);
		//scale entity
		if(!ScaleUp (ent.gameObject.transform)){
			//ent.PlayInternalFX(Entity.MaxScaleFXIndex);
			ent.IsAtMaxScale = true;
		}
		EventManager.Instance.entPowerupCollisionEvent.Invoke(ent, powerUp);
	}

	private static void PowerUpScaleDown(Entity ent, Entity powerUp){
		PowerUp th1s = (PowerUp)powerUp;
		//th1s.gameObject.SetActive (false);
		// if(ScaleDown (ent.gameObject.transform)){
		// 	ent.StopInternalFX();
		// }
		EventManager.Instance.entPowerupCollisionEvent.Invoke(ent, powerUp);
	}

	static GameObject CreateSphereChild(Transform ent, float offset, Material mat){
		GameObject c =  GameObject.CreatePrimitive(PrimitiveType.Sphere);
		c.GetComponent<SphereCollider>().enabled = false;
		c.transform.position = ent.position;
		c.transform.localScale = new Vector3(ent.transform.localScale.x + offset, ent.transform.localScale.y + offset, ent.transform.localScale.z + offset);
		c.transform.parent = ent;
		c.GetComponent<Renderer>().material = mat;
		return c;
	}

    public static void PowerUpShieldUp(Entity ent, Entity powerUp)
    {
		Debug.Log("ShieldUp!!!");
        //already have shield?
        if (!hasShield)
        {
            //Debug.Log("creating sphere");
            // ent.child = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // ent.child.GetComponent<SphereCollider>().enabled = false;
            // ent.child.transform.position = ent.transform.position;
			// ent.child.transform.localScale = new Vector3(ent.transform.localScale.x + OFFSET_SHIELD, ent.transform.localScale.y + OFFSET_SHIELD, ent.transform.localScale.z + OFFSET_SHIELD);
            // ent.child.transform.parent = ent.transform;//inherit rotation
            // ////ent.transform.position += new Vector3(0.0f, OFFSET_SHIELD * 0.5f, 0.0f); // floating inside
			// ent.child.GetComponent<Renderer>().material = powerUp.GetComponent<Renderer>().material;
			ent.child = CreateSphereChild(ent.transform, OFFSET_SHIELD, powerUp.GetComponent<Renderer>().material);
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
		Debug.Log("ShieldDown!!!");
        if (hasShield)
        {
            //Debug.Log("destroying sphere");
            //GameObjectUtil.Destroy(ent.child);
			Destroy(ent.child); 
			//ent.transform.position -= new Vector3(0.0f, OFFSET_SHIELD * 0.5f, 0.0f); // back to the ground
			hasShield = !hasShield;
        }
    }

   
   public static void PowerUpWaterUp(Entity ent, Entity powerUp){
	   Debug.Log("WaterUp!!!");
	   if(!hasWater){
		   ent.child = CreateSphereChild(ent.transform, OFFSET_SHIELD, powerUp.GetComponent<Renderer>().material);
		//    ent.child = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //     ent.child.GetComponent<SphereCollider>().enabled = false;
        //     ent.child.transform.position = ent.transform.position;
		// 	ent.child.transform.localScale = new Vector3(ent.transform.localScale.x + OFFSET_SHIELD, ent.transform.localScale.y + OFFSET_SHIELD, ent.transform.localScale.z + OFFSET_SHIELD);
        //     ent.child.transform.parent = ent.transform;//inherit rotation
        //     ////ent.transform.position += new Vector3(0.0f, OFFSET_SHIELD * 0.5f, 0.0f); // floating inside
		// 	ent.child.GetComponent<Renderer>().material = powerUp.GetComponent<Renderer>().material;
		   //play
		   ent.SpawnFX = ent.PlayPEAtPosition( Resources.Load(WATER_FX_PATH) as GameObject, ent.transform.position, false, ent.transform, 1.0f);
		   PowerUp th1s = (PowerUp)powerUp;
		   th1s.Invisiblify(true);
		   hasWater = !hasWater;
		   ent.Invoke("WaterDownWrapper", shieldDownSec);
		   EventManager.Instance.entPowerupCollisionEvent.Invoke (ent, powerUp);
	   }
   }

   public static void PowerUpWaterDown(Entity ent)
    {
		Debug.Log("FireDown!!!");
        if (hasWater)
        {
            //Debug.Log("destroying sphere");
            //GameObjectUtil.Destroy(ent.child);
			Destroy(ent.child); 
			if(ent.SpawnFX)
				Destroy(ent.SpawnFX);
			//ent.transform.position -= new Vector3(0.0f, OFFSET_SHIELD * 0.5f, 0.0f); // back to the ground
			hasWater = !hasWater;
        }
    }

	public static void PowerUpFireUp(Entity ent, Entity powerUp){
		Debug.Log("FireUp!!!");
		if(!hasFire){
			//GameObject OilSplashHighRoot = (GameObject)Instantiate(Resources.Load(FIRE_PATH) as GameObject);
			//OilSplashHighRoot.transform.position = transform.position;
			//OilSplashHighRoot.transform.parent = transform;
			ent.SpawnFX = ent.PlayPEAtPosition( Resources.Load(FIRE_PATH) as GameObject, ent.transform.position, false, ent.transform, 1.0f);                    
			ParticleSystem OilSplashHighRootParticleSystem = ent.SpawnFX.GetComponent<ParticleSystem>();
			Material lavaBallMat = Resources.Load(FIRE_MAT_PATH) as Material;
			Material curBallMat = ent.GetComponent<Renderer>().material;
			ent.FireCoRoutine = ent.playParticleEffectEvery(lavaBallMat, curBallMat, OilSplashHighRootParticleSystem, OilSplashHighRootParticleSystem.main.duration, 5.0f);
			ent.StartCoroutine(ent.FireCoRoutine);
			hasFire = !hasFire;
		}
	}

	public static void PowerUpFireDown(Entity ent){
		if(hasFire){
			ent.StopCoroutine(ent.FireCoRoutine);
			ent.GetComponent<Renderer>().material = ent.OriginalMat;
			
			if(ent.SpawnFX)
				Destroy(ent.SpawnFX);
			hasFire = !hasFire;
		}
	}
	
		
	private static Action<Entity, Entity>[] colliderHandlers = {
        PowerUpScaleUp,
        PowerUpScaleDown,
		PowerUpFireUp,
        PowerUpShieldUp,
		PowerUpWaterUp
    };

	public PowerUpType powerUptype = PowerUpType.SCALE_UP;

	void Start(){
		entityType = ENTITY_TYPE.POWER_UP;
		base.Init ();
		//oldMaterial = GetComponent<Renderer>().material;
    }

	public static void PowerUpHandler(Entity ent, Entity th1s){
		colliderHandlers [(int)((PowerUp)th1s).powerUptype] (ent, th1s);
	}
		
}