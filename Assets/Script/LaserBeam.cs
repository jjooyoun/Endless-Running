using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {
	private Transform parent;
	public float speed = -0.5f;
	public float dropSec = 1.5f;
	public Vector3 target = Vector3.zero; //to reset
	
	public bool rest = true;
	private Vector3 offSetCenter = new Vector3(0.3f, 1.5f, 0.0f);
	
	private float timeElapsed = 0.0f;

	public void Init(Transform parent){
		this.parent = parent;
		transform.position = parent.position + offSetCenter;
		//this.target = new Vector3(this.parent.transform.position.x, this.parent.transform.position.y - 1.0f, this.parent.transform.position.z);
		Debug.Log("target:" + target);
		rest = false;
	}

	void Update () {
		//Debug.Log("parent:" + parent);
		if(rest){
			Debug.Log("no!!");
			return;
		}
		if(timeElapsed > dropSec){
			ResetLaser();
		}
		transform.Translate(new Vector3(0.0f, speed, 0.0f));
		timeElapsed += Time.deltaTime;
	}	

	public void ResetLaser(){
		rest = true;
		Reset(this.parent.position + offSetCenter);
	}

	public void Reset(Vector3 pos, float sec = 1.0f){
		//yield return new WaitForSeconds(sec);
		transform.position = pos;
		rest = false;
		timeElapsed = 0.0f;
	}
}
