using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColorImage : ColorPickerComponent {
	Image image;

	public override void Awake () {
		base.Awake();

		image = GetComponent<Image>();
	}

	public override void OnColorChanged (Color color) {
		image.color = color;
	}
}
