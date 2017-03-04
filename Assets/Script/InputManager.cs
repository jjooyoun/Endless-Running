using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//attach to the ball for now
public class InputManager : MonoBehaviour {

//	private static SwipeManager instance;
//	public static SwipeManager Instance{get {return instance;}}

	private static bool Jumpnable = false;
	private static bool Shakeable = false;
	private Vector3 touchPosition;
	private float swipeResistanceX = 50.0f;
	private float swipeResistanceY = 100.0f;

	public static void SetShakeable(bool shakeable){
		Shakeable = shakeable;
	}

	public static void SetJump(bool jumpable){
		Jumpnable = jumpable;
	}

	// Update is called once per frame
	private void Update () {
		if (Input.GetMouseButtonDown (0)) {
			touchPosition = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp (0)) {
			Vector2 deltaSwipe = touchPosition - Input.mousePosition;

			if (Mathf.Abs (deltaSwipe.x) > swipeResistanceX) {
				// Swipe on the X axis
				//Direction |= (deltaSwipe.x < 0) ? SwipeDirection.Right : SwipeDirection.Left;
				if (deltaSwipe.x < 0) {
					EventManager.instance.swipeRightEvent.Invoke ();
				} else {
					EventManager.instance.swipeLeftEvent.Invoke ();
				}


			}

			if (Mathf.Abs (deltaSwipe.y) > swipeResistanceY) {
				// Swipe on the Y axis
				if (deltaSwipe.y <= 0) {
					EventManager.instance.swipeUpEvent.Invoke ();
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			if(Jumpnable)
				EventManager.instance.swipeUpEvent.Invoke ();
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			if(Shakeable)
				EventManager.instance.shakeEvent.Invoke (); //to test shake
		}

		if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			EventManager.instance.swipeLeftEvent.Invoke ();
		}

		if(Input.GetKeyDown(KeyCode.RightArrow)) {
			EventManager.instance.swipeRightEvent.Invoke ();
		}
		
	}
}


