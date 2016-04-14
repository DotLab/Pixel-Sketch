using System;

public struct Int2 {
	public Int16 x;
	public Int16 y;

	public Int2 (float x, float y) {
		this.x = (Int16)x;
		this.y = (Int16)y;
	}

	public Int2 (int x, int y) {
		this.x = (Int16)x;
		this.y = (Int16)y;
	}

	public Int2 (int value) {
		x = (Int16)value;
		y = (Int16)value;
	}

	public override bool Equals (object obj) {
		if (obj is Int2) {
			var int2 = (Int2)obj;
			return int2.x == x && int2.y == y;
		}

		return false;
	}

	public override int GetHashCode () {
		return x << 16 + y;
	}

	public override string ToString () {
		return "(" + x + ", " + y + ")";
	}

	public static bool operator == (Int2 left, Int2 right) {
		return left.Equals(right);
	}

	public static bool operator != (Int2 left, Int2 right) {
		return !left.Equals(right);
	}

	public static Int2 operator + (Int2 left, Int2 right) {
		return new Int2(left.x + right.x, left.y + right.y);
	}

	public static Int2 operator - (Int2 left, Int2 right) {
		return new Int2(left.x - right.x, left.y - right.y);
	}

	public static Int2 operator * (Int2 left, int right) {
		return new Int2(left.x * right, left.y - right);
	}
}

