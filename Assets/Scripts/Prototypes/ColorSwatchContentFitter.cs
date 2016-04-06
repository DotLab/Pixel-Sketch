using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ColorSwatchContentFitter : MonoBehaviour {
	[System.Serializable]
	public class UiContent : IComparable<UiContent> {
		public RectTransform Trans;
		public bool Controllable = true;

		public float OriginalX;
		public float OriginalY;
		public float TargetX;
		public float TargetY;

		public UiContent (RectTransform trans, bool controllable = true) {
			Trans = trans;
			Controllable = controllable;
		}

		public int CompareTo (UiContent obj) {
			var selfValue = 
				(Controllable ? TargetX : Trans.anchoredPosition.x) / Trans.rect.width
				- (int)((Controllable ? TargetY : Trans.anchoredPosition.y) / Trans.rect.height) * 1000;
			var objValue =
				(obj.Controllable ? obj.TargetX : obj.Trans.anchoredPosition.x) / obj.Trans.rect.width
				- (int)((obj.Controllable ? obj.TargetY : obj.Trans.anchoredPosition.y) / obj.Trans.rect.height) * 1000;

			return selfValue.CompareTo(objValue);
		}
	}

	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	public int GridCount = 4;

	public float Spacing = 10;
	public float Padding = 10;

	public List<UiContent> Contents;

	RectTransform trans;

	float originalHeight;
	float originalWidth;
	float targetHeight;
	float targetWidth;

	void Awake () {
		trans = GetComponent<RectTransform>();
	}

	[ContextMenu("Fit")]
	public void Fit () {
		var needFit = false;

		originalHeight = trans.rect.height;
		originalWidth = trans.rect.width;
		targetHeight = Padding;

		for (int y = 0; y < Contents.Count / GridCount + 1; y++) {
			targetWidth = Padding;
			for (int x = 0; x < GridCount; x++) {
				var i = y * GridCount + x;
				if (i >= Contents.Count) break;
				Contents[i].OriginalX = Contents[i].Trans.anchoredPosition.x;
				Contents[i].OriginalY = Contents[i].Trans.anchoredPosition.y;
				Contents[i].TargetX = targetWidth;
				Contents[i].TargetY = -targetHeight;
				needFit |= Contents[i].OriginalX != Contents[i].TargetX || Contents[i].OriginalY != Contents[i].TargetY;

				targetWidth += Contents[i].Trans.rect.width + Spacing;
			}
			if (y * GridCount >= Contents.Count) break;
			targetHeight += Contents[y * GridCount].Trans.rect.height + Spacing;
		}
		targetWidth = Padding * 2 + Spacing * (GridCount - 1) + Contents[0].Trans.rect.width * GridCount;
		targetHeight = targetHeight - Spacing + Padding;
		needFit |= originalHeight != targetHeight || originalWidth != targetWidth;

		if (!needFit) return;

		StopAllCoroutines();
		StartCoroutine(FitHandler());
	}

	IEnumerator FitHandler () {
		float time = 0;

		while (time < TransitionDuration) {
			var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

			trans.sizeDelta = new Vector2(
				Mathf.Lerp(originalWidth, targetWidth, easedStep),
				Mathf.Lerp(originalHeight, targetHeight, easedStep));
			for (int i = 0; i < Contents.Count; i++) {
				if (!Contents[i].Controllable) continue;
				Contents[i].Trans.anchoredPosition = new Vector2(
					Mathf.Lerp(Contents[i].OriginalX, Contents[i].TargetX, easedStep),
					Mathf.Lerp(Contents[i].OriginalY, Contents[i].TargetY, easedStep));
			}

			time += Time.deltaTime;
			yield return null;
		}

		trans.sizeDelta = new Vector2(targetWidth, targetHeight);
		for (int i = 0; i < Contents.Count; i++) {
			if (!Contents[i].Controllable) continue;
			Contents[i].Trans.anchoredPosition = new Vector2(
				Contents[i].TargetX,
				Contents[i].TargetY);
		}
	}
}
