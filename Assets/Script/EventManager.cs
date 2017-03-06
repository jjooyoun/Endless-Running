using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EntityEntityCollision : UnityEvent<Entity, Entity>
{

}

public class EventManager : MonoBehaviour {

	public EntityEntityCollision entPowerupCollisionEvent;
	public EntityEntityCollision entObstacleCollisionEvent;
	public EntityEntityCollision entEnemyCollisionEvent;
	public EntityEntityCollision FlashAndLoseLiveEvent;
	public UnityEvent level1AchievementEvent;
	public UnityEvent level2AchievementEvent;
	public UnityEvent level3AchievementEvent;
	public UnityEvent level4AchievementEvent;
	public UnityEvent level5AchievementEvent;
	public UnityEvent level7AchievementEvent;
	public UnityEvent shakeEvent;
	public UnityEvent shakeOutputEvent;
	public UnityEvent swipeLeftEvent;
	public UnityEvent swipeRightEvent;
	public UnityEvent swipeUpEvent;
	public UnityEvent pauseEvent;
	public UnityEvent resumeEvent;
	public UnityEvent shield;

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
