using UnityEngine;
using UnityEngine.UI;

namespace HsvColorPicker {
	[AddComponentMenu("HsvColorPicker/Color Graphic")]
	[RequireComponent(typeof(Graphic))]
	public class ColorGraphic : ColorPickerComponent {
		public bool Inverted;

		Graphic graphic;

	
		public override void OnInit () {
			graphic = GetComponent<Graphic>();
		}

		public override void OnColorChanged () {
			graphic.color = Inverted ? ColorHelper.InvertColor(Picker.CurrentColor) : Picker.CurrentColor;
		}
	}
}