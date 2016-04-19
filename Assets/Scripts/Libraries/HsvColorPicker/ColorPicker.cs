using UnityEngine;

namespace HsvColorPicker {
	[AddComponentMenu("HsvColorPicker/Color Picker")]
	public class ColorPicker : MonoBehaviour {
		public delegate void OnColorChanged ();

		public event OnColorChanged OnColorChangedEvent;

		public Color _currentColor;
		public HsvColor _currentHsvColor;

		public float _hue = 0;
		public float _saturation = 0;
		public float _value = 0;

		public float _red = 0;
		public float _green = 0;
		public float _blue = 0;

		float _alpha = 1;

		public Color CurrentColor {
			get { return _currentColor; }
			set {
				if (_currentColor == value) return;

				_red = value.r;
				_green = value.g;
				_blue = value.b;
				_alpha = value.a;

				OnRgbChanged();
			}
		}

		public HsvColor CurrentHsvColor {
			get { return _currentHsvColor; }
			set {
				if (_currentHsvColor == value) return;

				_hue = value.h;
				_saturation = value.s;
				_value = value.v;

				OnHsvChanged();
			}
		}

		public float R {
			get { return _red; }
			set {
				value = ColorHelper.Clump(value);
				if (_red == value) return;

				_red = value;

				OnRgbChanged();
			}
		}

		public float G {
			get { return _green; }
			set {
				value = ColorHelper.Clump(value);
				if (_green == value) return;

				_green = value;

				OnRgbChanged();
			}
		}

		public float B {
			get { return _blue; }
			set {
				value = ColorHelper.Clump(value);
				if (_blue == value) return;

				_blue = value;

				OnRgbChanged();
			}
		}

		public float A {
			get { return _alpha; }
			set {
				value = ColorHelper.Clump(value);
				if (_alpha == value) return;
				_alpha = value;

				OnRgbChanged();
			}
		}

		public float H {
			get { return _hue; }
			set {
				value = ColorHelper.ClumpHue(value);
				if (_hue == value) return;

				_hue = value;

				OnHsvChanged();
			}
		}

		public float S {
			get { return _saturation; }
			set {
				value = ColorHelper.Clump(value);
				if (_saturation == value) return;

				_saturation = value;

				OnHsvChanged();
			}
		}

		public float V {
			get { return _value; }
			set {
				value = ColorHelper.Clump(value);
				if (_value == value) return;

				_value = value;

				OnHsvChanged();
			}
		}

		void Start () {
			OnRgbChanged();
		}

		void OnRgbChanged () {
			_currentColor = new Color(_red, _green, _blue, _alpha);
			_currentHsvColor = ColorHelper.Rgb2Hsv(_red, _green, _blue);

			_hue = _currentHsvColor.h;
			_saturation = _currentHsvColor.s;
			_value = _currentHsvColor.v;

			if (OnColorChangedEvent != null) OnColorChangedEvent();
		}

		void OnHsvChanged () {
			_currentColor = ColorHelper.Hsv2Rgb(_hue, _saturation, _value);
			_currentColor.a = _alpha;
			_currentHsvColor = new HsvColor(_hue, _saturation, _value);

			_red = _currentColor.r;
			_green = _currentColor.g;
			_blue = _currentColor.b;

			if (OnColorChangedEvent != null) OnColorChangedEvent();
		}

		public void SetColorParam (ColorParamType type, float value) {
			switch (type) {
			case ColorParamType.R:
				R = value;
				break;
			case ColorParamType.G:
				G = value;
				break;
			case ColorParamType.B:
				B = value;
				break;
			case ColorParamType.A:
				A = value;
				break;
			case ColorParamType.H:
				H = value;
				break;
			case ColorParamType.S:
				S = value;
				break;
			case ColorParamType.V:
				V = value;
				break;
			default:
				throw new System.NotImplementedException();
			}
		}

		public float GetColorParam (ColorParamType type) {
			switch (type) {
			case ColorParamType.R:
				return R;
			case ColorParamType.G:
				return G;
			case ColorParamType.B:
				return B;
			case ColorParamType.A:
				return A;
			case ColorParamType.H:
				return H;
			case ColorParamType.S:
				return S;
			case ColorParamType.V:
				return V;
			default:
				throw new System.NotImplementedException();
			}
		}
	}
}