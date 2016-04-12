using UnityEngine;

using System.Collections;

public class ColorTabController : MonoBehaviour {
	[System.Serializable]
	public class SubTab {
		public RectTransform Trans;

		public bool Active = true;

		public Vector2 OriginalPosition;
		public Vector2 TargetPosition;

		public Vector2 CurrentPosition {
			get { return Trans.anchoredPosition; }
			set { Trans.anchoredPosition = value; }
		}

		public Vector2 Size {
			get { return Trans.sizeDelta; }
			set { Trans.sizeDelta = value; }
		}

		public SubTab (RectTransform trans) {
			Trans = trans;
		}
	}

	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	[Space]
	public float Spacing = 10;
	public float Padding = 10;

	[Space]
	public float ActiveX;
	public float InactiveX;

	[Space]
	public RectTransform ColorViewer;
	public RectTransform ColorPicker;
	public RectTransform ColorSwatch;

	RectTransform trans;

	SubTab[] subTabs;

	float originalHeight;
	float targetHeight;

	void Awake () {
		trans = GetComponent<RectTransform>();

		subTabs = new SubTab[3];
		subTabs[0] = new SubTab(ColorViewer);
		subTabs[1] = new SubTab(ColorPicker);
		subTabs[2] = new SubTab(ColorSwatch);
	}

	public void OnColorPickerIconClicked () {
		subTabs[1].Active = !subTabs[1].Active;
		Fit();
	}

	public void OnColorSwatchIconClicked () {
		subTabs[2].Active = !subTabs[2].Active;
		Fit();
	}

	[ContextMenu("Fit")]
	public void Fit () {
		var needFit = false;
	
		var height = Padding;
		for (int i = 0; i < 3; i++) {
			subTabs[i].TargetPosition = new Vector2(subTabs[i].Active ? ActiveX : InactiveX, -height);
			subTabs[i].OriginalPosition = subTabs[i].CurrentPosition;
			needFit |= subTabs[i].OriginalPosition != subTabs[i].TargetPosition;

			if (subTabs[i].Active)
				height += subTabs[i].Size.y + Spacing;
		}

		targetHeight = height - Spacing + Padding;
		originalHeight = trans.sizeDelta.y;
		needFit |= originalHeight != targetHeight;

		if (needFit) {
			StopAllCoroutines();
			StartCoroutine(FitHandler());
		}
	}

	IEnumerator FitHandler () {
		float time = 0;

		while (time < TransitionDuration) {
			var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

			trans.sizeDelta = new Vector2(
				trans.sizeDelta.x,
				Mathf.Lerp(originalHeight, targetHeight, easedStep));
			for (int i = 0; i < 3; i++) {
				subTabs[i].CurrentPosition = Vector2.Lerp(
					subTabs[i].OriginalPosition,
					subTabs[i].TargetPosition, easedStep);
			}

			time += Time.deltaTime;
			yield return null;
		}

		trans.sizeDelta = new Vector2(trans.sizeDelta.x, targetHeight);
		for (int i = 0; i < 3; i++) {
			subTabs[i].CurrentPosition = subTabs[i].TargetPosition;
		}
	}
}
