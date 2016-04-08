using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ColorSwatchContentFitter : MonoBehaviour {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	public int GridCount = 4;

	public float Spacing = 10;
	public float Padding = 10;

	public readonly List<ColorRectController> Contents = new List<ColorRectController>();

	RectTransform trans;

	float targetHeight;
	float targetWidth;

	void Awake () {
		trans = GetComponent<RectTransform>();
	}

	[ContextMenu("Fit")]
	public void Fit () {
		var needFit = false;

		targetHeight = Padding;

		for (int y = 0; y < Contents.Count / GridCount + 1; y++) {
			targetWidth = Padding;
			for (int x = 0; x < GridCount; x++) {
				var i = y * GridCount + x;
				if (i >= Contents.Count) break;
				Contents[i].OriginalPosition = Contents[i].Trans.anchoredPosition;
				Contents[i].TargetPosition = new Vector2(targetWidth, -targetHeight);

				needFit |= Contents[i].OriginalPosition != Contents[i].TargetPosition;

				targetWidth += Contents[i].Trans.rect.width + Spacing;
			}
			if (y * GridCount >= Contents.Count) break;
			targetHeight += Contents[y * GridCount].Trans.rect.height + Spacing;
		}
		targetWidth = Contents.Count < 1 ? 0 : Padding * 2 + Spacing * (GridCount - 1) + Contents[0].Trans.rect.width * GridCount;
		targetHeight = targetHeight - Spacing + Padding;
		trans.sizeDelta = new Vector2(targetWidth, targetHeight);

		if (!needFit) return;


		StopAllCoroutines();
		StartCoroutine(FitHandler());
	}

	IEnumerator FitHandler () {
		float time = 0;

		while (time < TransitionDuration) {
			var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

			for (int i = 0; i < Contents.Count; i++) {
				if (!Contents[i].Controllable) continue;
				Contents[i].Trans.anchoredPosition = Vector2.Lerp(
					Contents[i].OriginalPosition,
					Contents[i].TargetPosition, easedStep);
			}

			time += Time.deltaTime;
			yield return null;
		}

		for (int i = 0; i < Contents.Count; i++) {
			if (!Contents[i].Controllable) continue;
			Contents[i].Trans.anchoredPosition = Contents[i].TargetPosition;
		}
	}
}
