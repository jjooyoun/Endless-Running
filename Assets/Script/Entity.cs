using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Utility;

//nothing
[RequireComponent (typeof (BoxCollider))]
[RequireComponent (typeof (AudioSource))]
public class Entity : MonoBehaviour {
    public GameObject child;
	//public GameObject HitFX;
	public GameObject onCollidedFX;
	private ParticleSystem ps = null;
	//public AudioSource audioSource;

    System.Guid id = System.Guid.NewGuid();

	public enum ENTITY_TYPE{
		PLAYER,
		ENEMY,		//big to attack
		OBSTACLE, //small to attack
		POWER_UP
	};

	public ENTITY_TYPE entityType =  ENTITY_TYPE.PLAYER;
	public string entityName = "Entity";

	private bool isOnFire = false;

	public bool IsOnFire{
		get{return isOnFire;}
		set{isOnFire = value;}
	}
	private bool isAtMaxScale = false;
	public bool IsAtMaxScale{
		get{return isAtMaxScale;}
		set{isAtMaxScale = value;}
	}

	private GameObject PEGameObject = null;
	public static int MaxScaleFXIndex = 0;

	public ParticleSystem[] InternalFX;
	public static readonly string LASER_BEAM_PATH = "Prefabs/LaserBeam";
	public static readonly string INVISIBLE_SPHERE_PATH = "Prefabs/InvisibleSphere";
	private static readonly string FIRE_PATH = "Prefabs/OilSpashHighRoot";
	public static readonly string FIRE_MAT_PATH = "Materials/LavaBall";
	public static readonly string WALKER_NAME = "Walker";
	public static readonly string GATE_NAME = "Gate";
	public static readonly string BARRIER_NAME = "Barrier";
	public static readonly string VOLCANO_NAME = "FirePowerUp";
	public static readonly string TIE_FIGHTER_NAME = "TIE_Fighter";

	public static readonly string MAX_SCALE_FX_PATH = "Prefabs/MaxScaleFX";
	public static readonly string WATER_FX_PATH = "Prefabs/WaterFX";

	public static readonly string WATER_FX_COLLIDER = "Water_AOE";

	

	//HIDE
	private MeshRenderer[] meshes;
	private ParticleSystem[] fxChildren;
	//public bool Invisible = false;
	//MATERIAL
	private Material originalMaterial;
	private IEnumerator isOnFireCoRoutine;

	private static readonly int FXRangeBegin = 1;
	private static readonly int FXRangeEnd = 3;

	private int currentPowerupType = -1;
	//the FX that is spawned
	protected GameObject currentSpawnFX;
	public GameObject SpawnFX{
		get{return currentSpawnFX;}
		set{currentSpawnFX = value;}
	}

	public static void ShieldDown(Entity ent){
		Debug.Log("ent:" + ent.name);
		PowerUp.PowerUpShieldDown(ent);
		//Destroy(ent.SpawnFX.gameObject);
	}

	public void StopFireFX(){
		StopCoroutine(isOnFireCoRoutine);
		GetComponent<Renderer>().material = originalMaterial;
		isOnFire = false;
		if(SpawnFX)
			Destroy(SpawnFX);
	}

	static void FireDown(Entity ent){
		//PowerUp.FireDown(ent);
		ent.StopFireFX();
	}

	static void WaterDown(Entity ent){
		PowerUp.PowerUpWaterDown(ent);
		if(ent.SpawnFX)
			Destroy(ent.SpawnFX);
	}
	//
	private static Action<Entity>[] DownCalls = {
		FireDown,
        ShieldDown,
		WaterDown
		
    };

	private static Action<Entity, Entity>[] UpCalls = {

	};

	public void Init(){
		meshes = GetComponents<MeshRenderer>();
		if(meshes.Length == 0){
			meshes = GetComponentsInChildren<MeshRenderer>();
		}
		fxChildren = GetComponentsInChildren<ParticleSystem>();
		//Debug.Log(name + " meshes:" + meshes.Length);
		if(entityType == ENTITY_TYPE.PLAYER)
			originalMaterial = GetComponent<Renderer>().material; //for player
	}

	void Start(){
		Init ();
		if (entityType == ENTITY_TYPE.PLAYER && Setting.gameSetting.gameMode == GameSetting.GameMode.TEST) {
			EventManager.Instance.shield.AddListener (OnShieldUp);
			EventManager.Instance.shieldDownEvent.AddListener (OnShieldDown);
			EventManager.Instance.scaleUpEvent.AddListener (OnScaleUp);
			EventManager.Instance.scaleDownEvent.AddListener (OnScaleDown);
		}
	}

