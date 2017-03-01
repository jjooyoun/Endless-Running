using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundControl : MonoBehaviour {
	private DistanceSystem distancesystem;
	float speed = 0.5f;
	void Start()
	{
		distancesystem = FindObjectOfType<DistanceSystem> ();

	}
    //Material texture offset rate

    //Offset the material texture at a constant rate
    void Update()
    {

		if (distancesystem.rellapsed > 20) {
			speed = 0.7f;
		} else if (distancesystem.rellapsed > 60) {
			speed = 0.9f;
		}
		  else if (distancesystem.rellapsed > 90) {
			speed = 1f;
		}
			
		float offset = Time.time * speed;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, -offset);

	}
}
