using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EntityCollidePowerUpEvent : UnityEvent<Entity, Entity>
{
	
}

[System.Serializable]
public class EntityCollideEnemyEvent : UnityEvent<Entity, Entity>
{
}

public class EventManager : MonoBehaviour {

	public EntityCollidePowerUpEvent entPowerupCollisionEvent;
	public EntityCollideEnemyEvent entEnemyCollisionEvent;
	public UnityEvent swipeLeftEvent;
	public UnityEvent swipeRightEvent;

	private static EventManager eventManager;

	public static EventManager instance
	{
		get
		{
			if (!eventManager)
			{
				eventManager = FindObjectOfType (typeof (EventManager)) as EventManager;

				if (!eventManager)
				{
					Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
				}
			}

			return eventManager;
		}
	}
}
