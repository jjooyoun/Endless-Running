﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//nothing
[RequireComponent (typeof (Collider))]
public class Entity : MonoBehaviour {
    public GameObject child;
	public AudioSource audioSource;

    System.Guid id = System.Guid.NewGuid();

	public enum ENTITY_TYPE{
		PLAYER,
		ENEMY,		//big to attack
		OBSTACLE, //small to attack
		POWER_UP
	};

	public ENTITY_TYPE entityType =  ENTITY_TYPE.PLAYER;
	public string entityName = "Entity";

	public void Init(){
		//Debug.Log (entityName + ":init");
		audioSource = GetComponent<AudioSource> ();
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

	void playEntSound(Entity ent){
		AudioSource audio = ent.GetComponent<AudioSource>();//won't be null
		if (Setting.gameSetting.enableSound && !audio.isPlaying) {
			audio.Play();
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

			if (otherEnt.audioSource && otherEnt.audioSource.clip && !otherEnt.audioSource.isPlaying) {
				//Debug.Log ("Play clip:" + otherEnt.audioSource.clip.name);
				//playSoundAtPos(otherEnt.audioSource.clip, transform.position); // KNOCK IT OFF
				playEntSound(otherEnt);
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
					//OnShieldDown();
					return;
				}

				//changing shader
				SetRenderQueue srq = otherEnt.GetComponentInChildren<SetRenderQueue> ();
				if (srq) {
					srq.startHiding = true;
					GameObject invisibleSphere = (GameObject)GameObject.Instantiate (Resources.Load ("Prefabs/InvisibleSphere"));
					invisibleSphere.transform.position = new Vector3(transform.position.x,other.transform.position.y, other.transform.position.z); // take the ball x
					invisibleSphere.GetComponent<ObstacleScript> ().objectSpeed = otherEnt.GetComponent<ObstacleScript>().objectSpeed;
				}

				if (otherEnt.entityType == ENTITY_TYPE.ENEMY /*&& this.gameObject.transform.localScale.x > otherEnt.gameObject.transform.localScale.x*/) {
					EventManager.Instance.entEnemyCollisionEvent.Invoke (this, otherEnt);
					if (otherEnt.entityName == "Walker" && this.gameObject.transform.localScale.x > otherEnt.gameObject.transform.localScale.x) {
						return;
					}


                    //fire
                    if(otherEnt.entityName == "Volcano")
                    {
                        GameObject OilSplashHighRoot = (GameObject)Instantiate(Resources.Load("Prefabs/OilSpashHighRoot") as GameObject);
                        OilSplashHighRoot.transform.position = transform.position;
                        OilSplashHighRoot.transform.parent = transform;                    
                        ParticleSystem OilSplashHighRootParticleSystem = OilSplashHighRoot.GetComponent<ParticleSystem>();
                        Material lavaBallMat = Resources.Load("Materials/LavaBall") as Material;
                        Material curBallMat = GetComponent<Renderer>().material;
                        StartCoroutine(playParticleEffectEvery(lavaBallMat, curBallMat, OilSplashHighRootParticleSystem, OilSplashHighRootParticleSystem.duration, 5.0f));
                    }
				} else if (otherEnt.entityType == ENTITY_TYPE.OBSTACLE /*&& this.gameObject.transform.localScale.x < otherEnt.gameObject.transform.localScale.x*/) {
					EventManager.Instance.entObstacleCollisionEvent.Invoke (this, otherEnt);
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
