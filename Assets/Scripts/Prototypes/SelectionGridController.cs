using UnityEngine;

using System.Collections.Generic;

public class SelectionGridController : MonoBehaviour {
	public Color Color = Color.green;

	public float Scale;
	public float Rotation;
	public Vector3 Pivotal;
	public Vector3 Position;

	public Short2 size;

	public Selection Selection = new Selection();
	public List<Vector3> Grid = new List<Vector3>();

	RectTransform trans;
	Material material;

	void Awake () {
		trans = GetComponent<RectTransform>();

		var shader = Shader.Find("Hidden/Internal-Colored");
		material = new Material(shader);
		material.hideFlags = HideFlags.HideAndDontSave;
	}

	void Start () {
//		for (int x = 1; x <= 16; x++) {
//			for (int y = 1; y <= 16; y++) {
//				Selection.SetSelection(new Short2(x, y), true);
//			}
//		}
//		BuildGrid();
	}

	public void BuildGrid () {
		var drawingSize = DrawingSceneScheduler.Instance.DrawingSize;
		var factor = 600.0f / drawingSize.y;

		Grid.Clear();

		foreach (var key in Selection.Area.Keys) {
			var bottomLeft = new Vector2(key.x * factor, key.y * factor);
			var bottomRight = bottomLeft + Vector2.right * factor;
			var topLeft = bottomLeft + Vector2.up * factor;
			var topRight = bottomLeft + Vector2.one * factor;


			if (!Selection.GetSelection(new Short2(key.x - 1, key.y))) {
				Grid.Add((Vector3)topLeft);
				Grid.Add((Vector3)bottomLeft);
			}

			if (!Selection.GetSelection(new Short2(key.x + 1, key.y))) {
				Grid.Add((Vector3)topRight);
				Grid.Add((Vector3)bottomRight);
			}

			if (!Selection.GetSelection(new Short2(key.x, key.y - 1))) {
				Grid.Add((Vector3)bottomLeft);
				Grid.Add((Vector3)bottomRight);
			}

			if (!Selection.GetSelection(new Short2(key.x, key.y + 1))) {
				Grid.Add((Vector3)topLeft);
				Grid.Add((Vector3)topRight);
			}
		}
	}

	void OnRenderObject () {
		if ((Grid.Count & 1) != 0) return;

		// Apply the line material
		material.SetPass(0);

		GL.PushMatrix();
		GL.MultMatrix(transform.localToWorldMatrix);

		GL.Begin(GL.LINES);

		GL.Color(Color);
		for (int i = 0; i < Grid.Count - 1; i += 2) {
			GL.Vertex(TransformVertex(Grid[i]));
			GL.Vertex(TransformVertex(Grid[i + 1]));
		}

		GL.End();

		GL.PopMatrix();
	}

	// The position of the vertex in the grid has
	// a coonstant shift of (0.5, 0.5) from the
	// vertex in the selection controller.
	// This is because when selection controller
	// render one pixel, the vertex of the screen
	// is actuallly the center of the grid.
	Vector3 TransformVertex (Vector3 v) {
		var pivotal = new Vector3(Pivotal.x + 0.5f, Pivotal.y + 0.5f) * (600.0f / size.y);
		var rotation = Quaternion.Euler(0, 0, Rotation);
		var position = new Vector3(Position.x + 0.5f, Position.y + 0.5f) * (600.0f / size.y);

		v -= pivotal;
		v *= Scale;
		v = rotation * v;
		v += position;
		return v;
	}
}
