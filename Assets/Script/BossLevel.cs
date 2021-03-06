﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel : MonoBehaviour {

	public GameObject trump;

	public Transform startMarker;
	public Transform endMarker;

	public AudioClip[] clips;
	private static readonly float DISTANCE_CONSTRAINT = 0.5f;
	public float forceStrength = 5.0f;
	public float DelayBeforeHuntingSec = 2.0f;
	private Rigidbody rigidbody;
	private Entity thisEnt;
	private Entity playerEnt;
	public float speed = 1.0F;
	public bool isHit = false;
	public int BossLifeNum = 10;
	public bool startMoving = true;
	private bool endLevel = false;
	private bool playStartSound = false;

	private int counter = 0;
	private float startTime;
	private float journeyLength;

	public AudioClip TrumpTakeDamageSound;
	public AudioClip TrumpTakeDamageHit;
	public AudioClip startTrumpSound;

	private AudioSource source;

	public static readonly string BALL_NAME = "ball";
	public static readonly string TRUMP_HIT_FX_PATH = "Prefabs/TrumpHitFX";
	public static readonly string TRUMP_EXPLODE_FX_PATH = "Prefabs/TrumpExplodeFX";
	public static readonly string IMPACT_FX_PATH = "Prefabs/ImpactFX";
	public static readonly string STAR_FX_PATH = "Prefabs/StarFX";
	public static readonly string GOP_FX_PATH = "Prefabs/GOPFX";

	void Awake() {
		source = GetComponent<AudioSource>();
	}


	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		startMarker = transform;
		GameObject playerGO = GameObject.Find (BALL_NAME);
		endMarker = playerGO.transform;
		playerEnt = playerGO.GetComponent<Entity>();
		thisEnt = GetComponent<Entity>();
		startTime = Time.time;
		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
		//source = GetComponent<AudioSource>();
		WaitForSec(2.5f);
		//source.PlayOneShot(startTrumpSound, 1);
	}

	// Update is called once per frame
	void Update () {
		if(endLevel == true) {
			//Setting.StaticQuitGame();
			//Setting.LoadLevelCompletescene();
		}

		if(playStartSound == true) {
			playStartSound = false;
			source.PlayOneShot(startTrumpSound, 1);

		}
		else if (!startMoving)
			return;
		// int randIndex = Random.Range (0, 2);
		// if (randIndex == 0) {
		// 	//trump.transform.Translate (0.005f, 0, 0);
		// }
		// else if (randIndex == 1) {
		// 	//trump.transform.Translate (0.005f, 0, 0.005f);
		// }

		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
		//check distance, hit
		if(Vector3.Distance(transform.position, endMarker.position) <= DISTANCE_CONSTRAINT){
			rigidbody.isKinematic = false;
			rigidbody.AddForce(0.0f, 0.0f, forceStrength, ForceMode.Impulse);
			//ent no equipped wweapon
			if(!PowerUp.hasWater && !PowerUp.hasFire && !PowerUp.hasShield && !playerEnt.IsAtMaxScale){
				//flash player...maybe already did
				//play sound accordingly
				PlayTrumpSoundAtIndex(counter);
				//thisEnt.PlayPEAtPosition( Resources.Load(STAR_FX_PATH) as GameObject,thisEnt.transform.position, true, thisEnt.transform);
				//thisEnt.PlayPEAtPosition( Resources.Load(STAR_FX_PATH) as GameObject,transform.position);
				counter++;

				// reset the counter
				if(counter == clips.Length) {
					counter = 0;
				}
				WaitForSecBeforeStartHunting(DelayBeforeHuntingSec);
			}else{//got weapon or max scale
				//BossLevel.BossOnHit(playerEnt);
				OnHit();
			}
		}
	}

	void OnHit(){

		if (BossLifeNum - 1 == 0) {
			//play explosion animation
			//call finish level
			//Setting.StaticQuitGame();
			//Setting.LoadLevelCompletescene();
			thisEnt.PlayPEAtPosition( Resources.Load(TRUMP_EXPLODE_FX_PATH) as GameObject,transform.position);
			//thisEnt.PlayPEAtPosition( Resources.Load(IMPACT_FX_PATH) as GameObject,transform.position);
			thisEnt.PlayPEAtPosition( Resources.Load(GOP_FX_PATH) as GameObject,transform.position);
			trump.SetActive(false);
			//StartWaitForSec(5.0f);
			endLevel = true;


			//Setting.StaticQuitGame();
			//Setting.LoadLevelCompletescene();
			return;
		}
		
		BossLifeNum -= 1;
		source.PlayOneShot(TrumpTakeDamageSound, 1);
		source.PlayOneShot(TrumpTakeDamageHit, 1);
		thisEnt.PlayPEAtPosition( Resources.Load(TRUMP_HIT_FX_PATH) as GameObject,thisEnt.transform.position, true, thisEnt.transform);
		thisEnt.PlayPEAtPosition( Resources.Load(STAR_FX_PATH) as GameObject,thisEnt.transform.position, true, thisEnt.transform);

		ScoreSystem ss = GameObject.FindObjectOfType<ScoreSystem> ();
		ss.StartFlashWrapper (thisEnt);
		Debug.Log ("boss life after:" + BossLifeNum);
		startMoving = false;



	}

	public void WaitForSecBeforeStartHunting(float sec){
		startMoving = false;
		StartCoroutine(StartHuntingInSec(sec));
	}

	IEnumerator StartHuntingInSec(float sec){
		yield return new WaitForSeconds(sec);
		startMoving = true;
		GetComponent<Rigidbody>().isKinematic = true;
	}

	void PlayTrumpSoundAtIndex(int index){
		if(clips.Length > 0  && index < clips.Length){
			source.PlayOneShot(clips[index]);
		}
	}

	void WaitForSec(float sec)
	{
		StartCoroutine(StartWaitForSec(sec));
	}

	IEnumerator StartWaitForSec(float sec)
	{
		print(Time.time);
		yield return new WaitForSeconds(sec);
		print(Time.time);
		playStartSound = true;
	}
}
