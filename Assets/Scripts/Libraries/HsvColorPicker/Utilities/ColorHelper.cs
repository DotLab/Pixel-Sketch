using UnityEngine;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HsvColorPicker {
	public static class ColorHelper {
		const string ColorHexRegex = "^#?(?:[0-9a-fA-F]{3,4}){1,2}$";

		public static float Clump (float value) {
			return value < 0 ? 0 : (value > 1 ? 1 : value);
		}

		public static float ClumpHue (float value) {
			return value < 0 || value >= 1 ? 0 : value;
		}

		public static Color InvertColor (Color color) {
			return new Color(1 - color.r, 1 - color.g, 1 - color.b);
		}

		public static HsvColor InvertColor (HsvColor color) {
			return new HsvColor(ClumpHue(1 - color.h), color.s, color.v);
		}

		public static HsvColor Rgb2Hsv (Color color) {
			return Rgb2Hsv(color.r, color.g, color.b);
		}

		// http://www.rapidtables.com/convert/color/rgb-to-hsv.htm
		public static HsvColor Rgb2Hsv (float r, float g, float b) {
			float h = 0, s, cmin, cmax, delta;

			cmin = Mathf.Min(Mathf.Min(r, b), g);
			cmax = Mathf.Max(Mathf.Max(r, b), g);
			delta = cmax - cmin;

			if (delta == 0)
				h = 6;
			else if (cmax == r)
				h = (b - g) / delta;
			else if (cmax == b)
				h = (g - r) / delta + 2;
			else if (cmax == g)
				h = (r - b) / delta + 4;
			h /= 6;

			if (h <= 0) h += 1f;
			h = 1f - h;

			if (cmax == 0) s = 0;
			else s = delta / cmax;

			return new HsvColor(h, s, cmax);
		}

		public static Color Hsv2Rgb (HsvColor color, float a = 1) {
			return Hsv2Rgb(color.h, color.s, color.v, a);
		}

		// http://www.rapidtables.com/convert/color/hsv-to-rgb.htm
		public static Color Hsv2Rgb (float h, float s, float v, float a = 1) {
			h *= 360;
			float r, g, b;

			var c = v * s;
			var x = c * (1 - Mathf.Abs((h / 60) % 2 - 1));
			var m = v - c;

			switch ((int)(h / 60)) {
			case 0: // 0 <= h < 60
				r = c;
				g = x;
				b = 0;
				break;
			case 1: // 60 <= h < 120
				r = x;
				g = c;
				b = 0;
				break;
			case 2: // 120 <= h < 180
				r = 0;
				g = c;
				b = x;
				break;
			case 3: // 180 <= h < 240
				r = 0;
				g = x;
				b = c;
				break;
			case 4: // 240 <= h < 300
				r = x;
				g = 0;
				b = c;
				break;
			case 5: // 300 <= h < 360
				r = c;
				g = 0;
				b = x;
				break;
			default:
				throw new ArithmeticException(h + " is not in the range of 0 <= h < 360");
			}

			r += m;
			g += m;
			b += m;

			return new Color(Clump(r), Clump(g), Clump(b), a);
		}

		public static string Color2Hex (Color32 color, bool displayAlpha = false) {
			if (displayAlpha) return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.r, color.g, color.b, color.a);
			else return string.Format("#{0:X2}{1:X2}{2:X2}", color.r, color.g, color.b);
		}

		public static bool Hex2Color (string hex, out Color32 color) {
			// Check if this is a valid hex string (# is optional)
			if (Regex.IsMatch(hex, ColorHexRegex)) {
				int startIndex = hex.StartsWith("#") ? 1 : 0;

				if (hex.Length == startIndex + 8) { //#RRGGBBAA
					color = new Color32(byte.Parse(hex.Substring(startIndex, 2), NumberStyles.AllowHexSpecifier),
						byte.Parse(hex.Substring(startIndex + 2, 2), NumberStyles.AllowHexSpecifier),
						byte.Parse(hex.Substring(startIndex + 4, 2), NumberStyles.AllowHexSpecifier),
						byte.Parse(hex.Substring(startIndex + 6, 2), NumberStyles.AllowHexSpecifier));
				} else if (hex.Length == startIndex + 6) {  //#RRGGBB
					color = new Color32(byte.Parse(hex.Substring(startIndex, 2), NumberStyles.AllowHexSpecifier),
						byte.Parse(hex.Substring(startIndex + 2, 2), NumberStyles.AllowHexSpecifier),
						byte.Parse(hex.Substring(startIndex + 4, 2), NumberStyles.AllowHexSpecifier),
						255);
				} else if (hex.Length == startIndex + 4) { //#RGBA
					color = new Color32(byte.Parse("" + hex[startIndex] + hex[startIndex], NumberStyles.AllowHexSpecifier),
						byte.Parse("" + hex[startIndex + 1] + hex[startIndex + 1], NumberStyles.AllowHexSpecifier),
						byte.Parse("" + hex[startIndex + 2] + hex[startIndex + 2], NumberStyles.AllowHexSpecifier),
						byte.Parse("" + hex[startIndex + 3] + hex[startIndex + 3], NumberStyles.AllowHexSpecifier));
				} else {  //#RGB
					color = new Color32(byte.Parse("" + hex[startIndex] + hex[startIndex], NumberStyles.AllowHexSpecifier),
						byte.Parse("" + hex[startIndex + 1] + hex[startIndex + 1], NumberStyles.AllowHexSpecifier),
						byte.Parse("" + hex[startIndex + 2] + hex[startIndex + 2], NumberStyles.AllowHexSpecifier),
						255);
				}
				return true;
			} else {
				color = new Color32();
				return false;
			}
		}
	}
}