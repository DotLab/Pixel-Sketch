using UnityEngine;
using UnityEngine.EventSystems;

public class CursorTouchHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
	public bool Active {
		get { return active; }
		set {
			if (!value) {
				MainTouchPressed = false;
				AsistTouchPressed = false;
			}

			active = value;
		}
	}

	bool active = true;

	public CursorController CursorController;

	[Header("Touch")]
	public int MainTouchId;
	public bool MainTouchPressed;

	public int AsistTouchId;
	public bool AsistTouchPressed;

	public void OnPointerDown (PointerEventData eventData) {
		if (!active) return;

		if (!MainTouchPressed) {
			MainTouchPressed = true;
			MainTouchId = eventData.pointerId;
		} else if (!AsistTouchPressed && eventData.pointerId != MainTouchId) {
			AsistTouchPressed = true;
			AsistTouchId = eventData.pointerId;
			CursorController.PressCursor();
		}
	}

	public void OnDrag (PointerEventData eventData) {
		if (!active) return;

		var delta = (eventData).delta * (600.0f / Screen.height);
		if (MainTouchPressed && eventData.pointerId == MainTouchId) {
			CursorController.MoveCursor(delta * 2);
		}

		if (AsistTouchPressed && eventData.pointerId == AsistTouchId) {
			CursorController.NudgeCursor(delta);
		}
	}

	public void OnPointerUp (PointerEventData eventData) {
		if (!active) return;

		if (eventData.pointerId == MainTouchId) {
			MainTouchPressed = false;
		} else if (eventData.pointerId == AsistTouchId) {
			AsistTouchPressed = false;
			CursorController.ReleaseCursor();
		}
	}
}
