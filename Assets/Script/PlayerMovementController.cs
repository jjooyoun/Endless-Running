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
	public bool useAcc = false;


	public bool isJumping = false;
	public float height_Offset = 5.0f;
	public bool reachedTop = false;
	public bool reachedBottom = false;
	public float speed = 5.0f;
	private float originalY = 0.0f;
	private float topY = 0.0f;


	// Shake variables

	double accelerometerUpdateInterval = 1.0 / 60.0;
	// The greater the value of LowPassKernelWidthInSeconds, the slower the filtered value will converge towards current input sample (and vice versa).
	double lowPassKernelWidthInSeconds = 1.0;
	// This next parameter is initialized to 2.0 per Apple's recommendation, or at least according to Brady! ;)
	double shakeDetectionThreshold = 1.0;

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

		EventManager.instance.swipeLeftEvent.AddListener (onSwipeLeft);
		EventManager.instance.swipeRightEvent.AddListener (onSwipeRight);
		EventManager.instance.swipeUpEvent.AddListener (Jump);
		EventManager.instance.shakeEvent.AddListener (Shake);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate ( new Vector3(1,0,0) * ( 150.0f * Time.deltaTime ) );
		if (isJumping) {
			//Debug.Log (transform.position);
			if (!reachedTop) {
				if (MoveUp (speed, transform.position.y, topY)) {
					reachedTop = !reachedTop;
				}
			} else if (!reachedBottom) {
				if (MoveDown (speed, transform.position.y, originalY)) {
					reachedBottom = !reachedBottom;
					isJumping = !isJumping;
				}
			}
		} else {
			// Control by touch input 

			if(!LERPING && !isJumping) {
				/*if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Stationary) {
				Vector2 touchPosition = Input.GetTouch (0).position;
				double halfscreen = Screen.width / 2.0;
				if (touchPosition.x > halfscreen) {
					MoveRight ();
				}
				if (touchPosition.x < halfscreen) {
					MoveLeft ();
				}
			}
			*/
				// Accelerometer

				if (useAcc == true) {
					if (Input.acceleration.x > 0) {


						//  Left Wall
						if (destPos.x < RightWall.position.x - 2.5) {
							destPos = new Vector3 (transform.position.x + Input.acceleration.x, transform.position.y, transform.position.z);
							LERPING = true;
							currentLerpTime = 0f;
						}
					} else if (Input.acceleration.x < 0) {

						// Right Wall
						if (destPos.x > LeftWall.position.x + 2.5) {
							destPos = new Vector3 (transform.position.x + Input.acceleration.x, transform.position.y, transform.position.z);
							LERPING = true;
							currentLerpTime = 0f;
						}
					}
				}
			}
//			if(!LERPING) {
//				if(Input.GetKeyDown(KeyCode.LeftArrow)) {
//					MoveLeft();
//				}
//
//				if(Input.GetKeyDown(KeyCode.RightArrow)) {
//					MoveRight();
//				}
//			}

			if(LERPING && !isJumping) {
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
			

		if (ShakeDetection()) {
			Shake ();	
		}

	}

	bool ShakeDetection(){
		acceleration = Input.acceleration;
		lowPassValue = Vector3.Lerp(lowPassValue, acceleration, (float)lowPassFilterFactor);
		deltaAcceleration = acceleration - lowPassValue;
		return deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold;
	}

	void Shake(){
		// Perform your "shaking actions" here, with suitable guards in the if check above, if necessary to not, to not fire again if they're already being performed.
		Debug.Log("Shake event detected at time "+Time.time);
		Handheld.Vibrate();
		PowerUp.ScaleDown (this.transform);
		EventManager.instance.shakeOutputEvent.Invoke ();
	}

	public void MoveLeft() {
		//Debug.Log ("MoveLeft");
		if( currentLane == Lane.Left) {
			//do nothing, already in left lane
			//Debug.Log("Already in left!!");
		}
		if( currentLane == Lane.Center) {
			//Debug.Log("center>left");
			//move to left lane
			//GoLaneLeft();
			GoLane(LeftLanePos.position, Lane.Left);
		}
		if(currentLane == Lane.Right) {
			//Debug.Log ("right>center");
			//move to center lane
			//GoLaneCenter();
			GoLane(CenterLanePos.position, Lane.Center);
		}
	}

	public void MoveRight() {
		//Debug.Log ("MoveRight");
		if(currentLane == Lane.Right) {
			//Debug.Log("Already in right!!");
			//do nothing, already in right lane
		}
		if( currentLane == Lane.Center) {
			//Debug.Log("center>right");
			//move to Right lane
			//GoLaneRight();
			GoLane(RightLanePos.position, Lane.Right);
		}
		if( currentLane == Lane.Left) {
			//Debug.Log ("left>center");
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
	
	void onSwipeLeft(){
		if(!isJumping)
			MoveLeft ();
	}

	void onSwipeRight(){
		if(!isJumping)
			MoveRight ();
	}

	public void useAccelerometer(bool acc) {
		useAcc = acc;
	}

	public void Jump(){
		//Debug.Log ("im jumping!!!");
		if (!isJumping) {
			originalY = transform.position.y;
			topY = transform.position.y + height_Offset;
			reachedTop = false;
			reachedBottom = false;
			isJumping = !isJumping;
		}
	}

	public bool MoveUp(float speed, float currentY, float destY){
		float newY = currentY + speed * Time.deltaTime;
		if (newY > destY) {
			transform.position = new Vector3 (transform.position.x, destY, transform.position.z);
			return true;
		}
		transform.position = new Vector3 (transform.position.x, newY, transform.position.z);
		return false;
	}

	public bool MoveDown(float speed, float currentY, float destY){
		float newY = currentY - speed * Time.deltaTime;
		if (newY < destY) {
			transform.position = new Vector3 (transform.position.x, destY, transform.position.z);
			return true;
		}
		transform.position = new Vector3 (transform.position.x, newY, transform.position.z);
		return false;
	}

}
