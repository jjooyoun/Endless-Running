using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		//Debug.Log ("hello:" + other.name);
		//temporary . hopefully will move it later
		Entity ent = other.GetComponent<Entity>();
		SetRenderQueue srq =  ent.GetComponentInChildren<SetRenderQueue> ();
		if (srq) {
			srq.startHiding = false; // apparently no need to set it once the obscurer is destroyed
		}

        GameObjectUtil.Destroy(other.gameObject);
	}

}
