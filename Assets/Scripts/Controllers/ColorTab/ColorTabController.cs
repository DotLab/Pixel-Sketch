using UnityEngine;

using System.Collections;

using HsvColorPicker;
using Uif;

public class ColorTabController : MonoBehaviour {
	[System.Serializable]
	public class SubTab {
		public RectTransform Trans;

		public bool Active = true;

		public Vector2 TargetPosition  { get; set; }

		public Vector2 OriginalPosition  { get; set; }

		public Vector2 CurrentPosition {
			get { return Trans.anchoredPosition; }
			set { Trans.anchoredPosition = value; }
		}

		public Vector2 Size {
			get { return Trans.sizeDelta; }
			set { Trans.sizeDelta = value; }
		}
	}

	public delegate void OnColorChanged (Color color);

	public event OnColorChanged OnColorChangedEvent;

	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	[Space]
	public float Spacing = 10;
	public float Padding = 10;

	[Space]
	public float ActiveX;
	public float InactiveX;

	[Space]
	public ColorPicker ColorPicker;
	public Hidable ColorPickerPopup;
	public RectTransform ColorTabRect;
	public SubTab[] SubTabs = new SubTab[3];

	float targetHeight;
	float originalHeight;

	void Awake () {
		ColorPicker.OnColorChangedEvent += () => {
			if (OnColorChangedEvent != null)
				OnColorChangedEvent(ColorPicker.CurrentColor);
		};
	}

	public void OnControlButtonClicked (int index) {
		SubTabs[index].Active = !SubTabs[index].Active;
		Fit();
	}

	public void OnColorFieldButtonClicked () {
		PopupManager.Instance.ShowPopup(ColorPickerPopup);
	}

	[ContextMenu("Fit")]
	public void Fit () {
		var needFit = false;
	
		var heightPointer = Padding;
		for (int i = 0; i < 3; i++) {
			SubTabs[i].TargetPosition = new Vector2(SubTabs[i].Active ? ActiveX : InactiveX, -heightPointer);
			SubTabs[i].OriginalPosition = SubTabs[i].CurrentPosition;
			needFit |= SubTabs[i].OriginalPosition != SubTabs[i].TargetPosition;

			if (SubTabs[i].Active)
				heightPointer += SubTabs[i].Size.y + Spacing;
		}

		targetHeight = heightPointer - Spacing + Padding;
		originalHeight = ColorTabRect.sizeDelta.y;
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

			ColorTabRect.sizeDelta = new Vector2(
				ColorTabRect.sizeDelta.x,
				Mathf.Lerp(originalHeight, targetHeight, easedStep));
			for (int i = 0; i < SubTabs.Length; i++) {
				SubTabs[i].CurrentPosition = Vector2.Lerp(
					SubTabs[i].OriginalPosition,
					SubTabs[i].TargetPosition, easedStep);
			}

			time += Time.deltaTime;
			yield return null;
		}

		ColorTabRect.sizeDelta = new Vector2(ColorTabRect.sizeDelta.x, targetHeight);
		for (int i = 0; i < SubTabs.Length; i++) {
			SubTabs[i].CurrentPosition = SubTabs[i].TargetPosition;
		}
	}
}
