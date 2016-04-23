using UnityEngine;
using Uif;

public class ToolBarController : MonoBehaviour {
	public delegate void OnToolChanged (ToolType toolType);

	public event OnToolChanged OnToolChangedEvent;

	public ToolType CurrentToolType {
		get { return _currentToolType; }
		set {
			if (value == _currentToolType) return;
			_currentToolType = value;

			if (OnToolChangedEvent != null) OnToolChangedEvent(_currentToolType);
		}
	}

	ToolType _currentToolType = ToolType.None;

	[Space]
	public IconToggleGroup ToolGroup;

	[Space]
	public Hidable MagicToolHidable;
	public IconToggleGroup MagicToolGroup;
	public SpriteSwapable MagicToolIcon;
	public Sprite[] MagicToolIcons;

	public ToolType MagicToolType = ToolType.MagicNewTool;

	[Space]
	public Hidable ShowIcon;
	public Hidable ConfigPopup;
	public Hidable[] HidableTabs;


	void Awake () {
		ToolGroup.OnIconClickedEvent += OnDrawingIconClicked;

		MagicToolGroup.OnIconClickedEvent += OnMagicToolIconClicked;
	}

	void Start () {
		CurrentToolType = ToolType.PencilPaintTool;
	}

	public void OnDrawingIconClicked (int index) {
		MagicToolHidable.Hide();

		switch (index) {
		case 0:
			CurrentToolType = ToolType.PencilPaintTool;
			break;
		case 1:
			CurrentToolType = ToolType.BrushPaintTool;
			break;
		case 2:
			CurrentToolType = ToolType.EraserTool;
			break;
		case 3:
			if (((int)CurrentToolType & (int)ToolType.ToolFamilyMask) == (int)ToolType.MagicTool)
				MagicToolHidable.Show();
			CurrentToolType = MagicToolType;
			break;
		case 4:
			CurrentToolType = ToolType.TransTool;
			break;
		default:
			throw new System.NotImplementedException();
		}
	}

	public void OnMagicToolIconClicked (int index) {
		switch (index) {
		case 0:
			MagicToolType = ToolType.MagicNewTool;
			break;
		case 1:
			MagicToolType = ToolType.MagicAddTool;
			break;
		case 2:
			MagicToolType = ToolType.MagicSubTool;
			break;
		default:
			throw new System.NotImplementedException();
		}

		CurrentToolType = MagicToolType;
		MagicToolIcon.Swap(MagicToolIcons[index]);
		MagicToolHidable.Hide();
	}

	public void OnHideButtonClicked () {
		PackAll();

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

	public void OnConfigButtonClicked () {
		PopupManager.Instance.ShowPopup(ConfigPopup);
	}

	public void PackAll () {
		MagicToolHidable.Hide();
	}
}
