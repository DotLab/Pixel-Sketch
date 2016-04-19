using UnityEngine;

using System.Collections.Generic;

public class Selection {
	public enum SelectMode {
		New,
		Add,
		Remove
	}

	public readonly Dictionary<Short2, bool> Content = new Dictionary<Short2, bool>();

	public void SelectPoint (Short2 point, SelectMode mode) {
		if (mode == SelectMode.New) Content.Clear();

		switch (mode) {
		case SelectMode.New:
		case SelectMode.Add:
			Content[point] = true;
			break;
		case SelectMode.Remove:
			if (Content.ContainsKey(point)) Content.Remove(point);
			break;
		}
	}

	public void SelectLine () {

	}

	public void SelectRect (Short2 point1, Short2 point2, SelectMode mode) {
		
	}
}