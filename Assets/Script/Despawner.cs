using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		//Debug.Log ("hello:" + other.name);
		//temporary . hopefully will move it later
		Entity ent = (other.gameObject.GetComponent<Entity>() == null) ? other.gameObject.GetComponentInParent<Entity>() : null;
		if (ent && ent.GetComponent<SetRenderQueue>()) {
			Debug.Log ("restore!");
			ent.GetComponent<SetRenderQueue> ().startHiding = false;
		}
//		if (ent && ent.deform) {
//			Debug.Log ("hello ent:" + ent.entityName);
//			SetRenderQueue srq = ent.GetComponent<SetRenderQueue> ();
//			if (srq) {
//				Debug.Log ("disable hiding");
//				srq.startHiding = false; // apparently no need to set it once the obscurer is destroyed
//			}
//			Destroy (ent.deform.gameObject);
//		}

        GameObjectUtil.Destroy(other.gameObject);
	}

}
