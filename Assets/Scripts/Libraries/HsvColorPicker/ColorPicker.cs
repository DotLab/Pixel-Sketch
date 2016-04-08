using UnityEngine;

public class ColorPicker : MonoBehaviour {
	public float _hue = 0;
	public float _saturation = 0;
	public float _brightness = 0;

	public float _red = 0;
	public float _green = 0;
	public float _blue = 0;

	float _alpha = 1;

	public delegate void OnColorChanged (Color color);

	public event OnColorChanged OnColorChangedEvent;

	public Color CurrentColor {
		get { return new Color(_red, _green, _blue, _alpha); }
		set {
			if (CurrentColor == value) return;

			_red = value.r;
			_green = value.g;
			_blue = value.b;
			_alpha = value.a;

			BuildHsvColor();
		}
	}

	public float R {
		get { return _red; }
		set {
			if (_red == value) return;

			_red = value;

			BuildHsvColor();
		}
	}

	public float G {
		get { return _green; }
		set {
			if (_green == value) return;

			_green = value;

			BuildHsvColor();
		}
	}

	public float B {
		get { return _blue; }
		set {
			if (_blue == value) return;

			_blue = value;

			BuildHsvColor();
		}
	}

	public float A {
		get { return _alpha; }
		set { 
			_alpha = value;

			OnColorChangedEvent(CurrentColor);
		}
	}

	public float H {
		get { return _hue; }
		set {
			value = value - (int)value;
			if (_hue == value) return;

			_hue = value;

			BuildRgbColor();
		}
	}

	public float S {
		get { return _saturation; }
		set {
			if (_saturation == value) return;

			_saturation = value;

			BuildRgbColor();
		}
	}

	public float V {
		get { return _brightness; }
		set {
			if (_brightness == value) return;

			_brightness = value;

			BuildRgbColor();
		}
	}

	void Start () {
		OnColorChangedEvent(CurrentColor);
	}

	void BuildHsvColor () {
		var color = ColorHelper.Rgb2Hsv(_red, _green, _blue);

		_hue = color.H;
		_saturation = color.S;
		_brightness = color.V;

		OnColorChangedEvent(CurrentColor);
	}

	void BuildRgbColor () {
		var color = ColorHelper.Hsv2Rgb(_hue * 360, _saturation, _brightness);

		_red = color.r;
		_green = color.g;
		_blue = color.b;

		OnColorChangedEvent(CurrentColor);
	}

	public void AssignColorValue (ColorValueType type, float value) {
		switch (type) {
		case ColorValueType.R:
			R = value;
			break;
		case ColorValueType.G:
			G = value;
			break;
		case ColorValueType.B:
			B = value;
			break;
		case ColorValueType.A:
			A = value;
			break;
		case ColorValueType.H:
			H = value;
			break;
		case ColorValueType.S:
			S = value;
			break;
		case ColorValueType.V:
			V = value;
			break;
		default:
			throw new System.NotImplementedException();
		}
	}

	public float GetColorValue (ColorValueType type) {
		switch (type) {
		case ColorValueType.R:
			return R;
		case ColorValueType.G:
			return G;
		case ColorValueType.B:
			return B;
		case ColorValueType.A:
			return A;
		case ColorValueType.H:
			return H;
		case ColorValueType.S:
			return S;
		case ColorValueType.V:
			return V;
		default:
			throw new System.NotImplementedException();
		}
	}
}
