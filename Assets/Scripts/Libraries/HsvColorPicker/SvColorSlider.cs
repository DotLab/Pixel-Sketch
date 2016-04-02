using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxSlider), typeof(RawImage)), ExecuteInEditMode]
public class SvColorSlider : ColorPickerComponent {
	BoxSlider slider;
	RawImage image;

	float lastH = -1;
	bool listen = true;

	public override void Awake () {
		base.Awake();

		slider = GetComponent<BoxSlider>();
		image = GetComponent<RawImage>();

		slider.onValueChanged.AddListener(OnValueChanged);

		RegenerateSVTexture();
	}

	public override void OnDestroy () {
		base.OnDestroy();

		slider.onValueChanged.RemoveListener(OnValueChanged);
		if (image.texture != null) DestroyImmediate(image.texture);
	}

	public override void OnValidate () {
		base.OnValidate();
		if (Picker == null) return;

		image = GetComponent<RawImage>();
		RegenerateSVTexture();
	}

	void OnValueChanged (float saturation, float value) {
		if (listen) {
			Picker.AssignColorValue(ColorValueType.S, saturation);
			Picker.AssignColorValue(ColorValueType.V, value);
		}
		listen = true;
	}

	public override void OnColorChanged (Color color) {
		var h = Picker.H;
		var s = Picker.S;
		var v = Picker.V;

		if (lastH != h) {
			lastH = h;
			RegenerateSVTexture();
		}

		if (s != slider.normalizedValueX) {
			listen = false;
			slider.normalizedValueX = s;
		}

		if (v != slider.normalizedValueY) {
			listen = false;
			slider.normalizedValueY = v;
		}
	}

	void RegenerateSVTexture () {
		double h = Picker.H * 360;

		if (image.texture != null) DestroyImmediate(image.texture);

		var texture = new Texture2D(100, 100, TextureFormat.RGB24, false);
		texture.filterMode = FilterMode.Point;
		texture.hideFlags = HideFlags.DontSave;

		for (int s = 0; s < 100; s++) {
			var colors = new Color32[100];
			for (int v = 0; v < 100; v++) {
				colors[v] = ColorHelper.Hsv2Rgb(h, (float)s / 100, (float)v / 100);
			}
			texture.SetPixels32(s, 0, 1, 100, colors);
		}
		texture.Apply();

		image.texture = texture;
	}
}
