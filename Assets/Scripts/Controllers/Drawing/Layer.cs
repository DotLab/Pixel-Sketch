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

	#region Draw

	public bool HasColor (Short2 c) {
		return Content.ContainsKey(c);
	}

	public bool SetColor (Short2 c, Color color) {
		if (IsIllegal(c)) return false;

		if (Locked && !HasColor(c)) return false; // Transparency Lock

		if (color.a == 0) return ClearColor(c); // Set Clear

		if (GetColor(c) == color) return false; // No Change

		Content[c] = color;
		return true;
	}

	public Color GetColor (Short2 c) {
		if (IsIllegal(c)) return Color.clear;

		return HasColor(c) ? Content[c] : Color.clear;
	}

	public bool ClearColor (Short2 c) {
		if (IsIllegal(c)) return false;

		if (HasColor(c)) {
			Content.Remove(c);
			return true;
		}
		return false;
	}

	public bool FillColor (Short2 c, Color color) {
		if (IsIllegal(c)) return false;

		if (Locked && !HasColor(c)) return false; // Transparency Lock

		if (GetColor(c) == color) return false; // No Need

		FloodFill(c, GetColor(c), color);
		return true;
	}

	void FloodFill (Short2 c, Color src, Color dst) {
		if (IsIllegal(c) || GetColor(c) != src || GetColor(c) == dst) return;
		SetColor(c, dst);

		FloodFill(new Short2(c.x, c.y + 1), src, dst);
		FloodFill(new Short2(c.x, c.y - 1), src, dst);
		FloodFill(new Short2(c.x - 1, c.y), src, dst);
		FloodFill(new Short2(c.x + 1, c.y), src, dst);
	}

	#endregion

	#region Select

	public void AddToSelection (Short2 c, Selection selection) {
		if (IsIllegal(c)) return;

		FloodSetSelection(c, GetColor(c), true, selection);
	}

	public void SubFromSelection (Short2 c, Selection selection) {
		if (IsIllegal(c)) return;

		FloodSetSelection(c, GetColor(c), false, selection);
	}

	void FloodSetSelection (Short2 c, Color src, bool value, Selection selection) {
		if (IsIllegal(c) || GetColor(c) != src || selection.GetSelection(c) == value) return;
		selection.SetSelection(c, value);

		FloodSetSelection(new Short2(c.x, c.y + 1), src, value, selection);
		FloodSetSelection(new Short2(c.x, c.y - 1), src, value, selection);
		FloodSetSelection(new Short2(c.x - 1, c.y), src, value, selection);
		FloodSetSelection(new Short2(c.x + 1, c.y), src, value, selection);
	}

	public void ApplySelection (Selection selection) {
		foreach (var key in selection.Area.Keys) {
			
		}
	}

	public void ApplyTransform (Selection selection) {

	}

	#endregion

	#region Render

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

	#endregion

	bool IsIllegal (Short2 c) {
		return c.x < 0 || c.x >= size.x || c.y < 0 || c.y >= size.y;
	}
}