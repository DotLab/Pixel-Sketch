using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class HexColorField : ColorPickerComponent {
	InputField inputFiled;

	public override void Awake () {
		base.Awake();

		inputFiled = GetComponent<InputField>();
		inputFiled.onEndEdit.AddListener(OnEndEdit);
	}

	public override void OnDestroy () {
		base.OnDestroy();

		inputFiled.onValueChanged.RemoveListener(OnEndEdit);
	}

	public override void OnColorChanged (Color color) {
		inputFiled.text = ColorHelper.Color2Hex(color);
	}

	void OnEndEdit (string newHex) {
		Color32 color;
		if (ColorHelper.Hex2Color(newHex, out color)) {
			Picker.CurrentColor = color;
		} else {
			DebugConsole.Log("Hex value is in the wrong format, valid formats are: #RGB, #RGBA, #RRGGBB and #RRGGBBAA (# is optional).", "red");
			inputFiled.text = ColorHelper.Color2Hex(Picker.CurrentColor);
		}
	}
}
