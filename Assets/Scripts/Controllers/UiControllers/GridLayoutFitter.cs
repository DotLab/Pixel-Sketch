using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode]
public class GridLayoutFitter : MonoBehaviour {
	public bool FitX = true;
	public int CountX = 2;
	public float PaddingX = 10;

	public bool FitY = false;
	public int CountY = 2;
	public float PaddingY = 10;

	void Start () {
		OnRectTransformDimensionsChange();
	}

	void OnValidate () {
		OnRectTransformDimensionsChange();
	}

	[ContextMenu("Fit")]
	public void OnRectTransformDimensionsChange () {
		var trans = GetComponent<RectTransform>();
		var grid = GetComponent<GridLayoutGroup>();
		float cellSizeX, cellSizeY;
		cellSizeX = trans.rect.width / CountX - PaddingX * (CountX - 1) / 2;
		cellSizeY = trans.rect.height / CountY - PaddingY * (CountY - 1) / 2;
		if (!FitX) cellSizeX = cellSizeY;
		if (!FitY) cellSizeY = cellSizeX;

		grid.cellSize = new Vector2(cellSizeX, cellSizeY);
	}
}
