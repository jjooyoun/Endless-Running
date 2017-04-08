using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//nothing
[RequireComponent (typeof (BoxCollider))]
[RequireComponent (typeof (AudioSource))]
[RequireComponent (typeof (Rigidbody))]
public class Entity : MonoBehaviour {
    public GameObject child;
	//public GameObject HitFX;

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

	private static readonly string LASER_BEAM_PATH = "Prefabs/LaserBeam";
	private static readonly string INVISIBLE_SPHERE_PATH = "Prefabs/InvisibleSphere";

	private static readonly string FIRE_PATH = "Prefabs/OilSpashHighRoot";

	private static readonly string FIRE_MAT_PATH = "Materials/LavaBall";



	public void Init(){
		//Debug.Log (entityName + ":init");
		//audioSource = GetComponent<AudioSource> ();
		ps = GetComponentInChildren<ParticleSystem>();
		//Debug.Log("entityName:" + entityName);
		if(entityName == "TIE_Fighter"){ //quick and dirty solution
			Debug.Log("spawn laser beam!!!");
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
			Debug.Log ("Scale up" + name);
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

	void playEntSound(AudioSource sourceAudio, AudioSource transferAudio){
		AudioClip audioClip = transferAudio.clip;
		float volume = transferAudio.volume;
		//AudioSource audio = ent.GetComponent<AudioSource>();//won't be null
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

			playEntSound(GetComponent<AudioSource>(), otherEnt.GetComponent<AudioSource>()); // hit sound

            //player collided with powerup
			if (otherEnt.entityType == ENTITY_TYPE.POWER_UP && !isOnFire) {
				//Debug.Log ("powerup!!!");
				PowerUp.PowerUpHandler (this, otherEnt); // let powerup fire event
			} else if (otherEnt.entityType == ENTITY_TYPE.ENEMY || otherEnt.entityType == ENTITY_TYPE.OBSTACLE) {
				//has shield just destroying shield without broadcast event
				if (PowerUp.hasShield) {
					Debug.Log ("has Shield");
					PowerUp.PowerUpShieldDown (this);
					//OnShieldDown();
					return;
				}

				if(otherEnt.GetComponent<LaserBeam>()){
					//Debug.Log("hello");
					//StartCoroutine(otherEnt.GetComponent<LaserBeam>().Reset(otherEnt.transform.position));
					otherEnt.GetComponent<LaserBeam>().ResetLaser();
				}
				if(ps){
					ps.Play();
				}
				//changing shader
				EnableMeshCutOut(transform, otherEnt);
				
				if (otherEnt.entityType == ENTITY_TYPE.ENEMY /*&& this.gameObject.transform.localScale.x > otherEnt.gameObject.transform.localScale.x*/) {
					EventManager.Instance.entEnemyCollisionEvent.Invoke (this, otherEnt);
					if (!isOnFire && otherEnt.entityName == "Walker" && this.gameObject.transform.localScale.x > otherEnt.gameObject.transform.localScale.x) {
						return;
					}


                    //fire
                    if(otherEnt.entityName == "Volcano" && !isOnFire)
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
					if(isOnFire){ //CRUSH EVERYTHING
						return;
					}
					PowerUp.ScaleDown (this.transform);
					PowerUp.ScaleDown (this.transform);
					if(otherEnt.entityName == "Barrier"){
						EventManager.Instance.FlashAndLoseLiveEvent.Invoke (this, otherEnt);
					}
				} else {
					//flash lose live
					EventManager.Instance.FlashAndLoseLiveEvent.Invoke (this, otherEnt);
				}
			}
		}
	}

}
