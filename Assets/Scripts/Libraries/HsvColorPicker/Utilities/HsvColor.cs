namespace HsvColorPicker {
	// Describes a color in terms of
	// Hue, Saturation, and Value (brightness)
	public struct HsvColor {
		public readonly float h;
		public readonly float s;
		public readonly float v;

		public HsvColor (float h, float s, float v) {
			this.h = ColorHelper.Clump(h, true);
			this.s = ColorHelper.Clump(s);
			this.v = ColorHelper.Clump(v);
		}

		public override bool Equals (object obj) {
			if (obj is HsvColor) {
				var hsvColor = (HsvColor)obj;
				return hsvColor.h == h && hsvColor.s == s && hsvColor.v == v;
			}
			return false;
		}

		public override int GetHashCode () {
			return (int)(h * 1000000 + s * 10000 + v * 100);
		}

		public override string ToString () {
			return "HSV(" + h.ToString("f3") + "," + s.ToString("f3") + "," + v.ToString("f3") + ")";
		}

		public static bool operator == (HsvColor left, HsvColor right) {
			return left.Equals(right);
		}

		public static bool operator != (HsvColor left, HsvColor right) {
			return !left.Equals(right);
		}
	}
}