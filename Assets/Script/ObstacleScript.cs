using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour {

    public float objectSpeed = -0.5f;
	//public float objectSpeed = Setting.Instance.gameSetting.objectSpeed;

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
		objectSpeed = Setting.Instance.gameSetting.objectSpeed;
		EventManager.Instance.pauseEvent.AddListener (OnPause);
		EventManager.Instance.resumeEvent.AddListener (OnResume);
	}

    void Update()
    {
        transform.Translate(0, 0, objectSpeed);
    }
}
