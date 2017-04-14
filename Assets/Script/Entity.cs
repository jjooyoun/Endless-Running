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
	public static readonly string VOLCANO_NAME = "Volcano";
	public static readonly string TIE_FIGHTER_NAME = "TIE_Fighter";

	public static readonly string MAX_SCALE_FX_PATH = "Prefabs/MaxScaleFX";
	public static readonly string WATER_FX_PATH = "Prefabs/WaterFX";


	public void Init(){
		//Debug.Log (entityName + ":init");
		//audioSource = GetComponent<AudioSource> ();
		//ps = GetComponentInChildren<ParticleSystem>();
		//Debug.Log("entityName:" + entityName);
		// if(entityName == TIE_FIGHTER_NAME){ //quick and dirty solution
		// 	//ebug.Log("spawn laser beam!!!");
		// 	GameObject laserBeamGo = GameObject.Instantiate(Resources.Load(LASER_BEAM_PATH) as GameObject);
		// 	LaserBeam lb = laserBeamGo.GetComponent<LaserBeam>();
		// 	lb.Init(transform);
		// }
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
		PowerUp.PowerUpShieldDown(this);
	}

	public void WaterDownWrapper(){
		PowerUp.PowerUpWaterDown(this);
	}

	// private void OnCollisionEnter(Collision other){
	// 	//Debug.Log (name + "collided with:" + other.gameObject.name);
	// 	//Debug.Log (name + "collided with:" + other.name);
	// 	//player-first
	// 	if (entityType != ENTITY_TYPE.PLAYER) {
	// 		return;
	// 	}
	// 	//send colliding event accordingly
	// 	Entity otherEnt = other.gameObject.GetComponent<Entity>();
	// 	if (otherEnt) {
	// 		if (otherEnt.entityType == ENTITY_TYPE.PLAYER || otherEnt.entityType == entityType) { // what if we have to handle other player ? NOTE to make use of guid later on
	// 			return;
	// 		}

	// 		playEntSoundOnCollided(this, otherEnt); // hit sound
	// 		if(otherEnt.onCollidedFX){
	// 			Debug.Log ("collidedFX:" + otherEnt.onCollidedFX.name);
	// 			GameObject collidedFX = (GameObject)Instantiate(otherEnt.onCollidedFX) as GameObject;
	// 			collidedFX.transform.position = transform.position;
	// 			collidedFX.GetComponent<ParticleSystem>().Play();
	// 		}
	// 		//player collided with powerup
	// 		if (otherEnt.entityType == ENTITY_TYPE.POWER_UP && !isOnFire) {
	// 			//Debug.Log ("powerup!!!");
	// 			PowerUp.PowerUpHandler (this, otherEnt); // let powerup fire event
	// 		} else if (otherEnt.entityType == ENTITY_TYPE.ENEMY || otherEnt.entityType == ENTITY_TYPE.OBSTACLE) {
	// 			//has shield just destroying shield without broadcast event
	// 			// if (PowerUp.hasShield) {
	// 			// 	Debug.Log ("has Shield");
	// 			// 	PowerUp.PowerUpShieldDown (this);
	// 			// 	//OnShieldDown();
	// 			// 	return;
	// 			// }

	// 			//v2
	// 			// if(otherEnt.GetComponent<LaserBeam>()){
	// 			// 	//Debug.Log("hello");
	// 			// 	//StartCoroutine(otherEnt.GetComponent<LaserBeam>().Reset(otherEnt.transform.position));
	// 			// 	otherEnt.GetComponent<LaserBeam>().ResetLaser();
	// 			// }


	// 			// if(ps){
	// 			// 	ps.Play();
	// 			// }
	// 			//Debug.Log("particle:" + otherEnt.onCollidedFX);

	// 			//changing shader
	// 			EnableMeshCutOut(this, otherEnt);
	// 			//fire
	// 			if(otherEnt.entityName == VOLCANO_NAME && !isOnFire)
	// 			{
	// 				//Debug.Log("Volcano!!!");
	// 				GameObject OilSplashHighRoot = (GameObject)Instantiate(Resources.Load(FIRE_PATH) as GameObject);
	// 				OilSplashHighRoot.transform.position = transform.position;
	// 				OilSplashHighRoot.transform.parent = transform;                    
	// 				ParticleSystem OilSplashHighRootParticleSystem = OilSplashHighRoot.GetComponent<ParticleSystem>();
	// 				Material lavaBallMat = Resources.Load(FIRE_MAT_PATH) as Material;
	// 				Material curBallMat = GetComponent<Renderer>().material;
	// 				StartCoroutine(playParticleEffectEvery(lavaBallMat, curBallMat, OilSplashHighRootParticleSystem, OilSplashHighRootParticleSystem.main.duration, 5.0f));
	// 				isOnFire = true;
	// 			}

	// 			if (otherEnt.entityType == ENTITY_TYPE.ENEMY && this.gameObject.transform.localScale.x >= PowerUp.MAX_SCALE/*otherEnt.gameObject.transform.localScale.x*/) {

	// 				EventManager.Instance.entEnemyCollisionEvent.Invoke (this, otherEnt);
	// 				if (!isOnFire && otherEnt.entityName == WALKER_NAME && this.gameObject.transform.localScale.x > otherEnt.gameObject.transform.localScale.x) {
	// 					return;
	// 				}



	// 			} else if (otherEnt.entityType == ENTITY_TYPE.OBSTACLE /*&& this.gameObject.transform.localScale.x < otherEnt.gameObject.transform.localScale.x*/) {
	// 				EventManager.Instance.entObstacleCollisionEvent.Invoke (this, otherEnt);
	// 				//transition to a different scene
	// 				if(isOnFire){ //CRUSH EVERYTHING, Not even GATE
	// 					return;
	// 				}

	// 				if(PowerUp.ScaleDown (this.transform)){
	// 					StopInternalFX();
	// 				}
	// 				PowerUp.ScaleDown (this.transform);
	// 				if(otherEnt.entityName == BARRIER_NAME){
	// 					EventManager.Instance.FlashAndLoseLiveEvent.Invoke (this, otherEnt);
	// 				}
	// 			} else { //smaller flash
	// 				//flash lose live
	// 				//Debug.Log("hello");
	// 				EventManager.Instance.FlashAndLoseLiveEvent.Invoke (this, otherEnt);
	// 			}
	// 		}
	// 	}

	// 	other.gameObject.GetComponent<Collider>().isTrigger = true;
	// }


	public GameObject PlayPEAtPosition(GameObject PEGameObject, Vector3 position, bool autoDestroy = true, Transform followTarget = null, float yOffset = 0.0f){ // one off
		if(PEGameObject){
			GameObject PEFX = (GameObject)Instantiate(PEGameObject) as GameObject;
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
		return (otherEnt.entityType == ENTITY_TYPE.ENEMY || otherEnt.entityType == ENTITY_TYPE.OBSTACLE) && transform.localScale.x < PowerUp.MAX_SCALE;
	}
	
	//player v.s other
	void OnCollisionEnter(Collision other){
		//player-first
		if (entityType != ENTITY_TYPE.PLAYER) {
			//set trigger
			gameObject.GetComponent<Collider>().isTrigger = true;//go through and hit despawner later
			//Debug.Log("return????");
			return;
		}
		Debug.Log (name + "collided with:" + other.gameObject.name);
		//send colliding event accordingly
		Entity otherEnt = other.gameObject.GetComponent<Entity>();
		if (otherEnt) {
			if (otherEnt.entityType == ENTITY_TYPE.PLAYER || otherEnt.entityType == entityType) { // what if we have to handle other player ? NOTE to make use of guid later on
				return;
			}

			playEntSoundOnCollided(this, otherEnt); // hit sound

			if(FlashAble(otherEnt)){//snow ball small
				EventManager.Instance.FlashAndLoseLiveEvent.Invoke (this, otherEnt);
				return;
			}
			

			PlayPEAtPosition(otherEnt.onCollidedFX, transform.position);
           //player collided with powerup
			if (otherEnt.entityType == ENTITY_TYPE.POWER_UP && !isOnFire) {
				//Debug.Log("powerup???");
				PowerUp.PowerUpHandler (this, otherEnt); // let powerup fire event
			} else if (otherEnt.entityType == ENTITY_TYPE.ENEMY || otherEnt.entityType == ENTITY_TYPE.OBSTACLE) {
				if(isOnFire)
					return;
				//changing shader
				EnableMeshCutOut(this, otherEnt);
				//fire
				if(otherEnt.entityName == VOLCANO_NAME && !isOnFire)
				{
					GameObject OilSplashHighRoot = (GameObject)Instantiate(Resources.Load(FIRE_PATH) as GameObject);
					OilSplashHighRoot.transform.position = transform.position;
					OilSplashHighRoot.transform.parent = transform;                    
					ParticleSystem OilSplashHighRootParticleSystem = OilSplashHighRoot.GetComponent<ParticleSystem>();
					Material lavaBallMat = Resources.Load(FIRE_MAT_PATH) as Material;
					Material curBallMat = GetComponent<Renderer>().material;
					StartCoroutine(playParticleEffectEvery(lavaBallMat, curBallMat, OilSplashHighRootParticleSystem, OilSplashHighRootParticleSystem.main.duration, 5.0f));
					isOnFire = true;
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
