using UnityEngine;

using System.Collections.Generic;

using HsvColorPicker;
using Uif;

public class ColorSwatchController : MonoBehaviour {
	public const int MaxCardCount = 28;

	public ColorPicker ColorPicker;
	public ColorTabController ColorTabController;
	public RectContentFitter ColorSwatchFitter;

	[Space]
	public Hidable DeleteIconHidable;
	public RectTransform DeleteIconRect;

	[Space]
	public GameObject ColorCardPrototype;

	bool colorSaved;
	readonly List<ColorCardController> colorCards = new List<ColorCardController>();


	void OnValidate () {
		if (DeleteIconRect == null)
			DeleteIconRect = DeleteIconHidable.GetComponent<RectTransform>();
	}

	void Awake () {
		ColorPicker.OnColorChangedEvent += OnPickerColorChanged;
	}

	void Start () {
		AddColor(Color.white);
		AddColor(Color.black);

		ColorSwatchFitter.Fit(colorCards.ToArray());
		ColorTabController.Fit();
	}

	void OnPickerColorChanged () {
		colorSaved = false;
	}

	public void AddPickerColor () {
		if (colorSaved || colorCards.Count >= MaxCardCount) return;
		colorSaved = true;

		foreach (var content in colorCards) {
			if (content.Color == ColorPicker.CurrentColor) return;
		}

		AddColor(ColorPicker.CurrentColor);

		ColorSwatchFitter.Fit(colorCards.ToArray());
		ColorTabController.Fit();
	}

	void AddColor (Color color) {
		if (colorCards.Count >= MaxCardCount) return;

		var card = Instantiate(ColorCardPrototype);
		var controller = card.GetComponent<ColorCardController>();
		controller.OnClickedEvent += OnCardClicked;
		controller.OnPressedEvent += OnCardPressed;
		controller.OnDragedEvent += OnCardDraged;
		controller.OnReleasedEvent += OnCardReleased;
		controller.Init(color, transform);

		colorCards.Add(controller);
		colorCards.Sort(new ColorCardController.ColorComparer());
	}

	public void OnCardClicked (ColorCardController colorCard) {
		ColorPicker.CurrentColor = colorCard.Color;
	}

	public void OnCardPressed (ColorCardController colorCard) {
		DeleteIconHidable.Show();
	}

	public void OnCardDraged (ColorCardController colorCard) {
		colorCard.DeleteFlag =
			Vector2.Distance(colorCard.CurrentPosition, DeleteIconRect.anchoredPosition) < 100;
	}

	public void OnCardReleased (ColorCardController colorCard) {
		DeleteIconHidable.Hide();

		if (colorCard.DeleteFlag) colorCards.Remove(colorCard);

		ColorSwatchFitter.Fit(colorCards.ToArray());
		ColorTabController.Fit();
	}
}
