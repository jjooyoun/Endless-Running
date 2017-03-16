/*
	SetRenderQueue.cs
 
	Sets the RenderQueue of an object's materials on Awake. This will instance
	the materials, so the script won't interfere with other renderers that
	reference the same materials.
*/

using UnityEngine;
public class SetRenderQueue : MonoBehaviour {

	public int renderQueue = 2002;
	void Start(){
		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.material.renderQueue = renderQueue;
		}
	}
}
