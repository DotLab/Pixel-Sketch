using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage)), ExecuteInEditMode]
public class ColorSliderBackground : ColorPickerComponent {
	public ColorValueType Type;
	public Slider.Direction direction;

	RawImage image;

	public override void Awake () {
		base.Awake();

		image = GetComponent<RawImage>();
		RegenerateTexture();
	}

	public override void OnDestroy () {
		base.OnDestroy();

		DestroyImmediate(image.texture); 
	}

	public override void OnColorChanged (Color color) {
		if (Type == ColorValueType.A || Type == ColorValueType.H) return;

		RegenerateTexture();
	}

	public override void OnValidate () {
		base.OnValidate();
		if (Picker == null) return;

		image = GetComponent<RawImage>();
		RegenerateTexture();
	}

	void RegenerateTexture () {
		Color32 baseColor = Picker.CurrentColor;

		float h = Picker.H;
		float s = Picker.S;
		float v = Picker.V;

		Texture2D texture;
		Color32[] colors;

		bool vertical = direction == Slider.Direction.BottomToTop || direction == Slider.Direction.TopToBottom;
		bool inverted = direction == Slider.Direction.TopToBottom || direction == Slider.Direction.RightToLeft;

		int size;
		switch (Type) {
		case ColorValueType.R:
		case ColorValueType.G:
		case ColorValueType.B:
		case ColorValueType.A:
			size = 255;
			break;
		case ColorValueType.H:
			size = 360;
			break;
		case ColorValueType.S:
		case ColorValueType.V:
			size = 100;
			break;
		default:
			throw new System.NotImplementedException("");
		}

		texture = vertical ? new Texture2D(1, size, TextureFormat.RGB24, false) : new Texture2D(size, 1, TextureFormat.RGB24, false);
		texture.filterMode = FilterMode.Point;
		texture.hideFlags = HideFlags.DontSave;

		colors = new Color32[size];

		switch (Type) {
		case ColorValueType.R:
			for (byte i = 0; i < size; i++)
				colors[inverted ? size - 1 - i : i] = new Color32(i, baseColor.g, baseColor.b, 255);
			break;
		case ColorValueType.G:
			for (byte i = 0; i < size; i++)
				colors[inverted ? size - 1 - i : i] = new Color32(baseColor.r, i, baseColor.b, 255);
			break;
		case ColorValueType.B:
			for (byte i = 0; i < size; i++)
				colors[inverted ? size - 1 - i : i] = new Color32(baseColor.r, baseColor.g, i, 255);
			break;
		case ColorValueType.A:
			for (byte i = 0; i < size; i++)
				colors[inverted ? size - 1 - i : i] = new Color32(i, i, i, 255);
			break;
		case ColorValueType.H:
			for (int i = 0; i < size; i++)
				colors[inverted ? size - 1 - i : i] = ColorHelper.Hsv2Rgb(i, 1, 1);
			break;
		case ColorValueType.S:
			for (int i = 0; i < size; i++)
				colors[inverted ? size - 1 - i : i] = ColorHelper.Hsv2Rgb(h * 360, (float)i / size, v);
			break;
		case ColorValueType.V:
			for (int i = 0; i < size; i++)
				colors[inverted ? size - 1 - i : i] = ColorHelper.Hsv2Rgb(h * 360, s, (float)i / size);
			break;
		default:
			throw new System.NotImplementedException("");
		}

		texture.SetPixels32(colors);
		texture.Apply();

		if (image.texture != null)
			DestroyImmediate(image.texture);
		image.texture = texture;

		switch (direction) {
		case Slider.Direction.BottomToTop:
		case Slider.Direction.TopToBottom:
			image.uvRect = new Rect(0, 0, 2, 1);
			break;
		case Slider.Direction.LeftToRight:
		case Slider.Direction.RightToLeft:
			image.uvRect = new Rect(0, 0, 1, 2);
			break;
		}
	}

}
