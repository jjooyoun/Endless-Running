using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Ent2D{
	public UnityEvent TakeDamageEvent; // drytest

	// public void OnSpawn(){
	// 	Init();
	// }

	float ScreenX(){
		return Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
	}

	void moveRightWrapper(){
		if(!mover.MoveRight (DIST_X, RIGHT_BOUND_X - DIST_X)){
			transform.position = new Vector3(RIGHT_BOUND_X - DIST_X, transform.position.y, transform.position.z);
		}
	}

	void moveLeftWrapper(){
		if(!mover.MoveLeft (DIST_X, LEFT_BOUND_X + DIST_X)){
			transform.position = new Vector3(LEFT_BOUND_X + DIST_X, transform.position.y, transform.position.z);
		}
	}

	// Update is called once per frame
	public override void EntUpdate () {
		if(isZombie)
			return;
		//not dead
		//Debug.Log ("width left:" + Camera.main.ScreenToWorldPoint (new Vector3 (-Screen.width, 0.0f, 0.0f)).x);
		if (Input.GetMouseButtonDown (0) && transform.position.x <  ScreenX()) {
			moveRightWrapper();
		} else if (Input.GetMouseButtonDown (0) && transform.position.x > ScreenX()) {
			moveLeftWrapper();
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			moveRightWrapper();
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			moveLeftWrapper();
		}

		if (Input.GetKeyDown (KeyCode.D)) {
			TakeDamageEvent.Invoke (); //die
		}

		if (/*Input.GetKeyDown (KeyCode.Space) &&*/ !isShooting && shootable) { //autofire
            Ent2D.CreateBomb(child, this, DIST_Y, UP_BOUND_Y, -DOWN_BOUND_Y);
		}
	}

	public void EnableShoot(){
		shootable = true;
	}

	public void OnDieTest(){
		//nothing
		OnDie(this.GetComponent<BoxCollider2D>());
	}

	public override void OnDie(Collider2D other){
		base.OnDie(other);
		//fire player event
		//EventDispatcher.Instance.PlayerDieEvent.Invoke();
	}

	public override void OnDoneDie(){
		EventDispatcher.Instance.PlayerDieEvent.Invoke();
	}

	public void OnRespawn(){
		isZombie = false;
		shootable = true;
		GetComponent<BoxCollider2D>().isTrigger = true;
		sr.sprite = spawnSprite;
		sr.enabled = true;
	}

	public void OnGameOver(){
		Destroy(gameObject);
	}
}
