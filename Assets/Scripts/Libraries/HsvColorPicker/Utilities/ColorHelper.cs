using UnityEngine;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

public static class ColorHelper {
	const string ColorHexRegex = "^#?(?:[0-9a-fA-F]{3,4}){1,2}$";

	// http://www.rapidtables.com/convert/color/rgb-to-hsv.htm
	public static HsvColor Rgb2Hsv (double r, double b, double g) {
		double h = 0, s, cmin, cmax, delta;

		cmin = Math.Min(Math.Min(r, g), b);
		cmax = Math.Max(Math.Max(r, g), b);
		delta = cmax - cmin;

		if (delta == 0)
			h = 6;
		else if (cmax == r)
			h = (g - b) / delta;
		else if (cmax == g)
			h = (b - r) / delta + 2;
		else if (cmax == b)
			h = (r - g) / delta + 4;
		h *= 60;
		if (h <= 0.0) h += 360;
		h = 360 - h;

		if (cmax == 0) s = 0;
		else s = delta / cmax;

		return new HsvColor((float)h, (float)s, (float)cmax);
	}

	// http://www.rapidtables.com/convert/color/hsv-to-rgb.htm
	public static Color Hsv2Rgb (double h, double s, double v) {
		double r, g, b;

		var c = v * s;
		var x = c * (1 - Math.Abs((h / 60) % 2 - 1));
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

		return new Color((float)r, (float)g, (float)b, 1);
	}
	/*
	public static HsvColor Rgb2Hsv (double r, double b, double g) {
		float h, s, v;
		Color.RGBToHSV(new Color((float)r, (float)b, (float)g), out h, out s, out v);
		return new HsvColor(h, s, v);
	}

	public static Color Hsv2Rgb (double h, double s, double v) {
		return Color.HSVToRGB((float)h, (float)s, (float)v);
	}
	*/
	public static string Color2Hex (Color32 color) {
		return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.r, color.g, color.b, color.a);
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
