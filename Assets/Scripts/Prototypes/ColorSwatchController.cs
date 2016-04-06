using UnityEngine;
using System.Collections.Generic;

public class ColorSwatchController : MonoBehaviour {
	public ColorTabContentFitter ColorTabContentFitter;
	public ColorSwatchContentFitter ColorSwatchContentFitter;
	public RectTransform AddButton;
	public GameObject ColorRectPrototype;

	void Start () {
		ColorSwatchContentFitter.Contents = new List<ColorSwatchContentFitter.UiContent>();
		ColorSwatchContentFitter.Contents.Add(new ColorSwatchContentFitter.UiContent(AddButton));

		for (int i = 0; i < 10; i++) {
			AddColor(new Color(Random.value, Random.value, Random.value));
		}

		ColorTabContentFitter.Fit();
		ColorSwatchContentFitter.Fit();
	}

	public void AddColor (Color color) {
		var colorRect = Instantiate(ColorRectPrototype);
		var colorRectController = colorRect.GetComponent<ColorRectController>();
		colorRectController.SetColor(color);
		colorRectController.Trans.SetParent(transform, false);
		colorRectController.Trans.anchoredPosition = Vector2.zero;
		colorRectController.ColorSwatchController = this;
		colorRectController.UiContent = new ColorSwatchContentFitter.UiContent(colorRectController.Trans);
		ColorSwatchContentFitter.Contents.Add(colorRectController.UiContent);
	}

	public void OnAddColorClicked () {
		AddColor(new Color(Random.value, Random.value, Random.value));

		ColorTabContentFitter.Fit();
		ColorSwatchContentFitter.Fit();
	}

	public void OnColorRectPress (ColorRectController colorRect) {
		colorRect.UiContent.Controllable = false;
	}

	public void OnColorRectDrag (ColorRectController colorRect) {
		var originalIndex = ColorSwatchContentFitter.Contents.IndexOf(colorRect.UiContent);
		ColorSwatchContentFitter.Contents.Sort();
		if (ColorSwatchContentFitter.Contents.IndexOf(colorRect.UiContent) != originalIndex) {
			ColorSwatchContentFitter.Fit();
		}
	}

	public void OnColorRectRelease (ColorRectController colorRect) {
		colorRect.UiContent.Controllable = true;
		ColorSwatchContentFitter.Contents.Sort();
		ColorSwatchContentFitter.Fit();
	}
}
