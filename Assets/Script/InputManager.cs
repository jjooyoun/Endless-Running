using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//attach to the ball for now
public class InputManager : Singleton<InputManager> {
	protected InputManager(){}

	//private static bool Jumpnable = false;
	//private static bool Shakeable = false;
	private Vector3 touchPosition;
	private float swipeResistanceX = 50.0f;
	private float swipeResistanceY = 100.0f;

//	public static void SetShakeable(bool shakeable){
//		Shakeable = shakeable;
//	}
//
//	public static void SetJump(bool jumpable){
//		Jumpnable = jumpable;
//	}

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
					EventManager.Instance.swipeRightEvent.Invoke ();
				} else {
					EventManager.Instance.swipeLeftEvent.Invoke ();
				}


			}

			if (Mathf.Abs (deltaSwipe.y) > swipeResistanceY) {
				// Swipe on the Y axis
				if (deltaSwipe.y <= 0) {
					EventManager.Instance.swipeUpEvent.Invoke ();
				}
			}
		}

		if (Input.GetKeyDown (Setting.Instance.gameSetting.JUMP_KEY)) {
			if (Setting.Instance.gameSetting.enableJump) {
				/*Debug.Log ("listeners:" + EventManager.Instance.swipeUpEvent.GetPersistentEventCount ());
				for( int i = 0 ; i < EventManager.Instance.swipeUpEvent.GetPersistentEventCount(); i++){
					Debug.Log(EventManager.Instance.swipeUpEvent.GetPersistentMethodName(i));
				}*/
				EventManager.Instance.swipeUpEvent.Invoke ();
			}
		}

		if (Input.GetKeyDown (Setting.Instance.gameSetting.SHAKE_KEY)) {
			if(Setting.Instance.gameSetting.enableShake)
				EventManager.Instance.shakeEvent.Invoke (); //to test shake
		}

		if (Input.GetKeyDown (Setting.Instance.gameSetting.SHIELD_UP_KEY)) {
			EventManager.Instance.shield.Invoke ();
		}

		if (Input.GetKeyDown (Setting.Instance.gameSetting.SHIELD_DOWN_KEY)) {
			EventManager.Instance.shieldDownEvent.Invoke ();
		}

		if (Input.GetKeyDown (Setting.Instance.gameSetting.SCALE_UP_KEY)) {
			EventManager.Instance.scaleUpEvent.Invoke ();
		}

		if (Input.GetKeyDown (Setting.Instance.gameSetting.SCALE_DOWN_KEY)) {
			EventManager.Instance.scaleDownEvent.Invoke ();
		}

		if(Input.GetKeyDown(Setting.Instance.gameSetting.LEFT_KEY)) {
			EventManager.Instance.swipeLeftEvent.Invoke ();
		}

		if(Input.GetKeyDown(Setting.Instance.gameSetting.RIGHT_KEY)) {
			EventManager.Instance.swipeRightEvent.Invoke ();
		}
		
	}
}


