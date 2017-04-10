using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Swipe : MonoBehaviour {

	private static Swipe instance;
	public static Swipe Instance{get {return instance;}}

	private Vector3 touchPosition;
	private float swipeResistanceX = 50.0f;
	private float swipeResistanceY = 100.0f;

	private void Start()
	{
		instance = this;
	}


	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			touchPosition = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(0))
		{
			Vector2 deltaSwipe = touchPosition - Input.mousePosition;

				if (Mathf.Abs(deltaSwipe.x) > swipeResistanceX) 
			{
				// Swipe on Xaxis
			}
				if (Mathf.Abs(deltaSwipe.y) > swipeResistanceY) 
			{
			
			}
		}
	}
}