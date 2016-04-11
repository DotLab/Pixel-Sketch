using UnityEngine;

namespace HsvColorPicker {
	public abstract class ColorPickerComponent : MonoBehaviour {
		public ColorPicker Picker;

		void Awake () {
			Picker.OnColorChangedEvent += OnColorChanged;

			OnInit();
		}

		void OnValidate () {
			if (Picker == null) Picker = GetComponentInParent<ColorPicker>();

			OnEditorChanged();
		}

		void OnDestroy () {
			Picker.OnColorChangedEvent -= OnColorChanged;

			OnDeinit();
		}

		public virtual void OnInit () {
		}

		public virtual void OnDeinit () {
		}

		public virtual void OnEditorChanged () {
		}

		public abstract void OnColorChanged ();
	}
}