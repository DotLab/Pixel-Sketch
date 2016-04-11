using UnityEngine;
using Uif;

public class ToolBarController : MonoBehaviour {
	public delegate void OnDrawingToolChanged (ToolType newToolType);

	public event OnDrawingToolChanged OnDrawingToolChangedEvent;

	public ToolType CurrentToolType {
		get { return _currentToolType; }
		set {
			if (value == _currentToolType) return;
			if (OnDrawingToolChangedEvent != null) OnDrawingToolChangedEvent(value);

			_currentToolType = value;
		}
	}

	public ToolType _currentToolType = ToolType.PencilPaintTool;

	public ToolType SelectToolType = ToolType.RectSelectTool;
	public ToolType PaintToolType = ToolType.PencilPaintTool;
	public ToolType ShapeToolType = ToolType.RectShapeTool;

	[Space]
	public IconToggleGroup ToolGroup;
	[Space]
	public IconToggleGroup SelectToolGroup;
	public IconToggleGroup PaintToolGroup;
	public IconToggleGroup ShapeToolGroup;
	IHidable selectToolHidable;
	IHidable paintToolHidable;
	IHidable shapeToolHidable;

	[Space]
	public SpriteSwapable SelectToolIcon;
	public SpriteSwapable PaintToolIcon;
	public SpriteSwapable ShapeToolIcon;
	[Space]
	public Sprite[] SelectToolIcons;
	public Sprite[] PaintToolIcons;
	public Sprite[] ShapeToolIcons;

	[Space]
	public Hidable ShowIcon;
	public Hidable[] HidableTabs;


	void Awake () {
		ToolGroup.OnIconClickedEvent += OnDrawingIconClicked;

		SelectToolGroup.OnIconClickedEvent += OnSelectToolIconClicked;
		PaintToolGroup.OnIconClickedEvent += OnPaintToolIconClicked;
		ShapeToolGroup.OnIconClickedEvent += OnShapeToolIconClicked;

		selectToolHidable = SelectToolGroup.GetComponent<IHidable>();
		paintToolHidable = PaintToolGroup.GetComponent<IHidable>();
		shapeToolHidable = ShapeToolGroup.GetComponent<IHidable>();
	}

	public void OnDrawingIconClicked (int index) {
		selectToolHidable.Hide();
		paintToolHidable.Hide();
		shapeToolHidable.Hide();

		switch (index) {
		case 0:
			CurrentToolType = ToolType.MoveTool;
			break;
		case 1:
			if (((int)CurrentToolType & (int)ToolType.ToolFamilyMask) == (int)ToolType.SelectTool) {
				// CurrentTool belongs to SelectTool
				selectToolHidable.Show();
			}
			CurrentToolType = SelectToolType;
			break;
		case 2:
			if (((int)CurrentToolType & (int)ToolType.ToolFamilyMask) == (int)ToolType.PaintTool) {
				// CurrentTool belongs to PaintTool
				paintToolHidable.Show();
			}
			CurrentToolType = PaintToolType;
			break;
		case 3:
			CurrentToolType = ToolType.EraserTool;
			break;
		case 4:
			if (((int)CurrentToolType & (int)ToolType.ToolFamilyMask) == (int)ToolType.ShapeTool) {
				// CurrentTool belongs to ShapeTool
				shapeToolHidable.Show();
			}
			CurrentToolType = ShapeToolType;
			break;
		default:
			throw new System.NotImplementedException();
		}
	}

	public void OnSelectToolIconClicked (int index) {
		switch (index) {
		case 0:
			SelectToolType = ToolType.RectSelectTool;
			break;
		case 1:
			SelectToolType = ToolType.EllipSelectTool;
			break;
		case 2:
			SelectToolType = ToolType.PolySelectTool;
			break;
		default:
			throw new System.NotImplementedException();
		}
		CurrentToolType = SelectToolType;
		SelectToolIcon.Swap(SelectToolIcons[index]);
		selectToolHidable.Hide();
	}

	public void OnPaintToolIconClicked (int index) {
		switch (index) {
		case 0:
			PaintToolType = ToolType.PencilPaintTool;
			break;
		case 1:
			PaintToolType = ToolType.BrushPaintTool;
			break;
		default:
			throw new System.NotImplementedException();
		}
		CurrentToolType = PaintToolType;
		PaintToolIcon.Swap(PaintToolIcons[index]);
		paintToolHidable.Hide();
	}

	public void OnShapeToolIconClicked (int index) {
		switch (index) {
		case 0:
			ShapeToolType = ToolType.RectShapeTool;
			break;
		case 1:
			ShapeToolType = ToolType.EllipShapeTool;
			break;
		case 2:
			ShapeToolType = ToolType.PolyShapeTool;
			break;
		case 3:
			ShapeToolType = ToolType.LineShapeTool;
			break;
		default:
			throw new System.NotImplementedException();
		}
		CurrentToolType = ShapeToolType;
		ShapeToolIcon.Swap(ShapeToolIcons[index]);
		shapeToolHidable.Hide();
	}

	public void OnHideButtonClicked () {
		selectToolHidable.Hide();
		paintToolHidable.Hide();
		shapeToolHidable.Hide();
		foreach (var bar in HidableTabs) {
			bar.Hide();
		}
		ShowIcon.Show();
	}

	public void OnShowButtonClicked () {
		foreach (var bar in HidableTabs) {
			bar.Show();
		}
		ShowIcon.Hide();
	}
}
