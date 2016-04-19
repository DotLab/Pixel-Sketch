using UnityEngine;
using UnityEngine.UI;

namespace HsvColorPicker {
	[AddComponentMenu("HsvColorPicker/Hex Color Field")]
	[RequireComponent(typeof(InputField))]
	public class HexColorField : ColorPickerComponent {
		public bool DisplayAlpha = true;

		InputField inputFiled;


		public override void OnInit () {
			inputFiled = GetComponent<InputField>();
			inputFiled.onEndEdit.AddListener(OnEndEdit);
		}

		public override void OnDeinit () {
			inputFiled.onValueChanged.RemoveListener(OnEndEdit);
		}

		public override void OnColorChanged () {
			inputFiled.text = ColorHelper.Color2Hex(Picker.CurrentColor, DisplayAlpha);
		}

		void OnEndEdit (string newHex) {
			Color32 color;
			if (ColorHelper.Hex2Color(newHex, out color)) {
				Picker.CurrentColor = color;
			} else {
				Debug.LogError("Hex value is in the wrong format, valid formats are: #RGB, #RGBA, #RRGGBB and #RRGGBBAA (# is optional).");
				inputFiled.text = ColorHelper.Color2Hex(Picker.CurrentColor);
			}
		}
	}
}