using UnityEngine;
using UnityEngine.UI;

namespace HsvColorPicker {
	[AddComponentMenu("HsvColorPicker/SvColorSlider")]
	[RequireComponent(typeof(BoxSlider), typeof(RawImage))]
	public class SvColorSlider : ColorPickerComponent {
		BoxSlider slider;
		RawImage image;

		float h = -1;
		bool listen = true;

		public override void OnInit () {
			slider = GetComponent<BoxSlider>();
			image = GetComponent<RawImage>();

			slider.onValueChanged.AddListener(OnValueChanged);

			RegenerateSVTexture();
		}

		public override void OnDeinit () {
			slider.onValueChanged.RemoveListener(OnValueChanged);
			if (image.texture != null) DestroyImmediate(image.texture);
		}

		public override void OnEditorChanged () {
			image = GetComponent<RawImage>();

			RegenerateSVTexture();
		}

		public override void OnColorChanged () {
			if (Picker.H != h) {
				h = Picker.H;
				RegenerateSVTexture();
			}

			if (Picker.S != slider.normalizedValueX) {
				slider.normalizedValueX = Picker.S;
				listen = false;
			}

			if (Picker.V != slider.normalizedValueY) {
				slider.normalizedValueY = Picker.V;
				listen = false;
			}
		}

		void OnValueChanged (float saturation, float value) {
			if (listen) {
				Picker.SetColorParam(ColorParamType.S, saturation);
				Picker.SetColorParam(ColorParamType.V, value);
			}
			listen = true;
		}

		void RegenerateSVTexture () {
			var h = Picker.H;

			if (image.texture != null) DestroyImmediate(image.texture);

			var texture = new Texture2D(100, 100, TextureFormat.RGB24, false);
			texture.filterMode = FilterMode.Point;
			texture.hideFlags = HideFlags.DontSave;

			for (int s = 0; s < 100; s++) {
				var colors = new Color32[100];
				for (int v = 0; v < 100; v++) {
					colors[v] = ColorHelper.Hsv2Rgb(h, s / 100.0f, v / 100.0f);
				}
				texture.SetPixels32(s, 0, 1, 100, colors);
			}
			texture.Apply();

			image.texture = texture;
		}
	}
}