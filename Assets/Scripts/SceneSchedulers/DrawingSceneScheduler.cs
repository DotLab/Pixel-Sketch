using UnityEngine;

public class DrawingSceneScheduler : MonoBehaviour {
	public static DrawingSceneScheduler Instance { get { return instance; } }

	static DrawingSceneScheduler instance;

	public CursorController Cursor;
	public CanvasController Canvas;

	public ToolBarController ToolBar;
	public ColorTabController ColorTab;
	public LayerTabController LayerTab;

	public Color MainColor;
	public ToolType CurrentTool;

	public Drawing Drawing;

	void Awake () {
		instance = this;
		Drawing = new Drawing(Canvas, new Short2(32, 32));

		Cursor.OnCursorClickedEvent += position => DebugConsole.Log("OnCursorClickedEvent " + position);
		Cursor.OnCursorDraggedEvent += position => DebugConsole.Log("OnCursorDraggedEvent " + position);
		Cursor.OnCursorMovedEvent += position => DebugConsole.Log("OnCursorMovedEvent " + position);
		
		ToolBar.OnToolChangedEvent += OnToolChanged;
		
		ColorTab.OnColorChangedEvent += color => MainColor = color;
		
		LayerTab.OnLayerAddedEvent += Drawing.AddLayer;
		LayerTab.OnLayerDeletedEvent += Drawing.DeleteLayer;
		LayerTab.OnLayerSelectedEvent += Drawing.SelectLayer;
		LayerTab.OnLayerChangedEvent += layer => Drawing.RenderDrawing();
	}

	void Start () {
		Drawing.DrawPoly(new [] { new Short2(2, 3), new Short2(12, 31), new Short2(19, 6) }, Color.red);
		Drawing.ApplyDraw();
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
