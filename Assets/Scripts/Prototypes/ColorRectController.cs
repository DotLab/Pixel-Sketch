using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections.Generic;

public class ColorRectController : MonoBehaviour, IInitable<Color>, IPointerDownHandler, IDragHandler, IPointerUpHandler {
	public class ColorComparer : IComparer<ColorRectController> {
		public int Compare (ColorRectController obj1, ColorRectController obj2) {
			var color1 = ColorHelper.Rgb2Hsv(obj1.Color.r, obj1.Color.b, obj1.Color.g);
			var color2 = ColorHelper.Rgb2Hsv(obj2.Color.r, obj2.Color.b, obj2.Color.g);

			if (color1.H.CompareTo(color2.H) != 0) return color1.H.CompareTo(color2.H);
			else return (color1.S - color1.V).CompareTo(color2.S - color2.V);
		}
	}

	public class PositionComparer : IComparer<ColorRectController> {
		public int Compare (ColorRectController obj1, ColorRectController obj2) {
			var obj1Value =
				(obj1.Controllable ? obj1.TargetPosition.x : obj1.Trans.anchoredPosition.x) / obj1.Trans.rect.width
				- (int)((obj1.Controllable ? obj1.TargetPosition.y : obj1.Trans.anchoredPosition.y) / obj1.Trans.rect.height) * 1000;
			var obj2Value =
				(obj2.Controllable ? obj2.TargetPosition.x : obj2.Trans.anchoredPosition.x) / obj2.Trans.rect.width
				- (int)((obj2.Controllable ? obj2.TargetPosition.y : obj2.Trans.anchoredPosition.y) / obj2.Trans.rect.height) * 1000;

			return obj1Value.CompareTo(obj2Value);
		}
	}

	public const float DeleteRange = 200;

	public delegate void OnColorRectRelease (ColorRectController colorRectController, bool deleted);

	public event OnColorRectRelease OnColorRectReleaseEvent;

	public RectTransform Trans { get { return trans; } }

	public bool Controllable { get { return !pressed; } }

	public Color Color { get { return color; } }

	public Vector2 OriginalPosition;
	public Vector2 TargetPosition;


	Color color;

	Vector2 originalSize;
	Vector2 originalPosition;
	Vector2 totalDelta;

	bool pressed;
	bool deletaFlag;

	RectTransform trans;
	IColorable colorable;
	ISwapable<Color> colorSwapable;

	void Awake () {
		trans = GetComponent<RectTransform>();
		colorable = GetComponent<IColorable>();
		colorSwapable = GetComponent<ISwapable<Color>>();
	}

	public void Init (Color color) {
		this.color = color;

		colorable.SetColor(Color.clear);
		colorSwapable.Swap(color);
	}

	public void OnPointerDown (PointerEventData eventData) {
		pressed = true;
		deletaFlag = false;

		originalSize = trans.sizeDelta;
		originalPosition = trans.anchoredPosition;
		totalDelta = Vector2.zero;
	}

	public void OnDrag (PointerEventData eventData) {
		totalDelta += eventData.delta * (600.0f / Screen.height);
		trans.anchoredPosition = originalPosition + totalDelta;

		if (!deletaFlag && totalDelta.magnitude > DeleteRange) {
			deletaFlag = true;
			trans.sizeDelta = originalSize * 2;
			colorSwapable.Swap(Color.red);
		} else if (deletaFlag && totalDelta.magnitude < DeleteRange) {
			deletaFlag = false;
			trans.sizeDelta = originalSize;
			colorSwapable.Swap(Color);
		}
	}

	public void OnPointerUp (PointerEventData eventData) {
		pressed = false;

		if (deletaFlag) {
			colorSwapable.Swap(Color.clear);
			Destroy(gameObject, 0.5f);
		}

		if (OnColorRectReleaseEvent != null) OnColorRectReleaseEvent(this, deletaFlag);
	}
}