	void OnScaleUp(){
		if (entityType == ENTITY_TYPE.PLAYER && Setting.gameSetting.gameMode == GameSetting.GameMode.TEST) {
			//Debug.Log ("Scale up" + name);
			PowerUp.ScaleUp (transform);
		}
	}

	void OnScaleDown(){
		if (entityType == ENTITY_TYPE.PLAYER && Setting.gameSetting.gameMode == GameSetting.GameMode.TEST)
			PowerUp.ScaleDown (transform);
	}

	void OnShieldUp(){
		if (entityType == ENTITY_TYPE.PLAYER && Setting.gameSetting.gameMode == GameSetting.GameMode.TEST) {
			GameObject shield = (GameObject)GameObject.Instantiate (Resources.Load ("Prefabs/Shield"));
			PowerUp.PowerUpShieldUp (this, shield.GetComponent<PowerUp>());
		}
	}

	void OnShieldDown(){
		if (entityType == ENTITY_TYPE.PLAYER && Setting.gameSetting.gameMode == GameSetting.GameMode.TEST)
			PowerUp.PowerUpShieldDown (this);
	}




	void playSoundAtPos(AudioClip clip, Vector3 position){
		if (Setting.gameSetting.enableSound && clip) {
			AudioSource.PlayClipAtPoint (clip, position);
		}
	}

	//ducho
	//e.g: 0.8f is the new default
	//default value(from prefabs) is 0.8f
	//->new value should be : 0.64f --> 0.8f*0.8f
	//RIGHT? is my math right ?
	public void SetEntAudioVolume(float vol){
		//Debug.Log("passed in vol:" + vol);
		float newVol = GetComponent<AudioSource>().volume;
		//Debug.Log("vol at audioSource:" + newVol);
		if(newVol == vol){
			return;
		}
		newVol *= vol;
		GetComponent<AudioSource>().volume = newVol;
		//Debug.Log(name + "-vol:" + newVol);
	}

	//only change the clip if sound level are different
	//what about the object having the sound on its own ?
	void playEntSoundOnCollided(Entity ent, Entity otherEnt){
		AudioSource sourceAudio = GetComponent<AudioSource>();
		AudioSource transferAudio = otherEnt.GetComponent<AudioSource>();
		AudioClip audioClip = transferAudio.clip;
		float volume = transferAudio.volume;
		if(volume != Setting.gameSetting.soundLevel){//different level
			otherEnt.SetEntAudioVolume(volume);
		}
		if (Setting.gameSetting.enableSound && audioClip /*&& !sourceAudio.isPlaying*/) {
			//Debug.Log("play:" + audioClip.name);
			sourceAudio.clip = audioClip;
			sourceAudio.volume = volume;
			sourceAudio.Play();
		}
		
	}

    
    //support for the ball only
    IEnumerator playParticleEffectEvery(Material beginMat, Material endMat, ParticleSystem ps, float every, float total)
    {
        Renderer r = GetComponent<Renderer>();
        r.material = beginMat;
        float lerp = 0.0f;
        float start = 0.0f;
        while(start < total)
        {
            lerp = Mathf.Lerp(0, total, start);
            r.material.Lerp(beginMat, endMat, lerp);
            yield return new WaitForSeconds(every);
            ps.Play();
            PowerUp.ScaleDown(transform);
            start++;
        }
        ps.Stop();
        Destroy(ps.gameObject);
        r.material = endMat;
		isOnFire = false;
    }

	void Update(){
		if(isAtMaxScale && !PEGameObject){
			Debug.Log(name + ":do a max scale");
			PEGameObject = PlayPEAtPosition((GameObject)Resources.Load(MAX_SCALE_FX_PATH), transform.position, false, transform);
		}else{
			if(PEGameObject){
				Destroy(PEGameObject);
				PEGameObject = null;
			}
		}
	}


	ParticleSystem InstantiateFX(int index, Vector3 position){
		//Debug.Log("instantiate:" + InternalFX[index].name);
		ParticleSystem ps = GameObject.Instantiate(InternalFX[index], position, Quaternion.identity) as ParticleSystem;
		ps.name = InternalFX[index].name;
		return ps;
	}

