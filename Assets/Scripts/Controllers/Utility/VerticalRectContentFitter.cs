using UnityEngine;

public class VerticalRectContentFitter : RectContentFitter {
	public override bool TryFit (RectContent[] contents) {
		var needFit = false;

		var heightPointer = Padding;
		for (int i = 0; i < contents.Length; i++) {
			contents[i].TargetPosition = new Vector2(0, -heightPointer);
			contents[i].OriginalPosition = contents[i].CurrentPosition;
			needFit |= contents[i].OriginalPosition != contents[i].TargetPosition;

			if (contents[i].Active)
				heightPointer += contents[i].Size.y + Spacing;
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
