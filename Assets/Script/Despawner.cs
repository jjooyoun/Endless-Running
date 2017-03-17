using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		Debug.Log ("hello:" + other.name);
		//temporary . hopefully will move it later
		Entity ent = other.GetComponent<Entity>();
		if (ent && ent.deform) {
			//Destroy (ent.deform.gameObject);
			SetRenderQueue srq =  ent.GetComponentInChildren<SetRenderQueue> ();
			if (srq) {
				srq.startHiding = false; // apparently no need to set it once the obscurer is destroyed
			}
			Destroy (ent.deform.gameObject);
		}
        GameObjectUtil.Destroy(other.gameObject);
	}

}
