using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {

	public AudioClip shootSound;
	private AudioSource source;
	bool isFired = false;
	/// <summary>
	/// OnParticleCollision is called when a particle hits a collider.
	/// </summary>
	/// <param name="other">The GameObject hit by the particle.</param>
	void Awake() {
		source = GetComponent<AudioSource>();
	}

	void Start() {
		StartCoroutine(PlaySoundLaser());
	}

	void OnParticleCollision(GameObject other)
	{
		//Debug.Log("ghello:"+other.name);
		
		Entity ent = GetComponentInParent<Entity>();
		Entity otherEnt = other.GetComponent<Entity>();
		if(otherEnt && otherEnt.entityType == Entity.ENTITY_TYPE.PLAYER){
			if(ent.onCollidedFX){
				EventManager.Instance.FlashAndLoseLiveEvent.Invoke (otherEnt, ent);
//				GameObject collidedFX = (GameObject)Instantiate(ent.onCollidedFX) as GameObject;
//				collidedFX.transform.position = otherEnt.transform.position;
//				collidedFX.GetComponent<ParticleSystem>().Play();

			}
		}
	}

	IEnumerator PlaySoundLaser()
	{
		yield return new WaitForSeconds(2);
		Debug.Log ("TIE Fire sound");
		//source.clip = shootSound;
		source.PlayOneShot(shootSound,1);
	}

	void Update() {
		//Debug.Log ("isFire:" + isFired);
		//Debug.Log ("Time.time:" + Time.time);
		if (isFired == false) {
			//PlaySoundLaser ();
			isFired = true;
		}

	}
}
