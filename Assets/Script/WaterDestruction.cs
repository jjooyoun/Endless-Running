using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDestruction : MonoBehaviour {
	void OnParticleCollision(GameObject other) {
		Debug.Log ("water collided w/ :" + other.name);
		Entity ent = other.GetComponent<Entity> ();
		if (ent && ent.entityType == Entity.ENTITY_TYPE.ENEMY) {
			Debug.Log ("water collided w/ :" + ent.name);
			EventManager.Instance.spawnerDestroyedEvent.Invoke (ent);
		}
	}

//	void OnTriggerEnter(Collider other){
//		Debug.Log ("hello:" + other.name);
//	}
//
//	void OnParticleTrigger()
//	{
//		Debug.Log ("hello2:");
//		ParticleSystem ps = GetComponent<ParticleSystem>();
//		ps.SetTriggerParticles(
//	}

}
