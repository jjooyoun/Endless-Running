using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour {

    public float objectSpeed = -0.5f;

	private float savedObjectSpeed = 0.0f;

	void OnPause(){
		savedObjectSpeed = objectSpeed;
		objectSpeed = 0.0f;
	}

	void OnResume(){
		objectSpeed = savedObjectSpeed;
		savedObjectSpeed = 0.0f;
	}

	void Start(){
		EventManager.instance.pauseEvent.AddListener (OnPause);
		EventManager.instance.resumeEvent.AddListener (OnResume);
	}

    void Update()
    {
        transform.Translate(0, 0, objectSpeed);
    }
}
