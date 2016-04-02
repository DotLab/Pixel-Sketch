using UnityEngine;

public class ToolBarController : MonoBehaviour {
	public enum DrawingToolType {
		MoveTool = 0x00,

		RectSelectTool = 0x10,
		EllipSelectTool = 0x11,
		PolySelectTool = 0x12,

		PencilPaintTool = 0x20,
		BrushPaintTool = 0x21,
		EraserPaintTool = 0x22,

		RectShapeTool = 0x30,
		EllipShapeTool = 0x31,
		PolyShapeTool = 0x32,
		LineShapeTool = 0x33,
	}

	public delegate void OnDrawingToolChanged (DrawingToolType newType);

	public event OnDrawingToolChanged OnDrawingToolChangedEvent;

	public DrawingToolType CurrentToolType {
		get { return currentToolType; }
		set {
			if (value == currentToolType) return;
			if (OnDrawingToolChangedEvent != null) OnDrawingToolChangedEvent(value);

			currentToolType = value;
		}
	}

	public DrawingToolType currentToolType = DrawingToolType.PencilPaintTool;
	public DrawingToolType selectToolType = DrawingToolType.RectSelectTool;
	public DrawingToolType paintToolType = DrawingToolType.PencilPaintTool;
	public DrawingToolType shapeToolType = DrawingToolType.RectShapeTool;

	[Space]
	public IconToggleGroup DrawingToolGroup;
	[Space]
	public IconToggleGroup SelectToolGroup;
	public IconToggleGroup PaintToolGroup;
	public IconToggleGroup ShapeToolGroup;
	IHidable selectToolHidable;
	IHidable paintToolHidable;
	IHidable shapeToolHidable;

	[Space]
	public GameObject SelectToolIcon;
	public GameObject PaintToolIcon;
	public GameObject ShapeToolIcon;
	ISwapable<Sprite> selectToolIconSwapable;
	ISwapable<Sprite> paintToolIconSwapable;
	ISwapable<Sprite> shapeToolIconSwapable;
	[Space]
	public Sprite[] SelectToolIcons;
	public Sprite[] PaintToolIcons;
	public Sprite[] ShapeToolIcons;

	[Space]
	public GameObject[] Bars;
	public GameObject ShowIcon;
	IHidable[] barHidables;
	IHidable showIconHidable;


	void Awake () {
		DrawingToolGroup.OnIconClickedEvent += OnDrawingIconClicked;

		SelectToolGroup.OnIconClickedEvent += OnSelectToolIconClicked;
		PaintToolGroup.OnIconClickedEvent += OnPaintToolIconClicked;
		ShapeToolGroup.OnIconClickedEvent += OnShapeToolIconClicked;
		selectToolHidable = SelectToolGroup.GetComponent<IHidable>();
		paintToolHidable = PaintToolGroup.GetComponent<IHidable>();
		shapeToolHidable = ShapeToolGroup.GetComponent<IHidable>();

		selectToolIconSwapable = SelectToolIcon.GetComponent<ISwapable<Sprite>>();
		paintToolIconSwapable = PaintToolIcon.GetComponent<ISwapable<Sprite>>();
		shapeToolIconSwapable = ShapeToolIcon.GetComponent<ISwapable<Sprite>>();

		barHidables = new IHidable[Bars.Length];
		for (int i = 0; i < Bars.Length; i++) {
			barHidables[i] = Bars[i].GetComponent<IHidable>();
		}
		showIconHidable = ShowIcon.GetComponent<IHidable>();
	}

	public void OnDrawingIconClicked (int index) {
		selectToolHidable.Hide();
		paintToolHidable.Hide();
		shapeToolHidable.Hide();

		switch (index) {
		case 0:
			CurrentToolType = DrawingToolType.MoveTool;
			break;
		case 1:
			if (((int)CurrentToolType & 0xF0) == 0x10) {
				// CurrentTool belongs to SelectTool
				selectToolHidable.Show();
			}
			CurrentToolType = selectToolType;
			break;
		case 2:
			if (((int)CurrentToolType & 0xF0) == 0x20 && CurrentToolType != DrawingToolType.EraserPaintTool) {
				// CurrentTool belongs to PaintTool
				paintToolHidable.Show();
			}
			CurrentToolType = paintToolType;
			break;
		case 3:
			CurrentToolType = DrawingToolType.EraserPaintTool;
			break;
		case 4:
			if (((int)CurrentToolType & 0xF0) == 0x30) {
				// CurrentTool belongs to ShapeTool
				shapeToolHidable.Show();
			}
			CurrentToolType = shapeToolType;
			break;
		default:
			throw new System.NotImplementedException();
		}
	}

	public void OnSelectToolIconClicked (int index) {
		switch (index) {
		case 0:
			selectToolType = DrawingToolType.RectSelectTool;
			break;
		case 1:
			selectToolType = DrawingToolType.EllipSelectTool;
			break;
		case 2:
			selectToolType = DrawingToolType.PolySelectTool;
			break;
		default:
			throw new System.NotImplementedException();
		}
		if (((int)CurrentToolType & 0xF0) == 0x10) CurrentToolType = selectToolType;
		selectToolIconSwapable.Swap(SelectToolIcons[index]);
		selectToolHidable.Hide();
	}

	public void OnPaintToolIconClicked (int index) {
		switch (index) {
		case 0:
			paintToolType = DrawingToolType.PencilPaintTool;
			break;
		case 1:
			paintToolType = DrawingToolType.BrushPaintTool;
			break;
		default:
			throw new System.NotImplementedException();
		}
		if (((int)CurrentToolType & 0xF0) == 0x20 && CurrentToolType != DrawingToolType.EraserPaintTool) CurrentToolType = paintToolType;
		paintToolIconSwapable.Swap(PaintToolIcons[index]);
		paintToolHidable.Hide();
	}

	public void OnShapeToolIconClicked (int index) {
		switch (index) {
		case 0:
			shapeToolType = DrawingToolType.RectShapeTool;
			break;
		case 1:
			shapeToolType = DrawingToolType.EllipShapeTool;
			break;
		case 2:
			shapeToolType = DrawingToolType.PolyShapeTool;
			break;
		case 3:
			shapeToolType = DrawingToolType.LineShapeTool;
			break;
		default:
			throw new System.NotImplementedException();
		}
		if (((int)CurrentToolType & 0xF0) == 0x30) CurrentToolType = shapeToolType;
		shapeToolIconSwapable.Swap(ShapeToolIcons[index]);
		shapeToolHidable.Hide();
	}

	public void OnHideButtonClicked () {
		foreach (var hidables in barHidables) {
			hidables.Hide();
		}
		showIconHidable.Show();
	}

	public void OnShowButtonClicked () {
		foreach (var hidables in barHidables) {
			hidables.Show();
		}
		showIconHidable.Hide();
	}
}
