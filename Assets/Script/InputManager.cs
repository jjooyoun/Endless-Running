﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//attach to the ball for now
public class InputManager : Singleton<InputManager> {
	protected InputManager(){}

	private Vector3 touchPosition;
	private float swipeResistanceX = 50.0f;
	private float swipeResistanceY = 100.0f;

	// Update is called once per frame
	private void Update () {
		//JUMP1
		if (Input.GetMouseButtonDown (0)) {
			touchPosition = Input.mousePosition;
			
			if (!Setting.gameSetting.isPaused && Setting.gameSetting.enableJump && touchPosition.y <= Screen.height/2.0f) {
				EventManager.Instance.swipeUpEvent.Invoke ();
			}
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
			//force to pick one or the other
			else if (Mathf.Abs (deltaSwipe.y) > swipeResistanceY) {
				// Swipe on the Y axis
				if (deltaSwipe.y <= 0) {
					EventManager.Instance.swipeUpEvent.Invoke ();
				}
			}
		}
		//JUMP2
		if (Input.GetKeyDown (Setting.gameSetting.JUMP_KEY)) {
			if (Setting.gameSetting.enableJump) {
				EventManager.Instance.swipeUpEvent.Invoke ();
			}
		}

		if (Input.GetKeyDown (Setting.gameSetting.SHAKE_KEY)) {
			if(Setting.gameSetting.enableShake)
				EventManager.Instance.shakeEvent.Invoke (); //to test shake
		}

		if (Input.GetKeyDown (Setting.gameSetting.SHIELD_UP_KEY)) {
			EventManager.Instance.shield.Invoke ();
		}

		if (Input.GetKeyDown (Setting.gameSetting.SHIELD_DOWN_KEY)) {
			EventManager.Instance.shieldDownEvent.Invoke ();
		}

		if (Input.GetKeyDown (Setting.gameSetting.SCALE_UP_KEY)) {
			EventManager.Instance.scaleUpEvent.Invoke ();
		}

		if (Input.GetKeyDown (Setting.gameSetting.SCALE_DOWN_KEY)) {
			EventManager.Instance.scaleDownEvent.Invoke ();
		}

		if(Input.GetKeyDown(Setting.gameSetting.LEFT_KEY) && !Setting.gameSetting.isPaused) {
			EventManager.Instance.swipeLeftEvent.Invoke ();
		}

		if(Input.GetKeyDown(Setting.gameSetting.RIGHT_KEY) && !Setting.gameSetting.isPaused) {
			EventManager.Instance.swipeRightEvent.Invoke ();
		}
		
	}
}


