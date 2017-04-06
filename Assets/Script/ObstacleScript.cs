using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour {

    public float objectSpeed = -0.5f;
	public bool startMoving = true; //to test
	//public float objectSpeed = Setting.Instance.gameSetting.objectSpeed;

	private float savedObjectSpeed = 0.0f;

	void OnPause(){
		//Debug.Log(name + ":onPause:");
		savedObjectSpeed = objectSpeed;
		objectSpeed = 0.0f;
	}

	void OnResume(){
		objectSpeed = savedObjectSpeed;
		savedObjectSpeed = 0.0f;
	}

	void Start(){
		objectSpeed = Setting.gameSetting.objectSpeed;
		EventManager.Instance.pauseEvent.AddListener (OnPause);
		EventManager.Instance.resumeEvent.AddListener (OnResume);
	}

    void Update()
    {
		if(startMoving)
        	transform.Translate(0, 0, objectSpeed);
    }
}
