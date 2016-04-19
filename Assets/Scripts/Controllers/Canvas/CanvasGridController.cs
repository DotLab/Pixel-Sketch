using UnityEngine;

public class CanvasGridController : MonoBehaviour {
	public Color Color1 = Color.magenta;
	public Color Color2 = Color.magenta;

	public int GridSize = 16;
	public int SubDivide = 4;

	Material lineMaterial;

	void Awake () {
		// Unity has a built-in shader that is useful for drawing
		// simple colored things.
		var shader = Shader.Find("Hidden/Internal-Colored");
		lineMaterial = new Material(shader);
		lineMaterial.hideFlags = HideFlags.HideAndDontSave;
		// Turn on alpha blending
		lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		// Turn backface culling off
		lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
		// Turn off depth writes
		lineMaterial.SetInt("_ZWrite", 0);
	}

	void OnRenderObject () {
		// Apply the line material
		lineMaterial.SetPass(0);

		GL.PushMatrix();
		GL.MultMatrix(transform.localToWorldMatrix);
		GL.Begin(GL.LINES);

		GL.Color(Color.green);

		// Push

		GL.End();
		GL.PopMatrix();
	}
}
