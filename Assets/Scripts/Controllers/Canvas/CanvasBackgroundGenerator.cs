using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class CanvasBackgroundGenerator : MonoBehaviour {
	public const float GridSize = 8;

	public static readonly Color Color1 = new Color32(255, 255, 255, 255);
	public static readonly Color Color2 = new Color32(206, 206, 206, 255);

	public RawImage Image;
	public RectTransform Canvas;

	void OnValidate () {
		if (Image == null) Image = GetComponent<RawImage>();
		if (Canvas == null) Canvas = (RectTransform)transform.parent.FindChild("Canvas");

		GenerateBackground();
	}

	void OnRectTransformDimensionsChange () {
		GenerateBackground();
	}

	[ContextMenu("Generate Background")]
	public void GenerateBackground () {
		var gridCountX = (int)(Canvas.rect.width / GridSize + 0.5f);
		var gridCountY = (int)(Canvas.rect.height / GridSize + 0.5f);

		if (Image.texture != null) {
			if (Image.texture.width == gridCountX && Image.texture.height == gridCountY) return;
			else DestroyImmediate(Image.texture);
		}
			
		var texture = new Texture2D(gridCountX, gridCountY, TextureFormat.RGB24, false);
		texture.filterMode = FilterMode.Point;
		for (int x = 0; x < gridCountX; x++) {
			for (int y = 0; y < gridCountY; y++) {
				texture.SetPixel(x, y, ((x + y) & 1) == 0 ? Color1 : Color2);
			}
		}
		texture.Apply();

		Image.texture = texture;
	}
}