	public void PlayInternalFX(int index){
		//new one: not init || diff from the one ordered
		if(!ps || ps && ps.name != InternalFX[index].name){
			ps = InstantiateFX(index, transform.position);
		}
		ps.Play();
	}

	public void StopInternalFX(){
		Debug.Log("stop");
		if(ps)
			Destroy(ps.gameObject);
	}

	public static void EnableMeshCutOut(Entity ent, Entity otherEnt){
		//Debug.Log("ent:" + ent.name);
		//Debug.Log("otherEnt:" + otherEnt.name);
		SetRenderQueue srq = otherEnt.GetComponentInChildren<SetRenderQueue> ();
		if (srq) {
			//Debug.Log("Start hiding");
			srq.startHiding = true;
			GameObject invisibleSphere = (GameObject)GameObject.Instantiate (Resources.Load (INVISIBLE_SPHERE_PATH));
			invisibleSphere.transform.position = new Vector3(ent.transform.position.x,otherEnt.transform.position.y, otherEnt.transform.position.z); // take the ball x
			invisibleSphere.GetComponent<ObstacleScript> ().objectSpeed = otherEnt.GetComponent<ObstacleScript>().objectSpeed;
		}
	}

	public static void DisableMeshCutOut(Entity ent){
		if(ent.GetComponent<SetRenderQueue>()){
			ent.GetComponent<SetRenderQueue> ().startHiding = false;
		}
	}

	//get schedule from PowerUp.ShieldDw
	public void ShieldDownWrapper(){
		//PowerUp.PowerUpShieldDown(this);
		Debug.Log("shield down wrapper:" + name);
		Entity.ShieldDown(this);
	}

	public void WaterDownWrapper(){
		//PowerUp.PowerUpWaterDown(this);
		Entity.WaterDown(this);
	}


	public GameObject PlayPEAtPosition(GameObject PEGameObject, Vector3 position, bool autoDestroy = true, Transform followTarget = null, float yOffset = 0.0f){ // one off
		if(PEGameObject){
			GameObject PEFX = (GameObject)Instantiate(PEGameObject) as GameObject;
			Debug.Log("FX:" + PEFX.name);
			PEFX.transform.position = position;
			if(followTarget){
				FollowTarget ft = PEFX.AddComponent<FollowTarget>();
				ft.offset = new Vector3(0.0f, yOffset, 0.0f);
				ft.target = followTarget;
			}
			ParticleSystem ps = PEFX.GetComponent<ParticleSystem>();
			ps.Play();
			if(autoDestroy){
				Destroy(PEFX, ps.main.duration);
			}
			return PEFX;
		}
		return null;
	}


	bool FlashAble(Entity otherEnt){
		//Debug.Log("max_scale:" + PowerUp.MAX_SCALE);
		//return otherEnt.entityType != ENTITY_TYPE.POWER_UP && transform.localScale.x < PowerUp.MAX_SCALE;
		return (otherEnt.entityType == ENTITY_TYPE.ENEMY && transform.localScale.x < PowerUp.MAX_SCALE || otherEnt.entityType == ENTITY_TYPE.OBSTACLE && transform.localScale.x < PowerUp.MAX_SCALE ) ;
	}

	//TO DO : Performance tuning
	public virtual void Invisiblify(bool hide){
		//Debug.Log(name + "/hide:" + hide);
		foreach(MeshRenderer mr in meshes){
				mr.enabled = !hide;
		}

		foreach(ParticleSystem ps in fxChildren){
			ps.gameObject.SetActive(!hide);
		}
	}

