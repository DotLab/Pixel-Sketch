using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class ColorSwatchContentFitter : MonoBehaviour {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	[Space]
	public float Spacing = 10;
	public float Padding = 10;

	[Space]
	public int GridCount = 4;

	[Space]
	public List<ColorCardController> ColorCards;

	RectTransform trans;

	float targetHeight;
	float targetWidth;

	void Awake () {
		trans = GetComponent<RectTransform>();
	}

	[ContextMenu("Fit")]
	public void Fit () {
		var needFit = false;

		targetHeight = 0;
		targetWidth = 0;

		float height = Padding;

		int i = 0;
		while (i < ColorCards.Count) {
			float width = 0;
			float rowHeight = 0;

			int x = 0;
			while (x < GridCount && i < ColorCards.Count) {
				ColorCards[i].TargetPosition = new Vector2(width, -height);
				ColorCards[i].OriginalPosition = ColorCards[i].CurrentPosition;
				needFit |= ColorCards[i].TargetPosition != ColorCards[i].OriginalPosition;

				if (ColorCards[i].Active) {
					width += ColorCards[i].Size.x + Spacing;
					rowHeight = rowHeight < ColorCards[i].Size.y ? ColorCards[i].Size.y : rowHeight;
					x++;
				}
				i++;
			}
			height += rowHeight + Spacing;
			width = width - Spacing + Padding;
			targetWidth = targetWidth < width ? width : targetWidth;
		}
		targetHeight = height - Spacing + Padding;

		trans.sizeDelta = new Vector2(trans.sizeDelta.x, targetHeight);

		if (needFit) {
			StopAllCoroutines();
			StartCoroutine(FitHandler());
		}
	}

	IEnumerator FitHandler () {
		float time = 0;

		while (time < TransitionDuration) {
			var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

			for (int i = 0; i < ColorCards.Count; i++) {
				if (!ColorCards[i].Controllable) continue;

				ColorCards[i].CurrentPosition = Vector2.Lerp(
					ColorCards[i].OriginalPosition,
					ColorCards[i].TargetPosition, easedStep);
			}

			time += Time.deltaTime;
			yield return null;
		}

		for (int i = 0; i < ColorCards.Count; i++) {
			if (!ColorCards[i].Controllable) continue;

			ColorCards[i].CurrentPosition = ColorCards[i].TargetPosition;
		}
	}
}
