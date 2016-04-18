using UnityEngine;

public class GridRectContentFitter : RectContentFitter {
	[Space]
	public int GridCount = 4;

	public override bool TryFit (RectContent[] contents) {
		var needFit = false;

		float heightPointer = Padding;
		float maxRowWidth = 0;

		int i = 0;
		while (i < contents.Length) {
			float widthPointer = Padding;
			float maxRowHeight = 0;

			int x = 0;
			while (x < GridCount && i < contents.Length) {
				contents[i].TargetPosition = new Vector2(widthPointer, -heightPointer);
				contents[i].OriginalPosition = contents[i].CurrentPosition;
				needFit |= contents[i].TargetPosition != contents[i].OriginalPosition;

				if (contents[i].Active) {
					widthPointer += contents[i].Size.x + Spacing;
					maxRowHeight = Mathf.Max(maxRowHeight, contents[i].Size.y);
					x++;
				}
				i++;
			}
			heightPointer += maxRowHeight + Spacing;
			maxRowWidth = Mathf.Max(maxRowWidth, widthPointer);
		}
		targetSize = new Vector2(
			trans.sizeDelta.x,
			heightPointer - Spacing + Padding);
		originalSize = trans.sizeDelta;

		if (!EaseSize) trans.sizeDelta = targetSize;
		needFit |= targetSize != originalSize;

		return needFit;
	}
}
