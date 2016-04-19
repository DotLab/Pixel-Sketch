using UnityEngine;

using System.Collections.Generic;

public class Drawing {
	public readonly CanvasController UiController;

	Short2 size;
	Texture2D texture;

	Layer selectedLayer;
	readonly List<Layer> layers = new List<Layer>();

	public Drawing (CanvasController uiController, Short2 c) {
		UiController = uiController;
		size = c;

		RenderDrawing();
	}

	#region DrawShape

	public void SetColor (float x, float y, Color color) {
		if (selectedLayer == null) return;

		selectedLayer.SetColor(new Short2(x, y), color);
	}

	public void SetColor (Short2 point, Color color) {
		if (selectedLayer == null) return;

		selectedLayer.SetColor(point, color);
	}

	public void DrawPoint (Short2 point, Color color) {
		SetColor(point, color);
	}

	public void DrawLine (Short2 point1, Short2 point2, Color color) {
		int dx = point2.x - point1.x, dy = point2.y - point1.y;
		int s = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy));
		float xi = (float)dx / (float)s, yi = (float)dy / (float)s;
		float x = point1.x, y = point1.y;

		for (int i = 0; i <= s; i++) {
			SetColor(x + 0.5f, y + 0.5f, color);

			x += xi;
			y += yi;
		}
	}

	public void DrawRect (Short2 point1, Short2 point2, Color color) {
		int xmin = Mathf.Min(point1.x, point2.x), xmax = Mathf.Max(point1.x, point2.x);
		int ymin = Mathf.Min(point1.y, point2.y), ymax = Mathf.Max(point1.y, point2.y);

		for (int x = xmin; x <= xmax; x++) {
			SetColor(x, ymin, color);
			SetColor(x, ymax, color);
		}
		for (int y = ymin; y <= ymax; y++) {
			SetColor(xmin, y, color);
			SetColor(xmax, y, color);
		}
	}

	public void DrawCircle (Short2 center, int radius, Color color) {
		int x0 = center.x, y0 = center.y;
		int x = radius, y = 0;
		int decisionOver2 = 1 - x;   // Decision criterion divided by 2 evaluated at x=r, y=0

		while (y <= x) {
			SetColor(x + x0, y + y0, color); // Octant 1
			SetColor(y + x0, x + y0, color); // Octant 2
			SetColor(-x + x0, y + y0, color); // Octant 4
			SetColor(-y + x0, x + y0, color); // Octant 3
			SetColor(-x + x0, -y + y0, color); // Octant 5
			SetColor(-y + x0, -x + y0, color); // Octant 6
			SetColor(x + x0, -y + y0, color); // Octant 7
			SetColor(y + x0, -x + y0, color); // Octant 8
			y++;

			if (decisionOver2 <= 0) {
				decisionOver2 += 2 * y + 1;   // Change in decision criterion for y -> y+1
			} else {
				x--;
				decisionOver2 += 2 * (y - x) + 1;   // Change for y -> y+1, x -> x-1
			}
		}
	}

	public void DrawPoly (Short2[] points, Color color) {
		for (int i = 0; i < points.Length - 1; i++)
			DrawLine(points[i], points[i + 1], color);
		DrawLine(points[points.Length - 1], points[0], color);
	}

	public void ApplyDraw () {
		RenderDrawing();
		selectedLayer.RenderLayer();
	}

	#endregion

	#region LayerControl

	public void AddLayer (LayerController layerUi) {
		layers.Add(new Layer(layerUi, size));
	}

	public void DeleteLayer (LayerController layerUi) {
		var layer = FindLayer(layerUi);
		if (layer != null) layers.Remove(layer);
	}

	public void SelectLayer (LayerController layerUi) {
		var layer = FindLayer(layerUi);
		if (layer != null) selectedLayer = FindLayer(layerUi);
	}

	public Layer FindLayer (LayerController layerUi) {
		foreach (var layer in layers)
			if (layer.UiController == layerUi) return layer;

		return null;
	}

	#endregion

	#region Rendering

	public void ResizeDrawing (Short2 c) {
		if (c == size) return;
		size = c;

		foreach (var layer in layers)
			layer.ResizeLayer(size);

		RenderDrawing();
	}

	public void RenderDrawing () {
		if (texture == null || texture.width != size.x || texture.height != size.y) {
			if (texture != null) Object.DestroyImmediate(texture);
			texture = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false);
			texture.filterMode = FilterMode.Point;
			texture.hideFlags = HideFlags.DontSave;
			UiController.SetTexture(texture);
		}

		var pixels = new Color[size.x * size.y];
/*
		// O(height * width * layerCount)
		int i = 0;
		for (int y = 0; y < size.y; y++) {
			for (int x = 0; x < size.x; x++) {
				var key = new Short2(x, y);
				foreach (var layer in layers) {
					if (layer.HasColor(key)) {
						pixels[i] = layer.GetColor(key);
						break;
					}
				}
		
				i++;
			}
		}
*/

		// O(height * width * layerCount)
		var rendered = new Dictionary<Short2, bool>();
		foreach (var layer in layers) {
			if (layer.Hided) continue;

			foreach (var key in layer.Content.Keys) {
				if (IsIllegal(key)) continue;

				if (!rendered.ContainsKey(key)) {
					pixels[key.x + key.y * size.x] = layer.Content[key];
					rendered[key] = true;
				}
			}
		}

		texture.SetPixels(pixels);
		texture.Apply();
	}

	bool IsIllegal (Short2 c) {
		return c.x < 0 || c.x >= size.x || c.y < 0 || c.y >= size.y;
	}

	#endregion
}
