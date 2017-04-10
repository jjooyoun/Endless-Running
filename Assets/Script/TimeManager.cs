using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {
    public Transform sun;
    public Transform moon;
    public float speed = 5.0f;
	public Vector3 lookAt = Vector3.zero;

	private ParticleSystem pSys;

	// Use this for initialization
	void Start () {
		pSys = GetComponent<ParticleSystem> ();
	}
	

    void moveAround(Transform tf, Vector3 point, Vector3 axis, float speed)
    {
		tf.LookAt(point);
        tf.RotateAround(point, axis, speed*Time.deltaTime );
    }

	// Update is called once per frame
	void Update () {
		if(Setting.gameSetting.isPaused){
			pSys.Pause ();
			return;
		}
		if (sun.position.y < lookAt.y) {
			//Debug.Log ("star pulsing");
			StarEnable (true);
		} else {
			StarEnable (false);
		}

		moveAround(sun, lookAt, Vector3.right, speed);
		moveAround(moon, lookAt, Vector3.right, speed);
    }

	void StarEnable(bool enabled){
		if (enabled && !pSys.isPlaying) {
			pSys.Play ();
		} else if (!enabled && pSys.isPlaying) {
			pSys.Stop ();
		}
	}
}
