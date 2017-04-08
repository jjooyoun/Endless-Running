using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {
	public Transform parent;
	public float speed = -0.5f;
	public Vector3 target; //to reset
	
	public bool rest = true;

	void Update () {
		if(rest)
			return;
		float distY = transform.position.y - target.y;
		if(distY <= -0.05f){
			Debug.Log("turn on rest!!");
			rest = true;
			StartCoroutine(Reset(parent.position));
		}
		transform.Translate(new Vector3(0.0f, speed, 0.0f));
	}

	public void ResetLaser(){
		StartCoroutine(Reset(parent.position));
	}
	public IEnumerator Reset(Vector3 pos, float sec = 1.0f){
		yield return new WaitForSeconds(sec);
		//Debug.Log("done reset");
		transform.position = pos;
		rest = false;
	}
}
