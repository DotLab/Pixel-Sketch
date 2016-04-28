using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using Uif;

public class GalleryScheduler : MonoBehaviour {
	public Hidable SceneOverlay;
	public Hidable CopyButton;
	public Hidable DeleteButton;

	public GameObject DrawingPrototype;
	public RectTransform DrawingContainer;

	public DrawingController selectedDrawing;
	public List<DrawingController> drawings = new List<DrawingController>();


	void Start () {
		for (int i = 0; i < DataLayer.GetFileCount(); i++)
			if (DataLayer.HasFile(i)) AddDrawing(DataLayer.LoadFile(i));
	}

	DrawingController AddDrawing (DrawingFile file) {
		var drawing = Instantiate(DrawingPrototype);
		drawing.transform.SetParent(DrawingContainer, false);
		var drawingController = drawing.GetComponent<DrawingController>();
		drawings.Add(drawingController);
		drawingController.Init(file);
		drawingController.OnClickedEvent += OnDrawingClicked;

		return drawingController;
	}

	public void OnDrawingClicked (DrawingController drawing) {
		CopyButton.Show();
		DeleteButton.Show();

		if (drawing == selectedDrawing) {
			App.CurrentDrawingFile = drawing.DrawingFile;
			App.LoadScene(App.DrawingSceneIndex);
		} else {
			selectedDrawing = drawing;
			foreach (var d in drawings)
				d.Selected = d == selectedDrawing;
		}
	}

	public void OnDrawingPanelClicked () {
		selectedDrawing = null;
		foreach (var d in drawings)
			d.Selected = false;

		CopyButton.Hide();
		DeleteButton.Hide();
	}

	public void OnNewButtonClicked () {
		var newDrawing = new DrawingFile();
		newDrawing.Index = DataLayer.GetFileCount();
		newDrawing.DateCreated = System.DateTime.Now.ToLongDateString();
		newDrawing.Size = new Short2(8, 8);
		newDrawing.Layers = new LayerFile[0];

		DataLayer.SaveUnsavedFile(newDrawing);

		App.CurrentDrawingFile = newDrawing;
		StopAllCoroutines();
		StartCoroutine(SceneTransitionHandler());
	}

	public void OnCopyButtonClicked () {
		if (selectedDrawing == null) return;

		var file = DataLayer.DuplicateFile(selectedDrawing.DrawingFile.Index);
		selectedDrawing = AddDrawing(file);
		foreach (var d in drawings)
			d.Selected = d == selectedDrawing;
	}

	public void OnDeleteButtonClicked () {
		if (selectedDrawing == null) return;

		DataLayer.DeleteFile(selectedDrawing.DrawingFile.Index);

		var index = drawings.IndexOf(selectedDrawing);
		drawings.Remove(selectedDrawing);
		Destroy(selectedDrawing.gameObject);
		selectedDrawing = null;
		if (drawings.Count > 0) {
			if (index < drawings.Count) {
				selectedDrawing = drawings[index];
				foreach (var d in drawings)
					d.Selected = d == selectedDrawing;
			} else if (index - 1 >= 0) {
				selectedDrawing = drawings[index - 1];
				foreach (var d in drawings)
					d.Selected = d == selectedDrawing;
			}
		}
	}

	IEnumerator SceneTransitionHandler () {
		SceneOverlay.Show();
		yield return new WaitForSeconds(0.5f);
		App.LoadScene(App.DrawingSceneIndex);
	}
}
