using UnityEngine;
using UnityEngine.UI;

namespace HsvColorPicker {
	[AddComponentMenu("HsvColorPicker/ColorSliderBackground")]
	[RequireComponent(typeof(RawImage))]
	public class ColorSliderBackground : ColorPickerComponent {
		public ColorParamType Type;
		public Slider.Direction direction;

		RawImage image;

		public override void OnInit () {
			image = GetComponent<RawImage>();
			RegenerateTexture();
		}

		public override void OnDeinit () {
			DestroyImmediate(image.texture); 
		}

		public override void OnColorChanged () {
			if (Type == ColorParamType.A || Type == ColorParamType.H) return;

			RegenerateTexture();
		}

		public override void OnEditorChanged () {
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
			case ColorParamType.R:
			case ColorParamType.G:
			case ColorParamType.B:
			case ColorParamType.A:
				size = 255;
				break;
			case ColorParamType.H:
				size = 360;
				break;
			case ColorParamType.S:
			case ColorParamType.V:
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
			case ColorParamType.R:
				for (byte i = 0; i < size; i++)
					colors[inverted ? size - 1 - i : i] = new Color32(i, baseColor.g, baseColor.b, 255);
				break;
			case ColorParamType.G:
				for (byte i = 0; i < size; i++)
					colors[inverted ? size - 1 - i : i] = new Color32(baseColor.r, i, baseColor.b, 255);
				break;
			case ColorParamType.B:
				for (byte i = 0; i < size; i++)
					colors[inverted ? size - 1 - i : i] = new Color32(baseColor.r, baseColor.g, i, 255);
				break;
			case ColorParamType.A:
				for (byte i = 0; i < size; i++)
					colors[inverted ? size - 1 - i : i] = new Color32(i, i, i, 255);
				break;
			case ColorParamType.H:
				for (int i = 0; i < size; i++)
					colors[inverted ? size - 1 - i : i] = ColorHelper.Hsv2Rgb((float)i / size, 1, 1);
				break;
			case ColorParamType.S:
				for (int i = 0; i < size; i++)
					colors[inverted ? size - 1 - i : i] = ColorHelper.Hsv2Rgb(h, (float)i / size, v);
				break;
			case ColorParamType.V:
				for (int i = 0; i < size; i++)
					colors[inverted ? size - 1 - i : i] = ColorHelper.Hsv2Rgb(h, s, (float)i / size);
				break;
			default:
				throw new System.NotImplementedException("");
			}

			texture.SetPixels32(colors);
			texture.Apply();

			if (image.texture != null) DestroyImmediate(image.texture);
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
}