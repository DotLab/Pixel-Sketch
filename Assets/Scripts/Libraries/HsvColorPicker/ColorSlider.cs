using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays one of the color values of aColorPicker
/// </summary>
[RequireComponent(typeof(Slider))]
public class ColorSlider : ColorPickerComponent {
	public ColorValueType Type;

	Slider slider;
	bool listen = true;

	public override void Awake () {
		base.Awake();

		slider = GetComponent<Slider>();
		slider.onValueChanged.AddListener(OnValueChanged);
	}

	public override void OnDestroy () {
		base.Awake();

		slider.onValueChanged.RemoveListener(OnValueChanged);
	}

	public override void OnColorChanged (Color color) {
		if (Picker.H != slider.normalizedValue) {
			listen = false;
			slider.value = Picker.GetColorValue(Type);
		}
	}

	void OnValueChanged (float newValue) {
		if (listen) Picker.AssignColorValue(Type, newValue);
		listen = true;
	}
}
