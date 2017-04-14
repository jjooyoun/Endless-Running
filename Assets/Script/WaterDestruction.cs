using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//follow player
public class WaterDestruction : MonoBehaviour {
	private Transform parentTransform;
	private ParticleSystem ps;
	// public bool start = false;

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	// void OnEnable()
	// {
	// 	//Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"));
	// }
	// void Start(){
	// 	ps = GetComponent<ParticleSystem>();
	// }

	// public void SetParentTransform(Transform parent){
	// 	parentTransform = parent;
	// }

	// void Update(){
	// 	if(start && parentTransform){
	// 		transform.position = parentTransform.position;
	// 	}

	// }
	void OnParticleCollision(GameObject other) {
		//Debug.Log ("water collided w/ :" + other.name);
		Entity otherEnt = other.GetComponent<Entity> ();
		if (otherEnt && otherEnt.entityType == Entity.ENTITY_TYPE.ENEMY) {
			Debug.Log ("water collided w/ :" + otherEnt.name);
			EventManager.Instance.spawnerDestroyedEvent.Invoke (otherEnt);
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
