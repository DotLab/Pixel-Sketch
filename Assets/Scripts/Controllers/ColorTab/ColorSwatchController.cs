using UnityEngine;

using HsvColorPicker;
using Uif;

public class ColorSwatchController : MonoBehaviour {
	public const int MaxCardCount = 28;

	public ColorPicker ColorPicker;
	public ColorTabController ColorTabFitter;
	public ColorSwatchContentFitter ColorSwatchFitter;

	[Space]
	public GameObject ColorCardPrototype;

	[Space]
	public Hidable DeleteArea;
	RectTransform deleteAreaRect;

	void Awake () {
		deleteAreaRect = DeleteArea.GetComponent<RectTransform>();
	}

	void Start () {
		ColorSwatchFitter.ColorCards.Clear();

		AddColor(Color.white);
		AddColor(Color.black);

		ColorSwatchFitter.Fit();
		ColorTabFitter.Fit();
	}

	public void AddColor () {
		if (ColorSwatchFitter.ColorCards.Count >= MaxCardCount) return;

		foreach (var content in ColorSwatchFitter.ColorCards) {
			if (content.Color == ColorPicker.CurrentColor) return;
		}

		AddColor(ColorPicker.CurrentColor);

		ColorSwatchFitter.Fit();
		ColorTabFitter.Fit();
	}

	void AddColor (Color color) {
		if (ColorSwatchFitter.ColorCards.Count >= MaxCardCount) return;

		var colorRect = Instantiate(ColorCardPrototype);
		var colorRectController = colorRect.GetComponent<ColorCardController>();
		colorRectController.OnColorCardClickedEvent += OnColorCardClicked;
		colorRectController.OnColorCardPressedEvent += OnColorCardPressed;
		colorRectController.OnColorCardDragedEvent += OnColorCardDraged;
		colorRectController.OnColorCardReleasedEvent += OnColorCardReleased;
		colorRectController.Init(color, transform);

		ColorSwatchFitter.ColorCards.Add(colorRectController);
		ColorSwatchFitter.ColorCards.Sort(new ColorCardController.ColorComparer());
	}

	public void OnColorCardClicked (ColorCardController colorCard) {
		if (!colorCard.DeleteFlag && colorCard.Controllable)
			ColorPicker.CurrentColor = colorCard.Color;
	}

	public void OnColorCardPressed (ColorCardController colorCard) {
		DeleteArea.Show();
	}

	public void OnColorCardDraged (ColorCardController colorCard) {
		if (IsDeletable(colorCard)) colorCard.SetDeleteFlag();
		else colorCard.ResetDeleteFlag();
	}

	public void OnColorCardReleased (ColorCardController colorCard) {
		if (IsDeletable(colorCard)) colorCard.SetDeleteFlag();
		else colorCard.ResetDeleteFlag();

		DeleteArea.Hide();

		if (colorCard.DeleteFlag) {
			ColorSwatchFitter.ColorCards.Remove(colorCard);
			colorCard.Deinit();
		}

		ColorSwatchFitter.Fit();
		ColorTabFitter.Fit();
	}

	bool IsDeletable (RectContent colorCard) {
		return Vector2.Distance(colorCard.CurrentPosition, deleteAreaRect.anchoredPosition) < 100;
	}
}
