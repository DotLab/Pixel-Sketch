using UnityEngine;
using UnityEngine.EventSystems;

public class ColorRectController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
	public ColorSwatchController ColorSwatchController;
	public Color Color;

	public RectTransform Trans;
	public ColorSwatchContentFitter.UiContent UiContent;

	Vector2 originalPosition;
	Vector2 totalDelta;

	IColorable colorable;

	void Awake () {
		colorable = GetComponent<IColorable>();
	}

	public void SetColor (Color color) {
		Color = color;
		colorable.SetColor(Color);
	}

	public void OnPointerDown (PointerEventData eventData) {
		originalPosition = Trans.anchoredPosition;
		totalDelta = Vector2.zero;

		ColorSwatchController.OnColorRectPress(this);
	}

	public void OnDrag (PointerEventData eventData) {
		totalDelta += eventData.delta * (600.0f / Screen.height);
		Trans.anchoredPosition = originalPosition + totalDelta;
		//Trans.anchoredPosition = Vector2.Lerp(Trans.anchoredPosition, originalPosition + totalDelta, Time.deltaTime * 2);

		ColorSwatchController.OnColorRectDrag(this);
	}

	public void OnPointerUp (PointerEventData eventData) {
		ColorSwatchController.OnColorRectRelease(this);
	}
}
