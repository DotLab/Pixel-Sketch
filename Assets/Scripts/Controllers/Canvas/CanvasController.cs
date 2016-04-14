using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class CanvasController : MonoBehaviour {
	public RawImage CanvasImage;
	public RectTransform CanvasRect;
	public RectTransform BackgroundRect;

	[Space]
	public LayerImage SelectedImage;
	public List<LayerImage> LayerImages = new List<LayerImage>();

	[Space]
	public int Width = 64;
	public int Height = 64;

	RectTransform trans;


	void Awake () {
		trans = GetComponent<RectTransform>();
	}

	void Start () {
		SetCanvasSize();
	}

	public void Draw (Vector2 position) {
		if (SelectedImage == null) return;

		var curPixel = 
			new Int2(
				(position.x - CanvasRect.rect.min.x) / CanvasRect.rect.width * Width,
				(position.y - CanvasRect.rect.min.y) / CanvasRect.rect.height * Height);
		DebugConsole.Log(position);
		DebugConsole.Log(Rect.PointToNormalized(CanvasRect.rect, position));
		DebugConsole.Log(curPixel);

		SelectedImage.SetColor(curPixel.x, curPixel.y, color);
		SelectedImage.RenderLayer(Width, Height);
		RenderCanvas();
	}

	Color color;

	public void SetColor (Color color) {
		this.color = color;
	}

	public void OnCanvasSizeChanged (int width, int height) {
		Width = width;
		Height = height;

		foreach (var image in LayerImages) {
			image.RenderLayer(width, height);
		}
		RenderCanvas();
	}

	public void OnLayerAdded (LayerController uiLayer) {
		var newLayerImage = new LayerImage();
		newLayerImage.UiLayer = uiLayer;
		newLayerImage.RenderLayer(Width, Height);
		LayerImages.Add(newLayerImage);

		LayerImages.Sort(new LayerImage.UiComparer());
	}

	public void OnLayerDeleted (LayerController uiLayer) {
		for (int i = 0; i < LayerImages.Count; i++) {
			if (LayerImages[i].UiLayer == uiLayer) {
				if (SelectedImage == LayerImages[i]) SelectedImage = null;
				LayerImages.Remove(LayerImages[i]);
				return;
			}
		}
	}

	public void OnLayerSelected (LayerController uiLayer) {
		foreach (var image in LayerImages) {
			if (image.UiLayer == uiLayer) {
				SelectedImage = image;
				return;
			}
		}
	}

	public void OnLayerOrderChanged (LayerController[] uiLayers) {
		LayerImages.Sort(new LayerImage.UiComparer());
		RenderCanvas();
	}

	public void OnLayerHideStateChanged (LayerController uiLayer) {
		RenderCanvas();
	}

	public void RenderCanvas () {
		var pixels = new Color[Width * Height];
		// O(height * width * layerCount)
//		int i = 0;
//		for (int y = 0; y < Height; y++) {
//			for (int x = 0; x < Width; x++) {
//				foreach (var image in LayerImages) {
//					if (image.HasColor(x, y)) {
//						pixels[i] = image[x, y];
//						break;
//					}
//				}
//
//				i++;
//			}
//		}

		// O(pixelCountFromAllLayer)
		var rendedDic = new Dictionary<Int2, bool>();
		foreach (var image in LayerImages) {
			if (image.UiLayer.Hided) continue;

			foreach (var key in image.Content.Keys) {
				if (key.x < 0 || key.x >= Width || key.y < 0 || key.y >= Height) continue;

				if (!rendedDic.ContainsKey(key)) {
					pixels[key.x + key.y * Width] = image.Content[key];
					rendedDic[key] = true;
				}
			}
		}

		var texture = new Texture2D(Width, Height, TextureFormat.RGBA32, false);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels(pixels);
		texture.Apply();

		SetCanvasTexture(texture);
		SetCanvasSize();
	}

	public void SetCanvasTexture (Texture texture) {
		if (CanvasImage.texture != null) DestroyImmediate(CanvasImage.texture);
		CanvasImage.texture = texture;
	}

	[ContextMenu("SetCanvasSIze")]
	public void SetCanvasSize () {
		var aspect = (float)Width / Height;

		if (Camera.main.aspect >= aspect) {
			CanvasRect.sizeDelta = new Vector2(600 * aspect, 600);
		} else {
			var width = trans.rect.width;// * (600.0f / Screen.height);
			CanvasRect.sizeDelta = new Vector2(width, width / aspect);
		}

		BackgroundRect.sizeDelta = CanvasRect.sizeDelta;
	}
}
	