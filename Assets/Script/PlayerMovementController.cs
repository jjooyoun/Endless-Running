using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

	public Transform LeftLanePos;
	public Transform CenterLanePos;
	public Transform RightLanePos;

	Lane currentLane;
	enum Lane {Left, Center, Right};

	float lerpTime = 0.2f;
	float currentLerpTime;
	bool LERPING;
	Vector3 destPos;


	// Use this for initialization
	void Start () {
		currentLane = Lane.Center;
		transform.position = CenterLanePos.position;

		LERPING = false;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate ( new Vector3(1,0,0) * ( 150.0f * Time.deltaTime ) );

		if(!LERPING) {
			if(Input.GetKeyDown(KeyCode.LeftArrow)) {
				MoveLeft();
			}

			if(Input.GetKeyDown(KeyCode.RightArrow)) {
				MoveRight();
			}
		}

		if(LERPING) {
			currentLerpTime += Time.deltaTime;
			if (currentLerpTime > lerpTime) {
				currentLerpTime = lerpTime;
				LERPING = false;
			}

			//lerp!
			float perc = currentLerpTime / lerpTime;
			transform.position = Vector3.Lerp(transform.position, destPos, perc);
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
//		transform.position = LeftLanePos.position;
		destPos = LeftLanePos.position;
		LERPING = true;
		currentLane = Lane.Left;
		currentLerpTime = 0f;

	}

	void GoLaneCenter() {
//		transform.position = CenterLanePos.position;
		destPos = CenterLanePos.position;
		LERPING = true;
		currentLane = Lane.Center;
		currentLerpTime = 0f;
			
	}

	void GoLaneRight() {
//		transform.position = RightLanePos.position;
		destPos = RightLanePos.position;
		LERPING = true;
		currentLane = Lane.Right;
		currentLerpTime = 0f;
			
	}


}
