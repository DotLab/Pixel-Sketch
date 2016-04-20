using UnityEngine;
using UnityEngine.UI;

public class ConfigPopupController : MonoBehaviour {
	public CanvasGridController CanvasGrid;

	[Space]
	public InputField ResolutionXField;
	public InputField ResolutionYField;

	public InputField GridSizeField;
	public InputField GridSubdivisionField;

	void Start () {
		ResetResolutionField();
		ResetGridField();
	}

	public void OnResolutionFieldEndEdit () {
		int x, y;
		var stringX = ResolutionXField.text.Replace("px", "");
		var stringY = ResolutionYField.text.Replace("px", "");
		if (
			int.TryParse(stringX, out x)
			&& int.TryParse(stringY, out y)
			&& 0 < x && 0 < y) {

			ResolutionXField.text = x + "px";
			ResolutionYField.text = y + "px";

			DrawingSceneScheduler.Instance.ResizeDrawing(new Short2(x, y));
		} else ResetResolutionField();
	}

	void ResetResolutionField () {
		var drawingSize = DrawingSceneScheduler.Instance.DrawingSize;
		ResolutionXField.text = drawingSize.x + "px";
		ResolutionYField.text = drawingSize.y + "px";
	}

	public void OnGridFieldEndEdit () {
		int gridSize, subdivision;
		var drawingSize = DrawingSceneScheduler.Instance.DrawingSize;
		if (
			int.TryParse(GridSizeField.text, out gridSize)
			&& int.TryParse(GridSubdivisionField.text, out subdivision)
			&& 0 <= gridSize && gridSize <= Mathf.Max(drawingSize.x, drawingSize.y)
			&& 0 < subdivision && subdivision <= gridSize) {

			CanvasGrid.GridSize = gridSize;
			CanvasGrid.Subdivision = subdivision;
		} else ResetGridField();
	}

	void ResetGridField () {
		GridSizeField.text = CanvasGrid.GridSize.ToString();
		GridSubdivisionField.text = CanvasGrid.Subdivision.ToString();
	}

	public void OnSaveAndQuitButtonClicked () {
		DebugConsole.Log("Save and quit.");
	}

	public void OnDiscardButtonClicked () {
		DebugConsole.Log("Discard changes.");
	}
}
