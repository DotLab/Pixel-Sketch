using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class VerticalRectCellFitter : MonoBehaviour {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	public bool EasePosition = true;
	public bool EaseSize = true;

	[Space]
	// Space around all the contents.
	public Vector2 Padding;
	// Space between all the contents.
	public Vector2 Spacing;

	[Space]
	public List<RectCell> Cells;

	RectTransform trans;

	Vector2 originalSize;
	Vector2 targetSize;


	void Awake () {
		trans = GetComponent<RectTransform>();
	}

	[ContextMenu("Fit")]
	public void Fit () {
		var needFit = false;
	
		// Begin by top.
		targetSize = new Vector2(0, Padding.y);
	
		for (int i = 0; i < Cells.Count; i++) {
			Cells[i].TargetPosition = new Vector2(Padding.x, -targetSize.y);

			if (!EasePosition) Cells[i].CurrentPosition = Cells[i].TargetPosition;
			Cells[i].OriginalPosition = Cells[i].CurrentPosition;
			needFit |= Cells[i].OriginalPosition != Cells[i].TargetPosition;

			// Check height.
			if (Cells[i].Active) targetSize.y += Cells[i].Size.y + Spacing.y;

			// Check width.
			var rowWidth = Padding.x + Cells[i].Size.x + Padding.x;
			targetSize.x = targetSize.x < rowWidth ? rowWidth : targetSize.x;
		}
		targetSize.y = targetSize.y - Spacing.y + Padding.y;

		if (!EaseSize) trans.sizeDelta = targetSize;
		originalSize = trans.sizeDelta;
		needFit |= originalSize == targetSize;

		if (!needFit) return;

		StopAllCoroutines();
		StartCoroutine(FitHandler());
	}

	IEnumerator FitHandler () {
		float time = 0;

		while (time < TransitionDuration) {
			var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

			trans.sizeDelta = Vector2.Lerp(originalSize, targetSize, easedStep);
			for (int i = 0; i < Cells.Count; i++) {
				if (!Cells[i].Controllable) continue;

				Cells[i].CurrentPosition = Vector2.Lerp(
					Cells[i].OriginalPosition,
					Cells[i].TargetPosition, easedStep);
			}

			time += Time.deltaTime;
			yield return null;
		}

		trans.sizeDelta = targetSize;
		for (int i = 0; i < Cells.Count; i++) {
			if (!Cells[i].Controllable) continue;

			Cells[i].Trans.anchoredPosition = Cells[i].TargetPosition;
		}
	}
}