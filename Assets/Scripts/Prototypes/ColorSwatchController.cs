using UnityEngine;

public class ColorSwatchController : MonoBehaviour {
	public const int MaxColorRectCount = 28;

	public ColorPicker ColorPicker;

	public ColorTabContentFitter TabFitter;
	public ColorSwatchContentFitter SwatchFitter;

	public GameObject ColorRectPrototype;

	void Start () {
		SwatchFitter.Contents.Clear();

//		for (int i = 0; i < 4; i++) {
//			AddColor(new Color(Random.value, Random.value, Random.value));
//		}
		AddColor(Color.white);
		AddColor(Color.black);

		SwatchFitter.Fit();
		TabFitter.Fit();
	}

	public void AddColor (Color color) {
		if (SwatchFitter.Contents.Count >= MaxColorRectCount) return;

		var colorRect = Instantiate(ColorRectPrototype);
		var colorRectController = colorRect.GetComponent<ColorRectController>();
		colorRectController.Trans.SetParent(transform, false);
		colorRectController.Trans.anchoredPosition = Vector2.zero;
		colorRectController.OnColorRectReleaseEvent += OnColorRectRelease;
		colorRectController.Init(color);
		SwatchFitter.Contents.Add(colorRectController);
		SwatchFitter.Contents.Sort(new ColorRectController.ColorComparer());
	}

	public void AutoAddColor () {
		if (SwatchFitter.Contents.Count >= MaxColorRectCount) return;

		foreach (var content in SwatchFitter.Contents) {
			if (content.Color == ColorPicker.CurrentColor) return;
		}

		AddColor(ColorPicker.CurrentColor);

		SwatchFitter.Fit();
		TabFitter.Fit();
	}

	public void OnColorRectRelease (ColorRectController colorRectController, bool deleted) {
		if (deleted) {
			SwatchFitter.Contents.Remove(colorRectController);
			SwatchFitter.Fit();
			TabFitter.Fit();
		} else {
			ColorPicker.CurrentColor = colorRectController.Color;
			SwatchFitter.Fit();
		}
	}
}
