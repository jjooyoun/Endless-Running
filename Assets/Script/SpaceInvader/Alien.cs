using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : Ent2D {
	public bool startMoving = false;
	private int index = 0; //move direction
    public float distDivider = 1.0f;
	public string AlienType = "alien";
	//driving index by an array for example

	// void Start(){
	// 	Init ();
	// }

	public override void EntUpdate () {
		//Debug.Log("base shootable=" + base.shootable);
        //Debug.Log("alien Shootable:" + this.shootable);
		if (shootable && !isShooting)
        {
			//Debug.Log("Create bomb");
            Ent2D.CreateBomb(child, this, -DIST_Y, UP_BOUND_Y/*7.79f*/, DOWN_BOUND_Y/*-7.79f*/, false);
        }
		if (startMoving && !MoveIndex (index)) {
			index = ( index + 1 )%2;
		}
	}

	bool moveRightWrapper(){
		bool ret = mover.MoveRight (DIST_X / distDivider, RIGHT_BOUND_X - DIST_X);
		if(!ret){
			//Debug.Log("RBX-DX:" + (RIGHT_BOUND_X - DIST_X/distDivider));
			transform.position = new Vector3(RIGHT_BOUND_X - DIST_X, transform.position.y, transform.position.z);
			//Debug.Log("posx:" + transform.position.x);
		}
		return ret;
	}

	bool moveLeftWrapper(){
		bool ret = mover.MoveLeft (DIST_X/ distDivider, LEFT_BOUND_X + DIST_X);
		if(!ret){
			transform.position = new Vector3(LEFT_BOUND_X + DIST_X, transform.position.y, transform.position.z);
		}
		return ret;
	}


	bool MoveIndex(int index){
		bool ret = false;
		switch (index) {
		case 0:
			//ret = mover.MoveRight (DIST_X / distDivider, RIGHT_BOUND_X);
			ret = moveRightWrapper();
			break;
		case 1:
			//ret = mover.MoveLeft (DIST_X/ distDivider, LEFT_BOUND_X);
			ret = moveLeftWrapper();
			break;
		}

		return ret;
	}

    public override void OnDie(Collider2D other)
    {
        base.OnDie(other);
		//Debug.Log("shootable = " + shootable);
		//Debug.Log("isZombie=" + isZombie);
        //broadcast die event
        EventDispatcher.Instance.AlienDieEvent.Invoke(this);
    }
}
