using UnityEngine;

using System.Collections.Generic;

public class Selection {
	public readonly Dictionary<Short2, bool> Area = new Dictionary<Short2, bool>();
	public readonly Dictionary<Short2, Color> Content = new Dictionary<Short2, Color>();

	public float Scale;
	public float Rotation;
	public Vector2 Pivotal;
	public Vector2 Position;


	public void ClearSelection () {
		Area.Clear();
	}

	public bool GetSelection (Short2 c) {
		return Area.ContainsKey(c);
	}

	public void SetSelection (Short2 c, bool value) {
		if (value) Area[c] = true;
		else if (Area.ContainsKey(c)) Area.Remove(c);
	}
}