	bool PlayFireFX(Entity otherEnt){
		if(otherEnt.entityName == VOLCANO_NAME && !isOnFire)
		{
				Debug.Log ("FirePowerUp test");
				GameObject OilSplashHighRoot = (GameObject)Instantiate(Resources.Load(FIRE_PATH) as GameObject);
				OilSplashHighRoot.transform.position = transform.position;
				OilSplashHighRoot.transform.parent = transform;                    
				ParticleSystem OilSplashHighRootParticleSystem = OilSplashHighRoot.GetComponent<ParticleSystem>();
				Material lavaBallMat = Resources.Load(FIRE_MAT_PATH) as Material;
				Material curBallMat = GetComponent<Renderer>().material;
				isOnFireCoRoutine = playParticleEffectEvery(lavaBallMat, curBallMat, OilSplashHighRootParticleSystem, OilSplashHighRootParticleSystem.main.duration, 5.0f);
				StartCoroutine(isOnFireCoRoutine);
				isOnFire = true;
		}

		return isOnFire;
	}
	//player v.s other
	void OnTriggerEnter(Collider other){
		//Debug.Log(name + ":collided with - " + other.gameObject.name);
		//player-first
		if (entityType != ENTITY_TYPE.PLAYER) {
			//Debug.Log("entity_type:" + entityType);
			if(entityType == ENTITY_TYPE.ENEMY && other.name == WATER_FX_COLLIDER){
				Debug.Log("water destroyed:" + name);
				EventManager.Instance.spawnerDestroyedEvent.Invoke(this);
			}
			//set trigger
			//gameObject.GetComponent<Collider>().isTrigger = true;//go through and hit despawner later
			//Debug.Log("return????");
			return;
		}
		//Debug.Log (name + "collided with:" + other.gameObject.name);
		//send colliding event accordingly
		Entity otherEnt = other.gameObject.GetComponent<Entity>();
		if (otherEnt) {
			if (otherEnt.entityType == ENTITY_TYPE.PLAYER || otherEnt.entityType == entityType) { // what if we have to handle other player ? NOTE to make use of guid later on
				return;
			}

			playEntSoundOnCollided(this, otherEnt); // hit sound
			//shield
			// if(PowerUp.hasShield){
			// 	if(currentPowerupType >= FXRangeBegin && currentPowerupType <= FXRangeEnd){ //within range of down call
			// 		Debug.Log("invoking down call");
			// 		DownCalls[currentPowerupType].Invoke(this);
			// 	}else{
			// 		currentPowerupType = (int)otherEnt.GetComponent<PowerUp>().powerUptype;
			// 		Debug.Log("hello!!!");
			// 	}
			// 	return;
			// }
			

			if(!isOnFire && FlashAble(otherEnt)){//snow ball small
				Debug.Log("flashable");
				EventManager.Instance.FlashAndLoseLiveEvent.Invoke (this, otherEnt);
				return;
			}
			
			//play collided FX
			PlayPEAtPosition(otherEnt.onCollidedFX, transform.position);
			PlayFireFX(otherEnt);
			

           //player collided with powerup
			if (otherEnt.entityType == ENTITY_TYPE.POWER_UP /*&& !isOnFire*/) {
				Debug.Log("currentpowerupType:" + currentPowerupType);
				//down the current powerup
				if(currentPowerupType >= FXRangeBegin && currentPowerupType <= FXRangeEnd){ //within range of down call
					Debug.Log("invoking down call");
					DownCalls[currentPowerupType-1].Invoke(this);
				}else{
					currentPowerupType = (int)otherEnt.GetComponent<PowerUp>().powerUptype;
					Debug.Log("hello!!!");
				}
				//prevPowerupType = currentPowerupType;
				//currentPowerupType = otherEnt.GetComponent<PowerUp>().powerUptype;
				//Debug.Log("powerup???");

				/*if(isOnFire){
					 StopCoroutine(isOnFireCoRoutine);
					 GetComponent<Renderer>().material = originalMaterial;
					 isOnFire = false;
				}

				if(PlayFireFX(otherEnt)){
					return;
				}*/
				//up call
				PowerUp.PowerUpHandler (this, otherEnt); // let powerup fire event
			} else if (otherEnt.entityType == ENTITY_TYPE.ENEMY || otherEnt.entityType == ENTITY_TYPE.OBSTACLE) {
				
				
				//changing shader
				EnableMeshCutOut(this, otherEnt);
				//fire
				if(isOnFire){
					otherEnt.Invisiblify(true);
					return;
				}
				
				
				if (otherEnt.entityType == ENTITY_TYPE.ENEMY) {
					EventManager.Instance.entEnemyCollisionEvent.Invoke (this, otherEnt);
				} else if (otherEnt.entityType == ENTITY_TYPE.OBSTACLE) {
					EventManager.Instance.entObstacleCollisionEvent.Invoke (this, otherEnt);

					PowerUp.ScaleDown (this.transform);
					PowerUp.ScaleDown (this.transform);
					//Debug.Log("Scale down:" + name);
					if(otherEnt.entityName == BARRIER_NAME){//barrier flash no matter what
						EventManager.Instance.FlashAndLoseLiveEvent.Invoke (this, otherEnt);
					}
				}
			}
		}
	}
}
