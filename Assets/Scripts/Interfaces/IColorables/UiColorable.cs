using UnityEngine;
using UnityEngine.UI;

public class UiColorable : MonoBehaviour, IColorable {
	Graphic ui;

	void Awake () {
		ui = GetComponent<Graphic>();
	}

	public Color GetColor () {
		return ui.color;
	}

	public void SetColor (Color newColor) {
		ui.color = newColor;
	}
}
