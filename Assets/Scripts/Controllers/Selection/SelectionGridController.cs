using UnityEngine;

using System.Collections.Generic;

public class SelectionGridController : MonoBehaviour {
	public Color Color = Color.green;

	public List<Vector3> Grid = new List<Vector3>();

	float scale;
	Quaternion rotation;
	Vector3 pivotal;
	Vector3 position;

	Material material;

	void Awake () {
		var shader = Shader.Find("Hidden/Internal-Colored");
		material = new Material(shader);
		material.hideFlags = HideFlags.HideAndDontSave;
	}

	void Update () {
		if (SelectionController.Selection.GridDirtyFlag) BuildGrid();
	}

	// The position of the vertex in the grid has
	// a coonstant shift of (0.5, 0.5) from the
	// vertex in the selection controller.
	// This is because when selection controller
	// render one pixel, the vertex on the screen
	// is actuallly the center of the grid.
	public void BuildGrid () {
		var selection = SelectionController.Selection;

		scale = selection.Scale;
		rotation = Quaternion.Euler(0, 0, selection.Rotation);
		pivotal = new Vector3(selection.Pivotal.x + 0.5f, selection.Pivotal.y + 0.5f) * DrawingScheduler.Coordinate2Ui;
		position = new Vector3(selection.Position.x + 0.5f, selection.Position.y + 0.5f) * DrawingScheduler.Coordinate2Ui;

		var factor = DrawingScheduler.Coordinate2Ui;

		Grid.Clear();

		foreach (var key in selection.Area.Keys) {
			var bottomLeft = new Vector2(key.x * factor, key.y * factor);
			var bottomRight = bottomLeft + Vector2.right * factor;
			var topLeft = bottomLeft + Vector2.up * factor;
			var topRight = bottomLeft + Vector2.one * factor;


			if (!selection.GetSelection(new Short2(key.x - 1, key.y))) {
				Grid.Add((Vector3)topLeft);
				Grid.Add((Vector3)bottomLeft);
			}

			if (!selection.GetSelection(new Short2(key.x + 1, key.y))) {
				Grid.Add((Vector3)topRight);
				Grid.Add((Vector3)bottomRight);
			}

			if (!selection.GetSelection(new Short2(key.x, key.y - 1))) {
				Grid.Add((Vector3)bottomLeft);
				Grid.Add((Vector3)bottomRight);
			}

			if (!selection.GetSelection(new Short2(key.x, key.y + 1))) {
				Grid.Add((Vector3)topLeft);
				Grid.Add((Vector3)topRight);
			}
		}
	}

	void OnRenderObject () {
		if (Grid.Count < 1 || (Grid.Count & 1) != 0) return;

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

	Vector3 TransformVertex (Vector3 v) {
		v -= pivotal;
		v *= scale;
		v = rotation * v;
		v += position;
		return v;
	}
}
