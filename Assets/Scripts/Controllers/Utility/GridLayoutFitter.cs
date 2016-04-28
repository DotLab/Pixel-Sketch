using UnityEngine;
using UnityEngine.UI;

public class GridLayoutFitter : MonoBehaviour {
	[Range(1, 32)]
	public int Count = 2;

	public int Spacing = 10;
	public int Padding = 10;


	void OnValidate () {
		OnRectTransformDimensionsChange();
	}

	[ContextMenu("Fit")]
	public void OnRectTransformDimensionsChange () {
		var trans = GetComponent<RectTransform>();
		var grid = GetComponent<GridLayoutGroup>();
	
		if (trans.rect.width < 0) return;

		float cellSize;

		cellSize = (trans.rect.width - Padding * 2 - Spacing * (Count - 1)) / Count;

		grid.startAxis = GridLayoutGroup.Axis.Horizontal;
		grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
		grid.constraintCount = Count;


		grid.spacing = new Vector2(Spacing, Spacing);
		grid.padding = new RectOffset(Padding, Padding, Padding, Padding);

		grid.cellSize = new Vector2(cellSize, cellSize);
	}
}
