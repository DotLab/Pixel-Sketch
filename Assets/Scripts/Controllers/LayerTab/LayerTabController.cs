using UnityEngine;

using System.Collections.Generic;

using Uif;

public class LayerTabController : MonoBehaviour {
	public static LayerTabController Instance;

	public event LayerController.OnStateChanged OnLayerAddedEvent;
	public event LayerController.OnStateChanged OnLayerDeletedEvent;
	public event LayerController.OnStateChanged OnLayerSelectedEvent;
	public event LayerController.OnStateChanged OnLayerChangedEvent;

	public const int MaxLayerCount = 7;

	public RectTransform LayerTabRect;
	public RectContentFitter LayerTabFitter;

	[Space]
	public GameObject LayerPrototype;

	[Space]
	public Hidable DeleteIconHidable;
	public RectTransform DeleteIconRect;

	public LayerController SelectedLayer {
		get { return selectedLayer; }
		set {
			if (value == selectedLayer || value == null) return;
			selectedLayer = value;

			foreach (var layer in layers)
				layer.Selected = layer == selectedLayer;

			if (OnLayerSelectedEvent != null) OnLayerSelectedEvent(selectedLayer);
		}
	}

	LayerController selectedLayer;
	List<LayerController> layers = new List<LayerController>();


	void OnValidate () {
		if (LayerTabRect != null)
			LayerTabFitter = LayerTabRect.GetComponent<RectContentFitter>();
		if (DeleteIconHidable != null)
			DeleteIconRect = DeleteIconHidable.GetComponent<RectTransform>();
	}

	void Awake () {
		Instance = this;
	}

	public LayerController[] AddLayers (int count) {
		for (int i = 0; i < count; i++)
			AddLayer();
		
		return layers.ToArray();
	}

	public LayerController AddLayer () {
		if (layers.Count >= MaxLayerCount) return null;

		var layer = Instantiate(LayerPrototype);
		var layerController = layer.GetComponent<LayerController>();
		layerController.OnClickedEvent += OnLayerClicked;
		layerController.OnPressedEvent += OnLayerPressed;
		layerController.OnDragedEvent += OnLayerDraged;
		layerController.OnReleasedEvent += OnLayerReleased;
		layerController.OnChangedEvent += OnLayerChanged;
		layerController.Init(LayerTabRect, Vector2.zero);

		layers.Insert(0, layerController);
		LayerTabFitter.Fit(layers.ToArray());

		if (OnLayerAddedEvent != null) OnLayerAddedEvent(layerController);

		return layerController;
	}

	public void OnAddButtonClicked () {
		SelectedLayer = AddLayer();
	}

	public void OnLayerClicked (LayerController layer) {
		SelectedLayer = layer;
	}

	public void OnLayerPressed (LayerController layer) {
		DeleteIconHidable.Show();
	}

	public void OnLayerDraged (LayerController layer) {
		layer.DeleteFlag = Vector2.Distance(layer.CurrentPosition, DeleteIconRect.anchoredPosition) < 100;

		var index = layers.IndexOf(layer);
		layers.Sort(new LayerController.VerticalPositionComparer());
		if (layers.IndexOf(layer) != index)
			LayerTabFitter.Fit(layers.ToArray());
	}

	public void OnLayerReleased (LayerController layer) {
		DeleteIconHidable.Hide();

		if (layer.DeleteFlag) {
			layers.Remove(layer);

			if (OnLayerDeletedEvent != null) OnLayerDeletedEvent(layer);
		}

		LayerTabFitter.Fit(layers.ToArray());

		if (OnLayerChangedEvent != null) OnLayerChangedEvent(layer);
	}

	public void OnLayerChanged (LayerController layer) {
		if (OnLayerChangedEvent != null) OnLayerChangedEvent(layer);
	}
}
