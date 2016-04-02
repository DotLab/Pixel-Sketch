using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ColorLabel : ColorPickerComponent {
	public ColorValueType Type;

	public string Prefix = "R: ";
	public float MinValue = 0;
	public float MaxValue = 255;

	Text label;

	public override void Awake () {
		base.Awake();

		label = GetComponent<Text>();
	}

	public override void OnColorChanged (Color color) {
		var value = MinValue + (Picker.GetColorValue(Type) * (MaxValue - MinValue));
		label.text = Prefix + Mathf.FloorToInt(value);
	}
}
