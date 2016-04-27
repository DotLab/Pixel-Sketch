using UnityEngine;

using System.Collections.Generic;

public class Layer {
	public class UiComparer : IComparer<Layer> {
		public int Compare (Layer obj1, Layer obj2) {
			return obj1.Index.CompareTo(obj2.Index);
		}
	}

	public int Index { get { return layerUi.Index; } }

	public bool Hided { get { return layerUi.Hided; } }

	public bool Locked { get { return layerUi.Locked; } }

	readonly LayerController layerUi;
	readonly Dictionary<Short2, Color> content = new Dictionary<Short2, Color>();

	Short2 size;
	Texture2D texture;


	public Layer (Short2 size, LayerController layerUi) {
		this.size = size;
		this.layerUi = layerUi;

		RenderLayer();
	}

	#region Color

	public ICollection<Short2> GetColorKeys () {
		return content.Keys;
	}

	public bool HasColor (Short2 c) {
		return content.ContainsKey(c);
	}

	public bool SetColor (Short2 c, Color color) {
		if (IsIllegal(c)) return false;

		if (Locked && !HasColor(c)) return false; // Transparency Lock

		if (color.a == 0) return ClearColor(c); // Set Clear

		if (GetColor(c) == color) return false; // No Change

		content[c] = color;
		return true;
	}

	public Color GetColor (Short2 c) {
		if (IsIllegal(c)) return Color.clear;

		return HasColor(c) ? content[c] : Color.clear;
	}

	public bool ClearColor (Short2 c) {
		if (IsIllegal(c)) return false;

		if (HasColor(c)) {
			content.Remove(c);
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

	#region Selection

	public void AddToSelection (Short2 c, Selection selection) {
		if (IsIllegal(c)) return;

		FloodSetSelection(c, GetColor(c), true, selection);
	
		selection.CalcPivotal();
	}

	public void SubFromSelection (Short2 c, Selection selection) {
		if (IsIllegal(c)) return;

		FloodSetSelection(c, GetColor(c), false, selection);

		selection.CalcPivotal();
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
		if (selection.Area.Count == 0) {
			foreach (var key in content.Keys)
				selection.SetSelection(key);
			selection.CalcPivotal();
		}

		var keys = new List<Short2>();
		foreach (var key in selection.Area.Keys) {
			if (HasColor(key)) selection.Content[key] = GetColor(key);
			else keys.Add(key);
		
			ClearColor(key);
		}

		foreach (var key in keys) {
			selection.SetSelection(key, false);
		}
		selection.CalcPivotal();
	}

	public void ApplyTransform (Selection selection) {
		selection.CalcExtent();
		var min = selection.MinC;
		var max = selection.MaxC;

		var rotation = Quaternion.Euler(0, 0, -selection.Rotation);
		int i = 0;
		for (int y = 0; y < size.y; y++) {
			for (int x = 0; x < size.y; x++) {
				var coordinate = new Vector3(x, y);
				coordinate -= selection.Position;
				coordinate = rotation * coordinate;
				coordinate /= selection.Scale;
				coordinate += selection.Pivotal;
				var originalCoordinate = new Short2(coordinate.x + 0.5f, coordinate.y + 0.5f);

				if (selection.Content.ContainsKey(originalCoordinate) && selection.Content[originalCoordinate].a != 0)
					SetColor(new Short2(x, y), selection.Content[originalCoordinate]);

				i++;
			}
		}

		selection.ResetSelection();
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
			layerUi.SetThumbnail(texture);
		}

		var pixels = new Color[size.x * size.y];
		foreach (var key in content.Keys) {
			if (IsIllegal(key)) continue;

			pixels[key.x + key.y * size.x] = content[key];
		}
			
		texture.SetPixels(pixels);
		texture.Apply();
	}

	#endregion

	bool IsIllegal (Short2 c) {
		return c.x < 0 || c.x >= size.x || c.y < 0 || c.y >= size.y;
	}
}