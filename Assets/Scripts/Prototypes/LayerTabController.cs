using UnityEngine;

public class LayerTabController : MonoBehaviour {
	public const int MaxLayerCount = 7;

	public delegate void OnLayerChanged ();

	public event OnLayerChanged OnLayerChangedEvent;

	public LayerTabContentFitter TabFitter;

	public LayerController AddButton;
	public GameObject LayerPrototype;

	void Start () {
		TabFitter.Contents.Clear();
		AddButton.OnLayerDragEvent += OnLayerDrag;
		AddButton.OnLayerReleaseEvent += OnLayerRelease;
		TabFitter.Contents.Add(AddButton);

		AddLayer();

		TabFitter.Fit();
	}

	public void AddLayer () {
		if (TabFitter.Contents.Count >= MaxLayerCount) return;

		var layer = Instantiate(LayerPrototype);
		var layerController = layer.GetComponent<LayerController>();
		layerController.Trans.SetParent(transform, false);
		layerController.Trans.anchoredPosition = AddButton.Trans.anchoredPosition;
		layerController.OnLayerDragEvent += OnLayerDrag;
		layerController.OnLayerReleaseEvent += OnLayerRelease;
		TabFitter.Contents.Insert(TabFitter.Contents.IndexOf(AddButton) + 1, layerController);
		TabFitter.Fit();
		if (OnLayerChangedEvent != null) OnLayerChangedEvent();
	}

	public void OnLayerDrag (LayerController layerController, bool deleted) {
		var index = TabFitter.Contents.IndexOf(layerController);
		TabFitter.Contents.Sort(new LayerController.PositionComparer());
		if (TabFitter.Contents.IndexOf(layerController) != index) {
			TabFitter.Fit();
			if (OnLayerChangedEvent != null) OnLayerChangedEvent();
		}
	}

	public void OnLayerRelease (LayerController layerController, bool deleted) {
		if (deleted) {
			TabFitter.Contents.Remove(layerController);
		}

		TabFitter.Fit();
		if (OnLayerChangedEvent != null) OnLayerChangedEvent();
	}
}
