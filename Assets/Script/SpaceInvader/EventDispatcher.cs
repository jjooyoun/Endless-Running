using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AlienDieUnityEvent : UnityEvent<Alien> {}

[System.Serializable]
public class ShootableRowEradicateUnityEvent : UnityEvent<int> {}
public class EventDispatcher : MonoBehaviour {
	public UnityEvent PlayerDieEvent;
	public AlienDieUnityEvent AlienDieEvent;
	//public UnityEvent PlayerFinishedDyingEvent;
	public ShootableRowEradicateUnityEvent ShootableRowEradicateEvent;

	public UnityEvent GameStartEvent;
	public UnityEvent GameOverEvent;

	public UnityEvent GameBeatEvent;

	public UnityEvent QuitGameEvent;
 	public static EventDispatcher Instance = null;    
	 //Awake is always called before any Start functions
	void Awake()
	{
		//Check if instance already exists
		if (Instance == null)
			//if not, set instance to this
			Instance = this;
		//If instance already exists and it's not this:
		else if (Instance != this)
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
	}
}
