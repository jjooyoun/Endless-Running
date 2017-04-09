using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Ent2D {
	private int index = 0;
    private int indexSize = 4;
    public float diameterX = 5.0f;
	public float diameterY = 5.0f;
	public float distDivider = 1.0f;
	public bool startMoving = false;
	public bool dev = false;
	private int count = 0;


	//testing movement
//	void Start(){
//		Init(transform.position.x, transform.position.x + diameterX, transform.position.y + diameterY/2.0f, transform.position.y - diameterY/2.0f);
//		if (dev) {
//			CreateCirc (new Vector3 (transform.position.x, transform.position.y, transform.position.z), Color.green); //lbx
//			CreateCirc (new Vector3 (transform.position.x + diameterX, transform.position.y, transform.position.z), Color.green); //rbx
//			CreateCirc (new Vector3 (transform.position.x + diameterX/2.0f, transform.position.y + diameterY/2.0f, transform.position.z), Color.green); //uby
//			CreateCirc (new Vector3 (transform.position.x + diameterX/2.0f, transform.position.y - diameterY/2.0f, transform.position.z), Color.green); //dby
//		}
//	}

	public override void EntUpdate(){
		if (shootable)
		{
			//Debug.Log("Create bomb");
			Ent2D.CreateBomb(child, this, -DIST_Y, 7.79f, -7.79f, false);
		}
		if (startMoving && !MoveIndex (index)) {
			if (index >= indexSize)
				return;
			index = ( index + 1 )%indexSize;
		}
	}

	bool MoveIndex(int index){
		Vector3 prevPos = transform.position;
		bool ret = false;
		switch (index) {
		case 0:
			ret = mover.MoveDiagonalUpRight(DIST_X/distDivider, DIST_Y/distDivider, LEFT_BOUND_X + diameterX/2.0f, UP_BOUND_Y);
//			if(ret)
//				Debug.Log("up-right!");
			break;
		case 1:
			//Debug.Log ("here?");
			ret = mover.MoveDiagonalDownRight(DIST_X/distDivider, DIST_Y/distDivider, RIGHT_BOUND_X, DOWN_BOUND_Y - diameterY/2.0f);
//			if(ret)
//				Debug.Log ("down-right!");	
			break;
		case 2:
			ret = mover.MoveDiagonalDownLeft(DIST_X/distDivider, DIST_Y/distDivider, LEFT_BOUND_X + diameterX/2.0f, DOWN_BOUND_Y - diameterY/2.0f);
//			if(ret)
//				Debug.Log ("down-left!");
            break;
		case 3:
			
			ret = mover.MoveDiagonalUpLeft(DIST_X/distDivider, DIST_Y/distDivider, LEFT_BOUND_X, UP_BOUND_Y - diameterY/2.0f);
//			if(ret)
//				Debug.Log ("up-left!");
            break;

        }
		if(dev && ret){
			CreateCirc (prevPos, Color.red);
		}
		return ret;
	}

	public override void OnDie(Collider2D other){
		base.OnDie(other);
        //broadcast die event
        EventDispatcher.Instance.GameBeatEvent.Invoke();
	}
}
