using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {
    public Transform sun;
    public Transform moon;
    public float speed = 5.0f;
	public Vector3 lookAt = Vector3.zero;
	// Use this for initialization
	void Start () {
	    
	}
	

    void moveAround(Transform tf, Vector3 point, Vector3 axis, float speed)
    {
		tf.LookAt(point);
        tf.RotateAround(point, axis, speed*Time.deltaTime );
    }

	// Update is called once per frame
	void Update () {
		moveAround(sun, lookAt, Vector3.right, speed);
		moveAround(moon, lookAt, Vector3.right, speed);
    }
}
