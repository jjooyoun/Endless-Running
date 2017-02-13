using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//nothing
public class Entity : MonoBehaviour {
	public enum ENTITY_TYPE{
		PLAYER,
		ENEMY,
		POWER_UP
	};

	public ENTITY_TYPE entityType =  ENTITY_TYPE.PLAYER;
}
