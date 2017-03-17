/*
	SetRenderQueue.cs
 
	Sets the RenderQueue of an object's materials on Awake. This will instance
	the materials, so the script won't interfere with other renderers that
	reference the same materials.
*/

using UnityEngine;
public class SetRenderQueue : MonoBehaviour {
	public bool startHiding = false;
	private Shader[] originShader;
	private Renderer[] rs;
	public int renderQueue = 2002;

	public void SetOriginalShader(Renderer[] renderers){
		originShader = new Shader[rs.Length];
		for (int i = 0; i < rs.Length; i++) {
			originShader [i] = rs [i].material.shader;
		}
	}

	void Hides(int rqNum){
		for (int i = 0; i < rs.Length; i++){
			rs[i].material.renderQueue = rqNum;
		}
	}

	void Restore(){
		rs = GetComponentsInChildren<Renderer> ();
		SetOriginalShader (rs);
		if (startHiding) 
			Hides (renderQueue);
	}

	void Start(){
		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.material.renderQueue = renderQueue;
		}
	}

	void Update(){
		if (startHiding) 
			Hides (renderQueue);
		else
			Restore ();
	}
}
