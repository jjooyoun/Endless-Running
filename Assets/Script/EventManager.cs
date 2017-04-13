using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EntityEntityCollision : UnityEvent<Entity, Entity>
{

}

[System.Serializable]
public class SpawningNumEvent : UnityEvent<int>
{}


public class EventManager : Singleton<EventManager> {
	protected EventManager(){}
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
	public UnityEvent shieldDownEvent; // to test shield down
	public UnityEvent scaleUpEvent;
	public UnityEvent scaleDownEvent;
	public UnityEvent stage1;
	public SpawningNumEvent spawningNumEvent;
	public UnityEvent levelFinishedEvent;

	public UnityEvent finishedSpawningEvent;

	void OnDestroy(){
		stage1.RemoveAllListeners ();
	}
}
