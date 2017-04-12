using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//nothing
[RequireComponent (typeof (BoxCollider))]
[RequireComponent (typeof (AudioSource))]
[RequireComponent (typeof (Rigidbody))]
public class Entity : MonoBehaviour {
    public GameObject child;
	//public GameObject HitFX;
	public GameObject onCollidedFX;
	private ParticleSystem ps;
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

	public bool isOnFire = false;

	public static int MaxScaleFXIndex = 0;

	public ParticleSystem[] InternalFX;
	private static readonly string LASER_BEAM_PATH = "Prefabs/LaserBeam";
	private static readonly string INVISIBLE_SPHERE_PATH = "Prefabs/InvisibleSphere";
	private static readonly string FIRE_PATH = "Prefabs/OilSpashHighRoot";
	private static readonly string FIRE_MAT_PATH = "Materials/LavaBall";
	private static readonly string WALKER_NAME = "Walker";
	private static readonly string GATE_NAME = "Gate";
	private static readonly string BARRIER_NAME = "Barrier";
	private static readonly string VOLCANO_NAME = "Volcano";
	private static readonly string TIE_FIGHTER_NAME = "TIE_Fighter";

	

	public void Init(){
		//Debug.Log (entityName + ":init");
		//audioSource = GetComponent<AudioSource> ();
		//ps = GetComponentInChildren<ParticleSystem>();
		//Debug.Log("entityName:" + entityName);
		if(entityName == TIE_FIGHTER_NAME){ //quick and dirty solution
			//ebug.Log("spawn laser beam!!!");
			GameObject laserBeamGo = GameObject.Instantiate(Resources.Load(LASER_BEAM_PATH) as GameObject);
			LaserBeam lb = laserBeamGo.GetComponent<LaserBeam>();
			lb.Init(transform);
		}
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
		if(Input.GetKeyDown(KeyCode.P) && entityType == ENTITY_TYPE.PLAYER){
			Debug.Log("press P!!!");
			PlayInternalFX(MaxScaleFXIndex);
		}
		if(Input.GetKeyDown(KeyCode.I) && entityType == ENTITY_TYPE.PLAYER){
			Debug.Log("press I!!!");
			//PlayInternalFX(MaxScaleFXIndex);
			StopInternalFX();
		}
	}


	ParticleSystem InstantiateFX(int index){
		return GameObject.Instantiate(InternalFX[index], transform.position, Quaternion.identity) as ParticleSystem;
	}

	public void PlayInternalFX(int index){
		ps = InstantiateFX(index);
		ps.transform.parent = transform;
		ps.Play();
	}

	public void StopInternalFX(){
		//if(ps.isPlaying){
			//Debug.Log("Stopping!");
			ps.Stop();
		//}
	}
	public static void EnableMeshCutOut(Transform transform, Entity ent){
		SetRenderQueue srq = ent.GetComponentInChildren<SetRenderQueue> ();
		if (srq) {
			srq.startHiding = true;
			GameObject invisibleSphere = (GameObject)GameObject.Instantiate (Resources.Load (INVISIBLE_SPHERE_PATH));
			invisibleSphere.transform.position = new Vector3(transform.position.x,ent.transform.position.y, ent.transform.position.z); // take the ball x
			invisibleSphere.GetComponent<ObstacleScript> ().objectSpeed = ent.GetComponent<ObstacleScript>().objectSpeed;
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

	//player v.s other
	private void OnTriggerEnter(Collider other){
		//Debug.Log (name + "collided with:" + other.name);
		//player-first
		if (entityType != ENTITY_TYPE.PLAYER) {
			return;
		}
		//send colliding event accordingly
		Entity otherEnt = other.gameObject.GetComponent<Entity>();
		if (otherEnt) {
			if (otherEnt.entityType == ENTITY_TYPE.PLAYER || otherEnt.entityType == entityType) { // what if we have to handle other player ? NOTE to make use of guid later on
				return;
			}

			playEntSoundOnCollided(this, otherEnt); // hit sound

            //player collided with powerup
			if (otherEnt.entityType == ENTITY_TYPE.POWER_UP && !isOnFire) {
				//Debug.Log ("powerup!!!");
				PowerUp.PowerUpHandler (this, otherEnt); // let powerup fire event
			} else if (otherEnt.entityType == ENTITY_TYPE.ENEMY || otherEnt.entityType == ENTITY_TYPE.OBSTACLE) {
				//has shield just destroying shield without broadcast event
				// if (PowerUp.hasShield) {
				// 	Debug.Log ("has Shield");
				// 	PowerUp.PowerUpShieldDown (this);
				// 	//OnShieldDown();
				// 	return;
				// }

				if(otherEnt.GetComponent<LaserBeam>()){
					//Debug.Log("hello");
					//StartCoroutine(otherEnt.GetComponent<LaserBeam>().Reset(otherEnt.transform.position));
					otherEnt.GetComponent<LaserBeam>().ResetLaser();
				}
				// if(ps){
				// 	ps.Play();
				// }
				//Debug.Log("particle:" + otherEnt.onCollidedFX);
				
				//changing shader
				EnableMeshCutOut(transform, otherEnt);
				
				if (otherEnt.entityType == ENTITY_TYPE.ENEMY && this.gameObject.transform.localScale.x > otherEnt.gameObject.transform.localScale.x) {
					if(otherEnt.onCollidedFX){
						GameObject collidedFX = (GameObject)Instantiate(otherEnt.onCollidedFX) as GameObject;
						collidedFX.transform.position = transform.position;
						collidedFX.GetComponent<ParticleSystem>().Play();
					}
					EventManager.Instance.entEnemyCollisionEvent.Invoke (this, otherEnt);
					if (!isOnFire && otherEnt.entityName == WALKER_NAME && this.gameObject.transform.localScale.x > otherEnt.gameObject.transform.localScale.x) {
						return;
					}


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
				} else if (otherEnt.entityType == ENTITY_TYPE.OBSTACLE /*&& this.gameObject.transform.localScale.x < otherEnt.gameObject.transform.localScale.x*/) {
					EventManager.Instance.entObstacleCollisionEvent.Invoke (this, otherEnt);
					//transition to a different scene
					if(isOnFire){ //CRUSH EVERYTHING, Not even GATE
						return;
					}

					PowerUp.ScaleDown (this.transform);
					PowerUp.ScaleDown (this.transform);
					if(otherEnt.entityName == BARRIER_NAME){
						EventManager.Instance.FlashAndLoseLiveEvent.Invoke (this, otherEnt);
					}
				} else { //smaller flash
					//flash lose live
					EventManager.Instance.FlashAndLoseLiveEvent.Invoke (this, otherEnt);
				}
			}
		}
	}

}
