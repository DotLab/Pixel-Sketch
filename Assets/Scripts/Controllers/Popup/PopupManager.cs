using UnityEngine;

using System.Collections.Generic;

using Uif;

public class PopupManager : MonoBehaviour {
	public static PopupManager Instance {
		get { return instance; }
	}

	static PopupManager instance;

	public PopupOverlayController Overlay;

	readonly LinkedList<Hidable> popupHidables = new LinkedList<Hidable>();


	void Awake () {
		instance = this;

		Overlay.OnOverlayClickcedEvent += OnOverlayChlicked;
	}

	void OnOverlayChlicked () {
		while (popupHidables.Count > 0) {
			popupHidables.Last.Value.Hide();
			popupHidables.RemoveLast();
		}
		Overlay.HideOverlay();
	}

	public void ShowPopup (Hidable popupHidable) {
		popupHidable.Show();
		popupHidables.AddFirst(popupHidable);
		Overlay.ShowOverlay();
	}
}
