using UnityEngine;
using UnityEngine.UI;

public class UiGroupColorable : MonoBehaviour, IColorable {
	public bool ExcludeSelf = true;
	public Color Color = Color.white;

	Graphic[] uis;

	void Awake () {
		uis = GetComponentsInChildren<Graphic>();

		SetColor(Color);
	}

	public Color GetColor () {
		return Color;
	}

	public void SetColor (Color newColor) {
		foreach (var ui in uis) {
			if (ExcludeSelf && ui.gameObject == gameObject) continue;
			ui.color = newColor;
		}

		Color = newColor;
	}
}
