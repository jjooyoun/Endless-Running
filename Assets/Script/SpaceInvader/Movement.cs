using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	bool canMoveRight(float dist, float bound, ref Vector3 newPos){
		float newX = transform.position.x + dist;
		if (newX <= bound) {
			newPos = new Vector3 (dist, 0.0f, 0.0f);
			return true;
		}
		return false;
	}

	public bool MoveRight(float dist, float bound){
		Vector3 newPos = Vector3.zero;
		bool movable = canMoveRight (dist, bound, ref newPos);
		if (movable) {
			transform.position += newPos;
		}
		return movable;
	}

	bool canMoveLeft(float dist, float bound, ref Vector3 newPos){
		float newX = transform.position.x - dist;
		if (newX >= bound) {
			newPos= new Vector3 (-dist, 0.0f, 0.0f);
			return true;
		}
		return false;
	}

	public bool MoveLeft(float dist, float bound){
		Vector3 newPos = Vector3.zero;
		bool movable = canMoveLeft (dist, bound, ref newPos);
		if (movable) {
			transform.position += newPos;
		}
		return movable;
	}


	bool canMoveUp(float dist, float bound, ref Vector3 newPos){
		float newY = transform.position.y + dist;
		if (newY <= bound /*&&  newY >= -bound*/) {
			newPos = new Vector3 (0.0f, dist, 0.0f);
			return true;
		}
		return false;
	}


	public bool MoveUp(float dist, float bound){
		Vector3 newPos = Vector3.zero;
		bool movable = canMoveUp (dist, bound, ref newPos);
		if (movable) {
			transform.position += newPos;
		}
		return movable;
	}

	bool canMoveDown(float dist, float bound, ref Vector3 newPos){
		float newY = transform.position.y - dist;
		if (newY >= bound) {
			newPos = new Vector3 (0.0f, -dist, 0.0f);
			return true;
		}
		return false;
	}



	public bool MoveDown(float dist, float bound){
		Vector3 newPos = Vector3.zero;
		bool movable = canMoveDown (dist, bound, ref newPos);
		if (movable) {
			transform.position += newPos;
		}
		return movable;
	}

	public bool MoveDiagonalUpRight(float distX, float distY, float boundX, float boundY){
		Vector3 upPos = Vector3.zero, rightPos = Vector3.zero;
		bool ret = canMoveUp(distY, boundY, ref upPos) && canMoveRight(distX, boundX, ref rightPos);
		if (ret) {
			MoveUp (distY, boundY);
			MoveRight (distX, boundX);
		}
		return ret;
	}

	public bool MoveDiagonalDownRight(float distX, float distY, float boundX, float boundY){
		Vector3 dnPos = Vector3.zero, rightPos = Vector3.zero;
		bool ret = canMoveDown(distY, boundY, ref dnPos) && canMoveRight(distX, boundX, ref rightPos);
		if (ret) {
			MoveRight (distX, boundX);
			MoveDown (distY, boundY);
		}
		return ret;
	}

	public bool MoveDiagonalDownLeft(float distX, float distY, float boundX, float boundY){
		Vector3 dnPos = Vector3.zero, leftPos = Vector3.zero;
		bool ret = canMoveDown(distY, boundY, ref dnPos) && canMoveLeft(distX, boundX, ref leftPos);
		if (ret) {
			MoveDown (distY, boundY);
			MoveLeft (distX, boundX);
		}
		return ret;
	}

	public bool MoveDiagonalUpLeft(float distX, float distY, float boundX, float boundY){
		Vector3 upPos = Vector3.zero, leftPos = Vector3.zero;
		bool ret = canMoveUp(distY, boundY, ref upPos) && canMoveLeft(distX, boundX, ref leftPos);
		if (ret) {
			MoveLeft(distX, boundX);
			MoveUp (distY, boundY);
		}
		return ret;
	}

}
