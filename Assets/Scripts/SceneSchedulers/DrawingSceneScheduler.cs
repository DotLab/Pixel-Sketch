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

	public ColorSwatchController ColorSwatch;

	[Space]
	public Color MainColor;
	public Short2 DrawingSize;
	public ToolType CurrentTool;

	Drawing drawing;


	void Awake () {
		instance = this;
		drawing = new Drawing(Canvas, DrawingSize);

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

	void Start () {
		drawing.DrawPoly(new [] { new Short2(2, 3), new Short2(12, 31), new Short2(19, 6) }, Color.red);
		drawing.ApplyDraw();
	}

	public void ResizeDrawing (Short2 size) {
		DrawingSize = size;
		drawing.ResizeDrawing(size);
	}

	void OnCursorMoved (Vector2 position) {
	}

	void OnCursorDragged (Vector2 position) {
		var lastPoint = CursorPositionToCoordinate(Cursor.LastPosition);
		var point = CursorPositionToCoordinate(position);

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
			else drawing.AddSelection(point);
			break;
		case ToolType.MagicAddTool:
			drawing.AddSelection(point);
			break;
		case ToolType.MagicSubTool:
			drawing.SubSelection(point);
			break;
		}
	}

	void OnCursorClicked (Vector2 position) {
		var c = CursorPositionToCoordinate(position);

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
			drawing.AddSelection(c);
			break;
		case ToolType.MagicSubTool:
			drawing.SubSelection(c);
			break;
		}
	}

	Short2 CursorPositionToCoordinate (Vector2 position) {
		var drawingSize = DrawingSceneScheduler.Instance.DrawingSize;
		position -= Canvas.CanvasRect.rect.min;
		position *= drawingSize.x / Canvas.CanvasRect.rect.width;
		return new Short2(position.x, position.y);
	}

	void OnToolChanged (ToolType toolType) {
		CurrentTool = toolType;

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
		else if (toolType == ToolType.TransTool)
			Cursor.HideCursor();
	}
}
