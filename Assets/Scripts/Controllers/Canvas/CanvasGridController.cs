using UnityEngine;

public class CanvasGridController : MonoBehaviour {
	public Color Color1 = Color.magenta;
	public Color Color2 = Color.magenta;

	public int GridSize = 16;
	public int Subdivision = 4;

	RectTransform trans;
	Material material;

	void Awake () {
		trans = GetComponent<RectTransform>();

		var shader = Shader.Find("Hidden/Internal-Colored");
		material = new Material(shader);
		material.hideFlags = HideFlags.HideAndDontSave;
	}

	void OnRenderObject () {
		var drawingSize = DrawingScheduler.DrawingSize;
		if (GridSize < 1 || GridSize > Mathf.Max(drawingSize.x, drawingSize.y))
			return;

		// Apply the line material
		material.SetPass(0);

		GL.PushMatrix();
		GL.MultMatrix(transform.localToWorldMatrix);

		GL.Begin(GL.LINES);

		GL.Color(Color1);
		var worldGridSize = (float)GridSize * (trans.rect.width / drawingSize.x);
		for (float x = trans.rect.xMin; x <= trans.rect.xMax; x += worldGridSize) {
			GL.Vertex(new Vector3(x, trans.rect.yMin));
			GL.Vertex(new Vector3(x, trans.rect.yMax));
		}

		for (float y = trans.rect.yMin; y <= trans.rect.yMax; y += worldGridSize) {
			GL.Vertex(new Vector3(trans.rect.xMin, y));
			GL.Vertex(new Vector3(trans.rect.xMax, y));
		}

		if (0 < Subdivision && Subdivision <= GridSize) {
			GL.Color(Color2);
			var worldSubGridSize = worldGridSize / Subdivision;
			for (float x = trans.rect.xMin; x < trans.rect.xMax; x += worldSubGridSize) {
				if (x % worldGridSize == 0) continue;

				GL.Vertex(new Vector3(x, trans.rect.yMin));
				GL.Vertex(new Vector3(x, trans.rect.yMax));
			}

			for (float y = trans.rect.yMin; y < trans.rect.yMax; y += worldSubGridSize) {
				if (y % worldGridSize == 0) continue;

				GL.Vertex(new Vector3(trans.rect.xMin, y));
				GL.Vertex(new Vector3(trans.rect.xMax, y));
			}
		}

		GL.End();

		GL.PopMatrix();
	}
}
