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
}
