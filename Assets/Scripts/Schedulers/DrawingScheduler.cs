using UnityEngine;

public class DrawingScheduler : MonoBehaviour {
	public static Short2 DrawingSize { get; private set; }

	public static ToolType CurrentTool { get; private set; }

	public static Color MainColor { get; private set; }

	public static float AspectRatio { get { return Camera.main.aspect; } }

	public static float UiHeight { get { return 600.0f; } }

	public static float UiWidth { get { return UiHeight * AspectRatio; } }

	public static float Screen2Ui { get { return UiHeight / Screen.height; } }

	public static float Coordinate2Ui { get { return UiHeight / DrawingSize.y; } }

	public static float Ui2Coordinate { get { return DrawingSize.y / UiHeight; } }

	public static Drawing Drawing { get { return drawing; } }

	static Drawing drawing;

	public static void ResizeDrawing (Short2 size) {
		DrawingSize = size;
		drawing.ResizeDrawing(size);
	}

	public static bool IsIllegal (Short2 c) {
		return c.x < 0 || c.x >= DrawingSize.x || c.y < 0 || c.y >= DrawingSize.y;
	}

	public CanvasController Canvas;
	public CursorController Cursor;
	public CursorTouchHandler CursorTouch;
	public SelectionTouchHandler SelectionTouch;

	public ToolBarController ToolBar;
	public ColorTabController ColorTab;
	public ColorSwatchController ColorSwatch;
	public LayerTabController LayerTab;


	void Awake () {
		DrawingSize = new Short2(32, 32);
		drawing = new Drawing(DrawingSize, Canvas, SelectionController.Selection);

		Cursor.OnCursorMovedEvent += OnCursorMoved;
		Cursor.OnCursorDraggedEvent += OnCursorDragged;
		Cursor.OnCursorClickedEvent += OnCursorClicked;

		ToolBar.OnToolChangedEvent += OnToolChanged;

		ColorTab.OnColorChangedEvent += color => MainColor = color;

		LayerTab.OnLayerAddedEvent += drawing.AddLayer;
		LayerTab.OnLayerDeletedEvent += drawing.DeleteLayer;
		LayerTab.OnLayerSelectedEvent += drawing.SelectLayer;
		LayerTab.OnLayerChangedEvent += layer => drawing.RenderDrawing();
	}

	void OnToolChanged (ToolType toolType) {
		if (toolType == ToolType.PencilPaintTool)
			Cursor.SetCursorType(CursorType.PencilActionCursor);
		else if (toolType == ToolType.BrushPaintTool)
			Cursor.SetCursorType(CursorType.BrushActionCursor);
		else if (toolType == ToolType.EraserTool)
			Cursor.SetCursorType(CursorType.EraserActionCursor);
		else if (toolType == ToolType.BrushPaintTool)
			Cursor.SetCursorType(CursorType.BrushActionCursor);
		else if (toolType == ToolType.MagicNewTool)
			Cursor.SetCursorType(CursorType.MagicNewCursor);
		else if (toolType == ToolType.MagicAddTool)
			Cursor.SetCursorType(CursorType.MagicAddCursor);
		else if (toolType == ToolType.MagicSubTool)
			Cursor.SetCursorType(CursorType.MagicSubCursor);
		else if (toolType == ToolType.TransTool) {
			Cursor.HideCursor();
			drawing.ApplySelection();
			SelectionTouch.Active = true;
			CursorTouch.Active = false;
		}

		if (CurrentTool == ToolType.TransTool && toolType != ToolType.TransTool) {
			drawing.ApplyTransform();
			SelectionTouch.Active = false;
			CursorTouch.Active = true;
		}

		CurrentTool = toolType;
	}

	void OnCursorMoved (Vector2 position) {
	
	}

	void OnCursorDragged (Vector2 position) {
		var lastPoint = CursorToCoordinate(Cursor.LastPosition);
		var point = CursorToCoordinate(position);

		switch (CurrentTool) {
		case ToolType.PencilPaintTool:
			ColorSwatch.AddPickerColor();
			drawing.DrawLine(lastPoint, point, MainColor);
			drawing.ApplyDraw();
			break;
		case ToolType.BrushPaintTool:
			ColorSwatch.AddPickerColor();
			drawing.FillColor(point, MainColor);
			drawing.ApplyDraw();
			break;
		case ToolType.EraserTool:
			drawing.ClearLine(lastPoint, point);
			drawing.ApplyDraw();
			break;
		case ToolType.MagicNewTool:
			if (!Cursor.Dragged) drawing.NewSelection(point);
			else drawing.AddToSelection(point);
			break;
		case ToolType.MagicAddTool:
			drawing.AddToSelection(point);
			break;
		case ToolType.MagicSubTool:
			drawing.SubFromSelection(point);
			break;
		}
	}

	void OnCursorClicked (Vector2 position) {
		var c = CursorToCoordinate(position);

		switch (CurrentTool) {
		case ToolType.PencilPaintTool:
			ColorSwatch.AddPickerColor();
			drawing.SetColor(c, MainColor);
			drawing.ApplyDraw();
			break;
		case ToolType.BrushPaintTool:
			ColorSwatch.AddPickerColor();
			drawing.FillColor(c, MainColor);
			drawing.ApplyDraw();
			break;
		case ToolType.EraserTool:
			drawing.ClearColor(c);
			drawing.ApplyDraw();
			break;
		case ToolType.MagicNewTool:
			drawing.NewSelection(c);
			break;
		case ToolType.MagicAddTool:
			drawing.AddToSelection(c);
			break;
		case ToolType.MagicSubTool:
			drawing.SubFromSelection(c);
			break;
		}
	}

	Short2 CursorToCoordinate (Vector2 position) {
		position -= Canvas.Rect.min;
		position *= DrawingSize.x / Canvas.Rect.width;
		return new Short2(position.x, position.y);
	}
}
