using UnityEngine;
using UnityEngine.UI;

namespace HsvColorPicker {
	[AddComponentMenu("HsvColorPicker/ColorLabel")]
	[RequireComponent(typeof(Text))]
	public class ColorLabel : ColorPickerComponent {
		public ColorParamType Type;

		public string Prefix = "";
		public float MinValue = 0;
		public float MaxValue = 255;

		Text label;

		public override void OnInit () {
			label = GetComponent<Text>();
		}

		public override void OnColorChanged () {
			var value = MinValue + (Picker.GetColorParam(Type) * (MaxValue - MinValue));
			label.text = Prefix + Mathf.FloorToInt(value);
		}
	}
}