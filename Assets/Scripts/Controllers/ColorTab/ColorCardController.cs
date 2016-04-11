using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections.Generic;

using HsvColorPicker;
using Uif;

public class ColorCardController : RectContent, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public class ColorComparer : IComparer<ColorCardController> {
		public int Compare (ColorCardController obj1, ColorCardController obj2) {
			var color1 = ColorHelper.Rgb2Hsv(obj1.Color.r, obj1.Color.b, obj1.Color.g);
			var color2 = ColorHelper.Rgb2Hsv(obj2.Color.r, obj2.Color.b, obj2.Color.g);

			if (color1.h.CompareTo(color2.h) != 0) return color1.h.CompareTo(color2.h);
			else return (color1.v).CompareTo(color2.v);
		}
	}

	public delegate void OnColorCardStateChanged (ColorCardController colorCard);

	public event OnColorCardStateChanged OnColorCardClickedEvent;
	public event OnColorCardStateChanged OnColorCardPressedEvent;
	public event OnColorCardStateChanged OnColorCardDragedEvent;
	public event OnColorCardStateChanged OnColorCardReleasedEvent;

	public Color Color;

	public Colorable Colorable;
	public ColorSwapable ColorSwapable;

	public bool DeleteFlag;

	Vector2 startPosition;
	Vector2 positionDelta;


	void OnValidate () {
		if (Colorable == null) Colorable = GetComponent<Colorable>();
		if (ColorSwapable == null) ColorSwapable = GetComponent<ColorSwapable>();
	}

	public void Init (Color color, Transform parent) {
		Init(parent, Vector2.zero);

		Color = color;
		Colorable.SetColor(Color.clear);
		ColorSwapable.Swap(color);
	}

	public void SetDeleteFlag () {
		if (DeleteFlag) return;
		DeleteFlag = true;
		Active = false;

		ColorSwapable.Swap(Color.red);
	}

	public void ResetDeleteFlag () {
		if (!DeleteFlag) return;
		DeleteFlag = false;
		Active = true;

		ColorSwapable.Swap(Color);
	}

	public void Deinit () {
		ColorSwapable.Swap(Color.clear);

		Destroy(gameObject, 0.5f);
	}

	public void OnPointerClick (PointerEventData eventData) {
		if (OnColorCardClickedEvent != null) OnColorCardClickedEvent(this);
	}

	public void OnBeginDrag (PointerEventData eventData) {
		Controllable = false;

		startPosition = CurrentPosition;
		positionDelta = Vector2.zero;
		
		if (OnColorCardPressedEvent != null) OnColorCardPressedEvent(this);
	}

	public void OnDrag (PointerEventData eventData) {
		positionDelta += eventData.delta * (600.0f / Screen.height);
		CurrentPosition = startPosition + positionDelta;

		if (OnColorCardDragedEvent != null) OnColorCardDragedEvent(this);
	}

	public void OnEndDrag (PointerEventData eventData) {
		Controllable = true;

		if (OnColorCardReleasedEvent != null) OnColorCardReleasedEvent(this);
	}
}
