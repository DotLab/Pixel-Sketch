using UnityEngine;

using Uif;

public class LayerTabController : MonoBehaviour {
	public delegate void OnLayerOrderChanged (LayerController[] layers);

	public event LayerController.OnLayerChange OnLayerAddEvent;
	public event LayerController.OnLayerChange OnLayerDeleteEvent;

	public event LayerController.OnLayerChange OnLayerSelectedEvent;
	public event OnLayerOrderChanged OnLayerOrderChangedEvent;

	public event LayerController.OnLayerChange OnLayerHideStateChangedEvent;
	public event LayerController.OnLayerChange OnLayerLockStateChangedEvent;

	public const int MaxLayerCount = 7;

	public LayerTabContentFitter LayerTabFitter;

	[Space]
	public LayerController AddButton;
	public GameObject LayerPrototype;

	[Space]
	public RectTransform LayerTabRect;
	public Hidable DeleteArea;
	RectTransform deleteAreaRect;

	public LayerController SelectedLayer {
		get { return _selectedLayer; }
		set {
			if (value == _selectedLayer) return;
			if (OnLayerSelectedEvent != null) OnLayerSelectedEvent(value);
			foreach (var layer in LayerTabFitter.Layers) {
				if (!layer.IsLayer) continue;
				if (layer == value) layer.OnLayerSelected();
				else layer.OnLayerDeselected();
			}

			_selectedLayer = value;
		}
	}

	LayerController _selectedLayer;

	void Awake () {
		deleteAreaRect = DeleteArea.GetComponent<RectTransform>();
	}

	void Start () {
		LayerTabFitter.Layers.Clear();
		AddButton.IsLayer = false;
		AddButton.OnLayerClickedEvent += OnLayerClicked;
		AddButton.OnLayerPressedEvent += OnLayerPressed;
		AddButton.OnLayerDragedEvent += OnLayerDraged;
		AddButton.OnLayerReleasedEvent += OnLayerReleased;
		LayerTabFitter.Layers.Add(AddButton);

		var newLayer = AddLayer();
		if (newLayer != null) SelectedLayer = newLayer;

		LayerTabFitter.Fit();
	}

	public LayerController AddLayer () {
		if (LayerTabFitter.Layers.Count >= MaxLayerCount) return null;

		var layer = Instantiate(LayerPrototype);
		var layerController = layer.GetComponent<LayerController>();
		layerController.OnLayerClickedEvent += OnLayerClicked;
		layerController.OnLayerPressedEvent += OnLayerPressed;
		layerController.OnLayerDragedEvent += OnLayerDraged;
		layerController.OnLayerReleasedEvent += OnLayerReleased;
		layerController.OnLayerHideStateChangedEvent += OnLayerHideStateChangedEvent;
		layerController.OnLayerLockStateChangedEvent += OnLayerLockStateChangedEvent;
		layerController.Init(LayerTabRect, AddButton.CurrentPosition);

		LayerTabFitter.Layers.Insert(LayerTabFitter.Layers.IndexOf(AddButton) + 1, layerController);
		LayerTabFitter.Fit();

		if (OnLayerAddEvent != null) OnLayerAddEvent(layerController);
		if (OnLayerOrderChangedEvent != null) OnLayerOrderChangedEvent(LayerTabFitter.Layers.ToArray());

		return layerController;
	}

	#region Layer Controller Callback

	public void OnLayerClicked (LayerController layer) {
		if (!layer.DeleteFlag && layer.Controllable) {
			if (!layer.IsLayer) {
				var newLayer = AddLayer();
				if (newLayer != null) SelectedLayer = newLayer;
			} else SelectedLayer = layer;
		}
	}

	public void OnLayerPressed (LayerController layer) {
		DeleteArea.Show();
	}

	public void OnLayerDraged (LayerController layer) {
		if (IsDeletable(layer)) layer.SetDeleteFlag();
		else layer.ResetDeleteFlag();

		var index = LayerTabFitter.Layers.IndexOf(layer);
		LayerTabFitter.Layers.Sort(new LayerController.PositionComparer());
		if (LayerTabFitter.Layers.IndexOf(layer) != index) {
			LayerTabFitter.Fit();
			if (OnLayerOrderChangedEvent != null) OnLayerOrderChangedEvent(LayerTabFitter.Layers.ToArray());
		}
	}

	public void OnLayerReleased (LayerController layer) {
		if (IsDeletable(layer)) layer.SetDeleteFlag();
		else layer.ResetDeleteFlag();

		DeleteArea.Hide();

		if (layer.DeleteFlag) {
			if (_selectedLayer) {
				_selectedLayer = null;
			}
			LayerTabFitter.Layers.Remove(layer);
			layer.Deinit();
			if (OnLayerDeleteEvent != null) OnLayerDeleteEvent(layer);
		}

		LayerTabFitter.Fit();
		if (OnLayerOrderChangedEvent != null) OnLayerOrderChangedEvent(LayerTabFitter.Layers.ToArray());
	}

	#endregion

	bool IsDeletable (LayerController layer) {
		return layer.IsLayer && Vector2.Distance(layer.CurrentPosition, deleteAreaRect.anchoredPosition) < 100;
	}
}
