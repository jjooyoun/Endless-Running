using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Ent2D {
	public Ent2D parent;
	public bool fromShip = false;
	private bool lastMove = false;

	public override void EntUpdate () {
		// //parent dead, dont do anything
		// if(parent.isZombie)
		// 	return;
		if (fromShip) {
            //Debug.Log("move up");
			lastMove = mover.MoveUp (DIST_Y, UP_BOUND_Y);
		} else {
			lastMove = mover.MoveDown (DIST_Y, DOWN_BOUND_Y);
		}

		if (!lastMove) { // past the end of the screen(Y,-Y)
			//Debug.Log("move no more!!");
			OnDie(null);
		}
	}

	public void SetOwner(Ent2D parent, bool fromShip){
		this.fromShip = fromShip;
		this.parent = parent;
		if (this.fromShip)
			this.sr.flipY = true;
	}

	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log ("collided with:" + other.name);
		//check colliding with another bomb
		Bomb otherBomb = other.GetComponent<Bomb>();
		if(otherBomb){
			//Debug.Log ("collided with otherBomb");
			OnDie(other);
			otherBomb.OnDie(null);
			return;
		}

		Ent2D otherEnt = other.GetComponent<Ent2D> ();
		if (otherEnt && otherEnt != parent && !otherEnt.isZombie) {
			//destroy the bomb
			Debug.Log("collided with ent: " + otherEnt.name);
			otherEnt.OnDie (other);
			OnDie (other);
		}
	}

	public override void OnDie(Collider2D other){
		//Debug.Log ("on bomb die!");
		base.OnDie(other);
		if (parent && !parent.isZombie) {
			parent.shootable = true;
			parent.isShooting = false;
		}
		Destroy (gameObject);
	}
}
