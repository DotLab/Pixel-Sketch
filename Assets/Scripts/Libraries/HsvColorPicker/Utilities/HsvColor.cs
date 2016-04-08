using System;

// Describes a color in terms of
// Hue, Saturation, and Value (brightness)
public struct HsvColor {
	/// The Hue, ranges between 0 and 360
	public float H {
		get { return h; }
		set {
			h = value - (int)value; 
			if (h < 0) h += 1;
		}
	}

	/// The Saturation, ranges between 0 and 1
	public float S {
		get { return s; }
		set { s = value; }
	}

	// The Value (Brightness), ranges between 0 and 1
	public float V {
		get { return v; }
		set { v = value; }
	}

	float h;
	float s;
	float v;

	public HsvColor (float h, float s, float v) {
		this.h = h / 360;
		if (h > 1) h -= 1;
		if (h < 0) h += 1;

		this.s = s;
		this.v = v;
	}

	public override string ToString () {
		return "HSV(" + h.ToString("f3") + "," + s.ToString("f3") + "," + v.ToString("f3") + ")";
	}
}