using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LayerTabContentFitter : MonoBehaviour {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	public float Spacing = 10;
	public float Padding = 10;

	public float ShowX;
	public float HideX;

	public readonly List<LayerController> Contents = new List<LayerController>();

	RectTransform trans;

	float originalHeight;
	float targetHeight;

	void Awake () {
		trans = GetComponent<RectTransform>();
	}

	[ContextMenu("Fit")]
	public void Fit () {
		var needFit = false;

		originalHeight = trans.rect.height;
		targetHeight = Padding;
		for (int i = 0; i < Contents.Count; i++) {
			Contents[i].OriginalPosition = Contents[i].Trans.anchoredPosition;
			Contents[i].TargetPosition = new Vector2(0, -targetHeight);

			needFit |= Contents[i].OriginalPosition != Contents[i].TargetPosition;

			if (!Contents[i].DeleteFlag) targetHeight += Contents[i].Trans.rect.height + Spacing;
		}
		targetHeight = targetHeight - Spacing + Padding;
		needFit |= originalHeight != targetHeight;

		if (!needFit) return;

		StopAllCoroutines();
		StartCoroutine(FitHandler());
	}

	IEnumerator FitHandler () {
		float time = 0;

		while (time < TransitionDuration) {
			var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

			trans.sizeDelta = new Vector2(trans.sizeDelta.x, Mathf.Lerp(originalHeight, targetHeight, easedStep));
			for (int i = 0; i < Contents.Count; i++) {
				if (!Contents[i].Controllable) continue;
				Contents[i].Trans.anchoredPosition = Vector2.Lerp(
					Contents[i].OriginalPosition,
					Contents[i].TargetPosition, easedStep);
			}

			time += Time.deltaTime;
			yield return null;
		}

		trans.sizeDelta = new Vector2(trans.sizeDelta.x, targetHeight);
		for (int i = 0; i < Contents.Count; i++) {
			if (!Contents[i].Controllable) continue;
			Contents[i].Trans.anchoredPosition = Contents[i].TargetPosition;
		}
	}
}
