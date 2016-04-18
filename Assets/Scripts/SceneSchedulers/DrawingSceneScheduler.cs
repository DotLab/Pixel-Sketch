using UnityEngine;

public class DrawingSceneScheduler : MonoBehaviour {
	public CursorController Cursor;
	public CanvasController Canvas;

	public ToolBarController ToolBar;
	public ColorTabController ColorTab;
	public LayerTabController LayerTab;

	public Color MainColor;

	Drawing drawing;

	void Awake () {
		drawing = new Drawing(Canvas, new Short2(128, 64));

		Cursor.OnCursorClickedEvent += position => DebugConsole.Log("OnCursorClickedEvent " + position);
		Cursor.OnCursorDraggedEvent += position => DebugConsole.Log("OnCursorDraggedEvent " + position);
		Cursor.OnCursorMovedEvent += position => DebugConsole.Log("OnCursorMovedEvent " + position);
		
		ToolBar.OnToolChangedEvent += OnToolChanged;
		
		ColorTab.OnColorChangedEvent += color => MainColor = color;
		
		LayerTab.OnLayerAddedEvent += drawing.AddLayer;
		LayerTab.OnLayerDeletedEvent += drawing.DeleteLayer;
		LayerTab.OnLayerSelectedEvent += drawing.SelectLayer;
		LayerTab.OnLayerChangedEvent += layer => drawing.RenderDrawing();
	}

	void OnToolChanged (ToolType toolType) {
		if (toolType == ToolType.MoveTool)
			Cursor.SetCursorType(CursorType.MoveObjectCursor);
		else if (((int)toolType & (int)ToolType.ToolFamilyMask) == (int)ToolType.SelectTool)
			Cursor.SetCursorType(CursorType.CrossCursor);
		else if (toolType == ToolType.PencilPaintTool)
			Cursor.SetCursorType(CursorType.PencilActionCursor);
		else if (toolType == ToolType.BrushPaintTool)
			Cursor.SetCursorType(CursorType.BrushActionCursor);
		else if (toolType == ToolType.EraserTool)
			Cursor.SetCursorType(CursorType.EraserActionCursor);
		else if (((int)toolType & (int)ToolType.ToolFamilyMask) == (int)ToolType.ShapeTool)
			Cursor.SetCursorType(CursorType.CrossCursor);
	}
}
