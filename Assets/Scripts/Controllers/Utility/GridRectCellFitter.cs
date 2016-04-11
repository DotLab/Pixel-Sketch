using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class GridRectCellFitter : MonoBehaviour {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	public bool EasePosition = true;
	public bool EaseSize = true;

	[Space]
	// Max grid count along axis.
	public int CellCount = 4;
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
	
		// Begin by top left corner.
		targetSize = new Vector2(0, Padding.y);

		int i = 0;
		while (i < Cells.Count) {
			int x = 0;
			// Begin one row from left side;
			float rowWidth = Padding.x;
			float rowHeight = 0;

			while (x < CellCount && i < Cells.Count) {
				Cells[i].TargetPosition = new Vector2(rowWidth, -targetSize.y);

				if (!EasePosition) Cells[i].CurrentPosition = Cells[i].TargetPosition;
				Cells[i].OriginalPosition = Cells[i].CurrentPosition;
				needFit |= Cells[i].OriginalPosition != Cells[i].TargetPosition;

				if (Cells[i].Active) {
					// If active, add content's width to the row width.
					rowWidth += Cells[i].Size.x + Spacing.x;
					rowHeight = rowHeight < Cells[i].Size.y ? Cells[i].Size.y : rowHeight;
					x++;
				}

				i++;
			}
			// targetSize.x is the max width of all rows.
			rowWidth = rowWidth - Spacing.x + Padding.x;
			targetSize.x = targetSize.x < rowWidth ? rowWidth : targetSize.x;
			// Add the max height of current row to targetSize.y.
			targetSize.y += rowHeight + Spacing.y;
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

			if (EasePosition)
				for (int i = 0; i < Cells.Count; i++) {
					if (!Cells[i].Controllable) continue;

					Cells[i].CurrentPosition = Vector2.Lerp(
						Cells[i].OriginalPosition,
						Cells[i].TargetPosition, easedStep);
				}
			
			if (EaseSize)
				trans.sizeDelta = Vector2.Lerp(originalSize, targetSize, easedStep);

			time += Time.deltaTime;
			yield return null;
		}

		for (int i = 0; i < Cells.Count; i++) {
			if (!Cells[i].Controllable) continue;

			Cells[i].Trans.anchoredPosition = Cells[i].TargetPosition;
		}
		trans.sizeDelta = targetSize;
	}
}