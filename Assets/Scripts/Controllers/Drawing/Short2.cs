using System;

[Serializable]
public struct Short2 {
	public const Int16 MinValue = Int16.MinValue;
	public const Int16 MaxValue = Int16.MaxValue;

	public readonly Int16 x;
	public readonly Int16 y;

	public Short2 (Int16 value) {
		x = value;
		y = value;
	}

	public Short2 (Int16 x, Int16 y) {
		this.x = x;
		this.y = y;
	}

	public Short2 (int value) {
		x = (Int16)value;
		y = (Int16)value;
	}

	public Short2 (int x, int y) {
		this.x = (Int16)x;
		this.y = (Int16)y;
	}

	public Short2 (float x, float y) {
		this.x = (Int16)x;
		this.y = (Int16)y;
	}

	public override bool Equals (object other) {
		if (other is Short2) {
			var s = (Short2)other;
			return s.x == x && s.y == y;
		}

		return false;
	}

	public override int GetHashCode () {
		return x << 16 + y;
	}

	public override string ToString () {
		return "(" + x + ", " + y + ")";
	}

	public static Short2 Min (Short2 left, Short2 right) {
		return new Short2(
			left.x < right.x ? left.x : right.x,
			left.y < right.y ? left.y : right.y);
	}

	public static Short2 Max (Short2 left, Short2 right) {
		return new Short2(
			left.x > right.x ? left.x : right.x,
			left.y > right.y ? left.y : right.y);
	}

	public static bool operator == (Short2 left, Short2 right) {
		return left.Equals(right);
	}

	public static bool operator != (Short2 left, Short2 right) {
		return !left.Equals(right);
	}

	public static Short2 operator + (Short2 left, Short2 right) {
		return new Short2(left.x + right.x, left.y + right.y);
	}

	public static Short2 operator - (Short2 left, Short2 right) {
		return new Short2(left.x - right.x, left.y - right.y);
	}

	public static Short2 operator * (Short2 left, int right) {
		return new Short2(left.x * right, left.y - right);
	}
}

