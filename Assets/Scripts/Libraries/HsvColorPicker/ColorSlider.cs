using UnityEngine;
using UnityEngine.UI;

namespace HsvColorPicker {
	[AddComponentMenu("HsvColorPicker/Color Slider")]
	[RequireComponent(typeof(Slider))]
	public class ColorSlider : ColorPickerComponent {
		public ColorParamType Type;

		Slider slider;
		bool listen = true;

		public override void OnInit () {
			slider = GetComponent<Slider>();
			slider.onValueChanged.AddListener(OnValueChanged);
		}

		public override void OnDeinit () {
			slider.onValueChanged.RemoveListener(OnValueChanged);
		}

		public override void OnColorChanged () {
			if (slider.normalizedValue != Picker.GetColorParam(Type)) {
				listen = false;
				slider.normalizedValue = Picker.GetColorParam(Type);
			}
		}

		void OnValueChanged (float newValue) {
			if (listen) Picker.SetColorParam(Type, newValue);

			listen = true;
		}
	}
}