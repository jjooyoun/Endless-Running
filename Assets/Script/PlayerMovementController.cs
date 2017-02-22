using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

	public Transform LeftLanePos;
	public Transform CenterLanePos;
	public Transform RightLanePos;

	public Transform LeftWall;
	public Transform RightWall;

	Lane currentLane;
	enum Lane {Left, Center, Right};

	float lerpTime = 0.2f;
	float currentLerpTime;
	bool LERPING;
	Vector3 destPos;

	float speed = 10.0f;

	// Shake variables

	double accelerometerUpdateInterval = 1.0 / 60.0;
	// The greater the value of LowPassKernelWidthInSeconds, the slower the filtered value will converge towards current input sample (and vice versa).
	double lowPassKernelWidthInSeconds = 1.0;
	// This next parameter is initialized to 2.0 per Apple's recommendation, or at least according to Brady! ;)
	double shakeDetectionThreshold = 2.0;

	//double lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
	private double lowPassFilterFactor = 1.0/60.0;
	private Vector3 lowPassValue = Vector3.zero;
	private Vector3 acceleration ;
	private Vector3 deltaAcceleration;


	// Use this for initialization
	void Start () {
		currentLane = Lane.Center;
		transform.position = CenterLanePos.position;

		LERPING = false;

		// Shake detection
		shakeDetectionThreshold *= shakeDetectionThreshold;
		lowPassValue = Input.acceleration;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("x:" + transform.position.x);
		Debug.Log ("left:" + LeftWall.position.x);
		Debug.Log ("right:" + RightWall.position.x);
		transform.Rotate ( new Vector3(1,0,0) * ( 150.0f * Time.deltaTime ) );

		// Control by touch input 
		if(!LERPING) {
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Stationary) {
				Vector2 touchPosition = Input.GetTouch (0).position;
				double halfscreen = Screen.width / 2.0;
				if (touchPosition.x > halfscreen) {
					MoveRight ();
				}
				if (touchPosition.x < halfscreen) {
					MoveLeft ();
				}
			}

			// Accelerometer
			if (Input.acceleration.x > 0) {
				

				//  Left Wall
				if(destPos.x < RightWall.position.x - 2){
					destPos = new Vector3(transform.position.x + Input.acceleration.x,transform.position.y, transform.position.z);
					LERPING = true;
					currentLerpTime = 0f;
				}
			}
			else if (Input.acceleration.x < 0) {
				
				// Right Wall
				if (destPos.x > LeftWall.position.x + 2) {
					destPos = new Vector3 (transform.position.x + Input.acceleration.x, transform.position.y, transform.position.z);
					LERPING = true;
					currentLerpTime = 0f;
				}
			}
		}
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

		acceleration = Input.acceleration;
		lowPassValue = Vector3.Lerp(lowPassValue, acceleration, (float)lowPassFilterFactor);
		deltaAcceleration = acceleration - lowPassValue;
		if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
		{
			// Perform your "shaking actions" here, with suitable guards in the if check above, if necessary to not, to not fire again if they're already being performed.
			Debug.Log("Shake event detected at time "+Time.time);
			Handheld.Vibrate();
			PowerUp.ScaleDown (this.transform);
		}

	}

	void MoveLeft() {
		Debug.Log ("MoveLeft");
		if( currentLane == Lane.Left) {
			//do nothing, already in left lane
			Debug.Log("Already in left!!");
		}
		if( currentLane == Lane.Center) {
			Debug.Log("center>left");
			//move to left lane
			//GoLaneLeft();
			GoLane(LeftLanePos.position, Lane.Left);
		}
		if(currentLane == Lane.Right) {
			Debug.Log ("right>center");
			//move to center lane
			//GoLaneCenter();
			GoLane(CenterLanePos.position, Lane.Center);
		}
	}

	void MoveRight() {
		if(currentLane == Lane.Right) {
			//do nothing, already in right lane
		}
		if( currentLane == Lane.Center) {
			//move to Right lane
			//GoLaneRight();
			GoLane(RightLanePos.position, Lane.Right);
		}
		if( currentLane == Lane.Left) {
			//move to center lane
			//GoLaneCenter();
			GoLane(CenterLanePos.position, Lane.Center);
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

	void GoLane(Vector3 lane, Lane laneEnum){
		destPos = new Vector3(lane.x, transform.position.y, lane.z); // keep current y
		LERPING = true;
		currentLane = laneEnum;
		currentLerpTime = 0f;
	}

	Vector3 calDestByAcc(float s, float acc) {
		LERPING = true;

		Vector3 dir = Vector3.zero;

		dir.x = acc;

		if (dir.sqrMagnitude > 1) {
			dir.Normalize ();
		}

		dir *= Time.deltaTime;

		return dir * s;
	}


}
