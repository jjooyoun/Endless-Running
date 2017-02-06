using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

	public Transform LeftLanePos;
	public Transform CenterLanePos;
	public Transform RightLanePos;


	// Use this for initialization
	void Start () {
		transform.position = CenterLanePos.position;

	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate ( new Vector3(1,0,0) * ( 150.0f * Time.deltaTime ) );

		if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			SwitchLaneLeft();
		}

		if(Input.GetKeyDown(KeyCode.UpArrow)) {
			SwitchLaneCenter();
		}

		if(Input.GetKeyDown(KeyCode.RightArrow)) {
			SwitchLaneRight();
		}


			
	}


	void SwitchLaneLeft() {
		transform.position = LeftLanePos.position;
	}

	void SwitchLaneCenter() {
		transform.position = CenterLanePos.position;

	}

	void SwitchLaneRight() {
		transform.position = RightLanePos.position;

	}
}
