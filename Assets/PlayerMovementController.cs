using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

	public Transform LeftLanePos;
	public Transform CenterLanePos;
	public Transform RightLanePos;

	Lane currentLane;


	enum Lane {Left, Center, Right};


	// Use this for initialization
	void Start () {
		currentLane = Lane.Center;
		transform.position = CenterLanePos.position;

	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate ( new Vector3(1,0,0) * ( 150.0f * Time.deltaTime ) );
			
		if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			MoveLeft();
		}

		if(Input.GetKeyDown(KeyCode.RightArrow)) {
			MoveRight();
		}
	}

	void MoveLeft() {
		if( currentLane == Lane.Left) {
			//do nothing, already in left lane
		}
		if( currentLane == Lane.Center) {
			//move to left lane
			GoLaneLeft();
		}
		if(currentLane == Lane.Right) {
			//move to center lane
			GoLaneCenter();
		}
	}

	void MoveRight() {
		if(currentLane == Lane.Right) {
			//do nothing, already in right lane
		}
		if( currentLane == Lane.Center) {
			//move to Right lane
			GoLaneRight();
		}
		if( currentLane == Lane.Left) {
			//move to center lane
			GoLaneCenter();
		}
	}


	void GoLaneLeft() {
		transform.position = LeftLanePos.position;
		currentLane = Lane.Left;

	}

	void GoLaneCenter() {
		transform.position = CenterLanePos.position;
		currentLane = Lane.Center;
			
	}

	void GoLaneRight() {
		transform.position = RightLanePos.position;
		currentLane = Lane.Right;
			
	}


}
