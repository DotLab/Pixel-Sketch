using UnityEngine;

public abstract class ColorPickerComponent : MonoBehaviour {
	public ColorPicker Picker;

	public virtual void Awake () {
		Picker.OnColorChangedEvent += OnColorChanged;
	}

	public virtual void OnValidate () {
		if (Picker == null) Picker = GetComponentInParent<ColorPicker>();
	}

	public virtual void OnDestroy () {
		Picker.OnColorChangedEvent -= OnColorChanged;
	}

	public abstract void OnColorChanged (Color color);
}
