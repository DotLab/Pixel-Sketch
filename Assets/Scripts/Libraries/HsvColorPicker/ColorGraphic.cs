using UnityEngine;
using UnityEngine.UI;

namespace HsvColorPicker {
	[AddComponentMenu("HsvColorPicker/ColorGraphic")]
	[RequireComponent(typeof(Graphic))]
	public class ColorGraphic : ColorPickerComponent {
		public bool Inverted;

		Graphic graphic;

	
		public override void OnInit () {
			graphic = GetComponent<Graphic>();
		}

		public override void OnColorChanged () {
			graphic.color = Inverted ? ColorHelper.Invert(Picker.CurrentColor) : Picker.CurrentColor;
		}
	}
}