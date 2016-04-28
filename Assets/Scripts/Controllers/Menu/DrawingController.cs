using UnityEngine;
using UnityEngine.UI;

using Uif;

public class DrawingController : MonoBehaviour {
	public delegate void OnDrawingClicked (DrawingController drawing);

	public event OnDrawingClicked OnClickedEvent;

	public RawImage Thumbnail;
	public Hidable SelectMask;

	public bool Selected {
		set {
			if (value) SelectMask.Show();
			else SelectMask.Hide();
		}
	}

	public DrawingFile DrawingFile;

	Texture2D texture;

	public void Init (DrawingFile file) {
		DrawingFile = file;

		RenderDrawing();
	}

	public void OnClicked () {
		if (OnClickedEvent != null) OnClickedEvent(this);
	}

	void RenderDrawing () {
		if (texture == null) {
			texture = new Texture2D(DrawingFile.Size.x, DrawingFile.Size.y, TextureFormat.RGBA32, false);
			texture.filterMode = FilterMode.Point;
			texture.hideFlags = HideFlags.DontSave;
			Thumbnail.texture = texture;
		}
			
		var pixels = new Color[DrawingFile.Size.x * DrawingFile.Size.y];
		// O(height * width * layerCount)
		var renderDict = new System.Collections.Generic.Dictionary<Short2, bool>();
		foreach (var layer in DrawingFile.Layers) {
			if (layer.Hided) continue;

//			foreach (var pair in layer.Content) {
//				if (IsIllegal(pair.k)) continue;
//
//				if (!renderDict.ContainsKey(pair.k)) {
//					pixels[pair.k.x + pair.k.y * DrawingFile.Size.x] = pair.c;
//					renderDict[pair.k] = true;
//				}
//			}


			foreach (var key in layer.Content.Keys) {
				if (IsIllegal(key)) continue;
			
				if (!renderDict.ContainsKey(key)) {
					pixels[key.x + key.y * DrawingFile.Size.x] = layer.Content[key];
					renderDict[key] = true;
				}
			}
		}

		texture.SetPixels(pixels);
		texture.Apply();
	}

	bool IsIllegal (Short2 c) {
		return c.x < 0 || c.x >= DrawingFile.Size.x || c.y < 0 || c.y >= DrawingFile.Size.y;
	}
}
