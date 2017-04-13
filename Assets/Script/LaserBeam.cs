using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {
	/// <summary>
	/// OnParticleCollision is called when a particle hits a collider.
	/// </summary>
	/// <param name="other">The GameObject hit by the particle.</param>
	void OnParticleCollision(GameObject other)
	{
		//Debug.Log("ghello:"+other.name);
		
		Entity ent = GetComponentInParent<Entity>();
		Entity otherEnt = other.GetComponent<Entity>();
		if(otherEnt.entityType == Entity.ENTITY_TYPE.PLAYER){
			if(ent.onCollidedFX){
				GameObject collidedFX = (GameObject)Instantiate(ent.onCollidedFX) as GameObject;
				collidedFX.transform.position = otherEnt.transform.position;
				collidedFX.GetComponent<ParticleSystem>().Play();
			}
		}
	}
}
