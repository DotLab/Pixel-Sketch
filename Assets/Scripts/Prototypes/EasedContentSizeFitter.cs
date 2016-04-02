using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EasedContentSizeFitter : MonoBehaviour {
	[System.Serializable]
	public class UiContent {
		public RectTransform Trans;
		public bool Active = true;

		public float OriginalX;
		public float OriginalY;
		public float TargetX;
		public float TargetY;

		public UiContent (RectTransform trans, bool active) {
			Trans = trans;
			Active = active;
		}
	}

	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	public float Spacing = 10;
	public float Padding = 10;

	public float ShowX;
	public float HideX;

	public List<UiContent> Contents;

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
			Contents[i].OriginalX = Contents[i].Trans.anchoredPosition.x;
			Contents[i].OriginalY = Contents[i].Trans.anchoredPosition.y;
			Contents[i].TargetX = Contents[i].Active ? ShowX : HideX;
			Contents[i].TargetY = -targetHeight;
			needFit |= Contents[i].OriginalX != Contents[i].TargetX || Contents[i].OriginalY != Contents[i].TargetY;

			if (Contents[i].Active) targetHeight += Contents[i].Trans.rect.height + Spacing;
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
				Contents[i].Trans.anchoredPosition = new Vector2(
					Mathf.Lerp(Contents[i].OriginalX, Contents[i].TargetX, easedStep),
					Mathf.Lerp(Contents[i].OriginalY, Contents[i].TargetY, easedStep));
			}

			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		trans.sizeDelta = new Vector2(trans.sizeDelta.x, targetHeight);
		for (int i = 0; i < Contents.Count; i++) {
			Contents[i].Trans.anchoredPosition = new Vector2(
				Contents[i].TargetX,
				Contents[i].TargetY);
		}
	}
}
