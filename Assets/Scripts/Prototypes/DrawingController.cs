using UnityEngine;

public class DrawingController : MonoBehaviour {
	public ToolBarController ToolBar;
	public ColorPicker ColorPicker;
	public LayerTabController LayerTab;

	void Awake () {
		ToolBar.OnDrawingToolChangedEvent += OnDrawingToolChange;
		ColorPicker.OnColorChangedEvent += OnColorChange;

	}

	void OnColorChange (Color color) {
		
	}

	void OnDrawingToolChange (ToolBarController.DrawingToolType newType) {
		
	}
}
