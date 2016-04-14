using System.Collections.Generic;

using UnityEngine;

public class LayerImage {
	public class UiComparer : IComparer<LayerImage> {
		public int Compare (LayerImage obj1, LayerImage obj2) {
			return obj1.UiLayer.Index.CompareTo(obj2.UiLayer.Index);
		}
	}

	public LayerController UiLayer;
	public Dictionary<Int2, Color> Content;

	public Color this [int x, int y] {
		get { return GetColor(x, y); }
		set { SetColor(x, y, value); }
	}

	public LayerImage () {
		Content = new Dictionary<Int2, Color>();
	}

	public void RenderLayer (int width, int height) {
		var pixels = new Color[width * height];
		foreach (var key in Content.Keys) {
			if (key.x < 0 || key.x >= width || key.y < 0 || key.y >= height) continue;

			pixels[key.x + key.y * width] = Content[key];
		}

		var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels(pixels);
		texture.Apply();

		UiLayer.SetThumbnail(texture);
	}

	public bool HasColor (int x, int y) {
		return Content.ContainsKey(new Int2(x, y));
	}

	public Color GetColor (int x, int y) {
		var key = new Int2(x, y);
		return Content.ContainsKey(key) ? Content[key] : Color.clear;
	}

	public void SetColor (int x, int y, Color color) {
		var key = new Int2(x, y);
		if (UiLayer.Locked && !Content.ContainsKey(key)) return;
		Content[key] = color;
	}
}

