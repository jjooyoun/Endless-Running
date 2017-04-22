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

	public AudioClip flashSound;

	private AudioSource source;

    System.Guid id = System.Guid.NewGuid();

	public enum ENTITY_TYPE{
		PLAYER,
		ENEMY,		//big to attack
		OBSTACLE, //small to attack
		POWER_UP
	};

	public ENTITY_TYPE entityType =  ENTITY_TYPE.PLAYER;
	public string entityName = "Entity";

	private bool isAtMaxScale = false;
	public bool IsAtMaxScale{
		get{return isAtMaxScale;}
		set{isAtMaxScale = value;}
	}

	private GameObject PEGameObject = null;

	public static readonly string BALL_MAT_PATH = "BALL_MAT_PATH";
	public static readonly string LASER_BEAM_PATH = "Prefabs/LaserBeam";
	public static readonly string INVISIBLE_SPHERE_PATH = "Prefabs/InvisibleSphere";
	public static readonly string FIRE_PATH = "Prefabs/OilSpashHighRoot";
	public static readonly string FIRE_MAT_PATH = "Materials/LavaBall";
	public static readonly string WALKER_NAME = "Walker";
	public static readonly string GATE_NAME = "Gate";
	public static readonly string BARRIER_NAME = "Barrier";
	public static readonly string VOLCANO_NAME = "FirePowerUp";
	public static readonly string TIE_FIGHTER_NAME = "TIE_Fighter";

	public static readonly string MAX_SCALE_FX_PATH = "Prefabs/MaxScaleFX";
	public static readonly string WATER_FX_PATH = "Prefabs/WaterFX";

	public static readonly string IMPACT_FX_PATH = "Prefabs/ImpactFX";

	public static readonly string WATER_FX_COLLIDER = "Water_AOE";

	public static readonly string BRICK_DESTROY_FX_PATH = "Prefabs/DestroyBrick";

	public static readonly string WATER_DESTORY_FX_PATH = "Prefabs/WaterDestroyFX";

	public static readonly string SMOKE_FX_PATH = "Prefabs/SmokeFX";


	public static readonly string FIRE_DOWN_WRAPPER = "FireDownWrapper";
	public static readonly string WATER_DOWN_WRAPPER = "WaterDownWrapper";
	public static readonly string SHIELD_DOWN_WRAPPER = "ShieldDownWrapper";


	public static readonly string BOSS_NAME = "Trump";

	

	//HIDE
	private MeshRenderer[] meshes;
	private ParticleSystem[] fxChildren;
	//public bool Invisible = false;
	//MATERIAL
	private Material originalMaterial;
	public Material OriginalMat{
		get{return originalMaterial;}
		set{originalMaterial = value;}
	}
	private Renderer rend;

	private IEnumerator isOnFireCoRoutine;

	public IEnumerator FireCoRoutine{
		get{return isOnFireCoRoutine;}
		set{isOnFireCoRoutine = value;}
	}

	public static readonly int FXRangeBegin = 2;
	public static readonly int FXRangeEnd = 4;

	private int currentFXPowerupType = -1;
	public int CurFXType{
		get{return currentFXPowerupType;}
		set{currentFXPowerupType = value;}
	}

	private int prevFXPowerUpType = -1;

	//the FX that is spawned
	protected GameObject currentSpawnFX;
	public GameObject SpawnFX{
		get{return currentSpawnFX;}
		set{currentSpawnFX = value;}
	}


	public static bool FloatDiff(float a, float b, float Epsilon = 0.00001f){
		return Mathf.Abs( a - b ) < Epsilon;
	}

	static void ShieldDown(Entity ent){
		//Debug.Log("Down-call::ShieldDown!!!" + ent.name);
		ent.CurFXType = -1;
		PowerUp.PowerUpShieldDown(ent);
		//Destroy(ent.SpawnFX.gameObject);
	}

	static void FireDown(Entity ent){
		//Debug.Log("Down-call::FireDown!!!");
		ent.CurFXType = -1;
		PowerUp.PowerUpFireDown(ent);
	}

	static void WaterDown(Entity ent){
		//Debug.Log("Down-call::WaterDown!!!");
		ent.CurFXType = -1;
		PowerUp.PowerUpWaterDown(ent);
	}

	//get schedule from PowerUp.ShieldDw
	public void ShieldDownWrapper(){
		Entity.ShieldDown(this);
	}

	public void WaterDownWrapper(){
		Entity.WaterDown(this);
	}

	public void FireDownWrapper(){
		Entity.FireDown(this);
	}

	//
	private static Action<Entity>[] DownCalls = {
		FireDown,
        ShieldDown,
		WaterDown
    };

	public void Init(){
		source = GetComponent<AudioSource>();

		meshes = GetComponents<MeshRenderer>();
		if(meshes.Length == 0){
			meshes = GetComponentsInChildren<MeshRenderer>();
		}
		fxChildren = GetComponentsInChildren<ParticleSystem>();
		//Debug.Log(name + " meshes:" + meshes.Length);
		if(entityType == ENTITY_TYPE.PLAYER){
			rend = GetComponent<Renderer>();
			originalMaterial = rend.material; //for player
			string ball_mat = PlayerPrefs.GetString("BALL_MAT_PATH");
			Debug.Log("BALL_MAT_PATH:" + ball_mat);
			if(ball_mat != ""){
				Debug.Log("loading material!!! from:" + ball_mat);
				//rend.material = Resources.Load(ball_mat, typeof(Material)) as Material;
				UnityEngine.Object ball = Resources.Load(ball_mat);
				if(ball){//sanity check
					GameObject ballGO = GameObject.Instantiate((GameObject)ball);
					rend.material = ballGO.GetComponent<Renderer>().material;
					Destroy(ballGO);
					originalMaterial = 	rend.material;
				}
			}
		}
	}

	void Start(){
		Init ();
		//source = GetComponent<AudioSource>();
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
	//Not Pause hit sound YET.
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
	//FIRE
    public IEnumerator playParticleEffectEvery(Material beginMat, Material endMat, ParticleSystem ps, float every, float total)
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
    }

	void Update(){
		//Debug.Log("currentFX:" + CurFXType);
		if(IsAtMaxScale && !PEGameObject){
			//Debug.Log(name + ":do a max scale");
			PEGameObject = PlayPEAtPosition((GameObject)Resources.Load(MAX_SCALE_FX_PATH), transform.position, false, transform);
		}else{
			if(PEGameObject){
				Destroy(PEGameObject);
				PEGameObject = null;
			}
		}
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

	


	public GameObject PlayPEAtPosition(GameObject PEGameObject, Vector3 position, bool autoDestroy = true, Transform followTarget = null, float yOffset = 0.0f){ // one off
		if(PEGameObject){
			GameObject PEFX = (GameObject)Instantiate(PEGameObject) as GameObject;
			//Debug.Log("FX:" + PEFX.name);
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
		return (otherEnt.entityType == ENTITY_TYPE.ENEMY && !IsAtMaxScale || otherEnt.entityType == ENTITY_TYPE.OBSTACLE && !IsAtMaxScale ) ;
	}

	bool isFXPowerUpType(int powerUpType){
		return powerUpType >= (int)PowerUp.PowerUpType.FIRE && powerUpType <= (int)PowerUp.PowerUpType.WATER;
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
	//player v.s other
	void OnTriggerEnter(Collider other){
		if(Setting.gameSetting.isPaused)
			return;
		//Debug.Log(name + ":collided with - " + other.gameObject.name);
		//player-first
		if (entityType != ENTITY_TYPE.PLAYER) {
			//Debug.Log("entity_type:" + entityType);
			if(entityType == ENTITY_TYPE.ENEMY && other.name == WATER_FX_COLLIDER){
				//exclude trump
				if(entityName == BOSS_NAME){
					return;
				}
				Debug.Log("water destroyed:" + name);
				PlayPEAtPosition (Resources.Load (WATER_DESTORY_FX_PATH) as GameObject, transform.position);
				EventManager.Instance.spawnerDestroyedEvent.Invoke(this);
			}
			
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
			

			if(!PowerUp.hasWater && !PowerUp.hasFire && !PowerUp.hasShield && FlashAble(otherEnt)){//snow ball small
				Debug.Log("flashable");
				source.PlayOneShot(flashSound,1);
				EventManager.Instance.FlashAndLoseLiveEvent.Invoke (this, otherEnt);
				return;
			}
			//NOT FLASHABLE
			//play collided FX
			if (PowerUp.hasShield && IsAtMaxScale == false) {
				PlayPEAtPosition (Resources.Load (SMOKE_FX_PATH) as GameObject, transform.position);
			} else {
				PlayPEAtPosition (otherEnt.onCollidedFX, transform.position);
			}
			

           //player collided with powerup
			if (otherEnt.entityType == ENTITY_TYPE.POWER_UP) {
				//prevPowerUpType = currentPowerupType;
				int powerupType = (int)otherEnt.GetComponent<PowerUp>().powerUptype;
				
				//Debug.Log("currentpowerupType:" + powerupType);
				//down the current powerup
				if(isFXPowerUpType(powerupType) && isFXPowerUpType(CurFXType)){ //within range of down call
					//Debug.Log("currentpoweruptype:" + powerupType);
					//Debug.Log("prev FX:" + CurFXType);
					//Debug.Log("-" + (CurFXType - FXRangeBegin));
					// if(CurFXType == powerupType)
					// 	DownCalls[CurFXType].Invoke(this);
					DownCalls[CurFXType-FXRangeBegin].Invoke(this);//clamping to the down-calls array
					//CurFXType = powerupType;
				}
				//up call
				PowerUp.PowerUpHandler (this, otherEnt); // let powerup fire event
			} else if (otherEnt.entityType == ENTITY_TYPE.ENEMY || otherEnt.entityType == ENTITY_TYPE.OBSTACLE) {
				//Debug.Log (otherEnt.entityName + ": in here");
				if(PowerUp.hasShield && IsAtMaxScale){
					otherEnt.Invisiblify(true);
					//Debug.Log("here2323");
					return;
				}

				//EITHER HAS SHIELD 
				if(PowerUp.hasShield){
					return;
				}
				//OR ATMAXSCALE : check down there
				
				//NEITHER
				//changing shader
				EnableMeshCutOut(this, otherEnt);
				//fire
				if(PowerUp.hasFire || PowerUp.hasWater){
					otherEnt.Invisiblify(true);
					if(otherEnt.entityName == BARRIER_NAME) {
						PlayPEAtPosition( Resources.Load(BRICK_DESTROY_FX_PATH) as GameObject,transform.position);
					}
					else if (PowerUp.hasWater) {
						Debug.Log ("Dretoy Water FX");
					}
					return;
				}
				
				//ATMAXSCALE HERE??
				if (otherEnt.entityType == ENTITY_TYPE.ENEMY) {
					otherEnt.Invisiblify(true);
					EventManager.Instance.entEnemyCollisionEvent.Invoke (this, otherEnt);
				} else if (otherEnt.entityType == ENTITY_TYPE.OBSTACLE) {
					otherEnt.Invisiblify(true);
					EventManager.Instance.entObstacleCollisionEvent.Invoke (this, otherEnt);
					PowerUp.ScaleDown (this.transform);
					PowerUp.ScaleDown (this.transform);
					IsAtMaxScale = false;
					if(otherEnt.entityName == BARRIER_NAME){//barrier flash no matter what
						EventManager.Instance.FlashAndLoseLiveEvent.Invoke (this, otherEnt);
					}
				}
			}
		}
	}
}
