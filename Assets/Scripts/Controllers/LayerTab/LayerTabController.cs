using UnityEngine;

using Uif;

public class LayerTabController : MonoBehaviour {
	public const int MaxLayerCount = 7;

	public LayerTabContentFitter LayerTabFitter;

	[Space]
	public LayerController AddButton;
	public GameObject LayerPrototype;

	[Space]
	public Hidable DeleteArea;
	RectTransform deleteAreaRect;

	[Space]
	public LayerController SelectedLayer;

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

		AddLayer();

		LayerTabFitter.Fit();
	}

	public void AddLayer () {
		if (LayerTabFitter.Layers.Count >= MaxLayerCount) return;

		var layer = Instantiate(LayerPrototype);
		var layerController = layer.GetComponent<LayerController>();
		layerController.OnLayerClickedEvent += OnLayerClicked;
		layerController.OnLayerPressedEvent += OnLayerPressed;
		layerController.OnLayerDragedEvent += OnLayerDraged;
		layerController.OnLayerReleasedEvent += OnLayerReleased;
		layerController.Init(transform, AddButton.CurrentPosition);

		LayerTabFitter.Layers.Insert(LayerTabFitter.Layers.IndexOf(AddButton) + 1, layerController);
		LayerTabFitter.Fit();
	}



	#region Layer Controller Callback

	public void OnLayerClicked (LayerController layer) {
		if (!layer.DeleteFlag && layer.Controllable) {
			if (!layer.IsLayer) AddLayer();
			SelectedLayer = layer;
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
		}
	}

	public void OnLayerReleased (LayerController layer) {
		if (IsDeletable(layer)) layer.SetDeleteFlag();
		else layer.ResetDeleteFlag();

		DeleteArea.Hide();

		if (layer.DeleteFlag) {
			LayerTabFitter.Layers.Remove(layer);
			layer.Deinit();
		}

		LayerTabFitter.Fit();
	}

	#endregion

	bool IsDeletable (LayerController layer) {
		return layer.IsLayer && Vector2.Distance(layer.CurrentPosition, deleteAreaRect.anchoredPosition) < 100;
	}
}
