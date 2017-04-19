using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundControl : MonoBehaviour {
	private DistanceSystem distancesystem;
	private Renderer rend;

	private Mesh mesh;
	public float speed = 0.5f;

	Vector2 uvOffset = Vector2.zero;
	void Start()
	{
		distancesystem = FindObjectOfType<DistanceSystem> ();
		rend = GetComponent<Renderer>();
		mesh = GetComponent<MeshFilter>().mesh;
		speed = Mathf.Abs(Setting.gameSetting.objectSpeed/100.0f);

	}
    //Material texture offset rate

    //Offset the material texture at a constant rate
    void Update()
    {
		// if(Setting.gameSetting.isPaused)
		// 	return;
		// if (distancesystem.rellapsed > 20) {
		// 	speed = 0.7f;
		// } else if (distancesystem.rellapsed > 60) {
		// 	speed = 0.9f;
		// }
		//   else if (distancesystem.rellapsed > 90) {
		// 	speed = 1f;
		// }
		uvOffset += (new Vector2(0.0f, speed) * Time.deltaTime); //delta time must sum
		rend.material.mainTextureOffset = -uvOffset;
	}
}
