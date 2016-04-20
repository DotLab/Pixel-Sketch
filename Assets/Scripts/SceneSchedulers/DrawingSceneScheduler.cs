using UnityEngine;

public class DrawingSceneScheduler : MonoBehaviour {
	public static DrawingSceneScheduler Instance {
		get { return instance; }
	}

	static DrawingSceneScheduler instance;

	public CursorController Cursor;
	public CanvasController Canvas;

	public ToolBarController ToolBar;
	public ColorTabController ColorTab;
	public LayerTabController LayerTab;

	public Color MainColor;
	public Short2 DrawingSize;
	public ToolType CurrentTool;

	Drawing drawing;

	void Awake () {
		instance = this;
		drawing = new Drawing(Canvas, DrawingSize);

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

	void Start () {
		drawing.DrawPoly(new [] { new Short2(2, 3), new Short2(12, 31), new Short2(19, 6) }, Color.red);
		drawing.ApplyDraw();
	}

	public void ResizeDrawing (Short2 size) {
		drawing.ResizeDrawing(size);
	}

	void OnToolChanged (ToolType toolType) {
		CurrentTool = toolType;

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
