using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections.Generic;

using HsvColorPicker;
using Uif;

public class ColorCardController : RectContent, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public class ColorComparer : IComparer<ColorCardController> {
		public int Compare (ColorCardController obj1, ColorCardController obj2) {
			var color1 = ColorHelper.Rgb2Hsv(obj1.Color);
			var color2 = ColorHelper.Rgb2Hsv(obj2.Color);

			return color1.h != color2.h ? color1.h.CompareTo(color2.h) : (color1.v).CompareTo(color2.v);
		}
	}

	public delegate void OnStateChanged (ColorCardController colorCard);

	public event OnStateChanged OnClickedEvent;
	public event OnStateChanged OnPressedEvent;
	public event OnStateChanged OnDragedEvent;
	public event OnStateChanged OnReleasedEvent;

	public Color Color;

	public bool DeleteFlag {
		get { return deleteFlag; }
		set {
			if (deleteFlag == value) return;

			deleteFlag = value;
			Active = !deleteFlag;

			ColorSwapable.Swap(deleteFlag ? Color.red : Color);
		}
	}

	public Colorable Colorable;
	public ColorSwapable ColorSwapable;

	bool deleteFlag;

	Vector2 touchStartPosition;
	Vector2 touchDeltaPosition;

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

	public void OnPointerClick (PointerEventData eventData) {
		if (Controllable && !deleteFlag)
		if (OnClickedEvent != null) OnClickedEvent(this);
	}

	public void OnBeginDrag (PointerEventData eventData) {
		Controllable = false;

		touchStartPosition = CurrentPosition;
		touchDeltaPosition = Vector2.zero;
		
		if (OnPressedEvent != null) OnPressedEvent(this);
	}

	public void OnDrag (PointerEventData eventData) {
		touchDeltaPosition += eventData.delta * (600.0f / Screen.height);
		CurrentPosition = touchStartPosition + touchDeltaPosition;

		if (OnDragedEvent != null) OnDragedEvent(this);
	}

	public void OnEndDrag (PointerEventData eventData) {
		Controllable = true;

		if (deleteFlag) Delete();

		if (OnReleasedEvent != null) OnReleasedEvent(this);
	}

	void Delete () {
		ColorSwapable.Swap(Color.clear);

		Destroy(gameObject, 0.5f);
	}
}
