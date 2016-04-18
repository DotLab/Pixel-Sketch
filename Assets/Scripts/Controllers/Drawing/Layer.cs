using UnityEngine;

using System.Collections.Generic;

public class Layer {
	public class UiComparer : IComparer<Layer> {
		public int Compare (Layer obj1, Layer obj2) {
			return obj1.Index.CompareTo(obj2.Index);
		}
	}

	public int Index { get { return UiController.Index; } }

	public bool Hided { get { return UiController.Hided; } }

	public bool Locked { get { return UiController.Locked; } }

	public readonly LayerController UiController;
	public readonly Dictionary<Short2, Color> Content = new Dictionary<Short2, Color>();

	Short2 size;
	Texture2D texture;


	public Layer (LayerController uiController, Short2 c) {
		UiController = uiController;
		size = c;

		RenderLayer();
	}

	public void SetColor (Short2 c, Color color) {
		if (IsIllegal(c)) return;

		if (color.a == 0) ClearColor(c);
		else if (Locked && !HasColor(c)) return;
		else Content[c] = color;
	}

	public Color GetColor (Short2 c) {
		if (IsIllegal(c)) return Color.clear;

		return HasColor(c) ? Content[c] : Color.clear;
	}

	public bool HasColor (Short2 c) {
		return Content.ContainsKey(c);
	}

	public void ClearColor (Short2 c) {
		if (IsIllegal(c)) return;

		Content.Remove(c);
	}

	public void ResizeLayer (Short2 c) {
		if (c == size) return;
		size = c;

		RenderLayer();
	}

	public void RenderLayer () {
		if (texture == null || texture.width != size.x || texture.height != size.y) {
			if (texture != null) Object.DestroyImmediate(texture);
			texture = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false);
			texture.filterMode = FilterMode.Point;
			texture.hideFlags = HideFlags.DontSave;
			UiController.SetThumbnail(texture);
		}

		var pixels = new Color[size.x * size.y];
		foreach (var key in Content.Keys) {
			if (IsIllegal(key)) continue;

			pixels[key.x + key.y * size.x] = Content[key];
		}
			
		texture.SetPixels(pixels);
		texture.Apply();
	}

	bool IsIllegal (Short2 c) {
		return c.x < 0 || c.x >= size.x || c.y < 0 || c.y >= size.y;
	}
}