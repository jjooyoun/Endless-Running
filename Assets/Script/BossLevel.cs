using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel : MonoBehaviour {

	public GameObject trump;

	public Transform startMarker;
	public Transform endMarker;
	public float speed = 1.0F;
	public int BossLifeNum = 10;
	public bool startMoving = true;
	private float startTime;
	private float journeyLength;

	public AudioClip TrumpTakeDamageSound;
	public AudioClip startTrumpSound;

	private AudioSource source;

	public static readonly string TRUMP_HIT_FX_PATH = "Prefabs/TrumpHitFX";

	void Awake() {
		source = GetComponent<AudioSource>();
	}


	// Use this for initialization
	void Start () {
		startMarker = transform;
		endMarker = GameObject.Find ("ball").transform;
		startTime = Time.time;
		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
		//source = GetComponent<AudioSource>();
		source.PlayOneShot(startTrumpSound, 1);
	}

	// Update is called once per frame
	void Update () {
		if (!startMoving)
			return;
		int randIndex = Random.Range (0, 2);
		if (randIndex == 0) {
			//trump.transform.Translate (0.005f, 0, 0);
		}
		else if (randIndex == 1) {
			//trump.transform.Translate (0.005f, 0, 0.005f);
		}

		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);

	}

	public static void BossOnHit(Entity bossEnt){
		BossLevel boss = bossEnt.GetComponent<BossLevel> ();
		if (boss.BossLifeNum - 1 == 0) {
			//play explosion animation
			//call finish level
			return;
		}
		boss.source.PlayOneShot(boss.TrumpTakeDamageSound, 1);
		//if(boosEnt.entityName == bossEtBARRIER_NAME) {
		bossEnt.PlayPEAtPosition( Resources.Load(TRUMP_HIT_FX_PATH) as GameObject,bossEnt.transform.position, true, bossEnt.transform);
		//}
		boss.BossLifeNum -= 1;
		ScoreSystem ss = GameObject.FindObjectOfType<ScoreSystem> ();
		ss.StartFlashWrapper (bossEnt);
		Debug.Log ("boss life after:" + boss.BossLifeNum);
		boss.startMoving = false;
	}

//	private void OnTriggerEnter(Collider other){
//		Entity otherEnt = other.GetComponent<Entity>();
//		if(otherEnt){
//			Debug.Log ("trump collided with:" + otherEnt.entityName);
//			if(otherEnt.entityType == Entity.ENTITY_TYPE.PLAYER){
//				GetComponent<Rigidbody>().AddForce(0.0f, 0.0f, 10.0f, ForceMode.Impulse);
//			}
//		}
//	}
}
