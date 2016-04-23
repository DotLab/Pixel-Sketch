using UnityEngine;
using UnityEngine.EventSystems;

public class CursorTouchHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
	public CursorController Cursor;

	[Space]
	public int MainTouchId;
	public bool MainTouchPressed;

	public int AsistTouchId;
	public bool AsistTouchPressed;

	public void OnPointerDown (PointerEventData eventData) {
		if (!MainTouchPressed) {
			MainTouchPressed = true;
			MainTouchId = eventData.pointerId;
		} else if (!AsistTouchPressed && eventData.pointerId != MainTouchId) {
			AsistTouchPressed = true;
			AsistTouchId = eventData.pointerId;
			Cursor.PressCursor();
		}
	}

	public void OnDrag (PointerEventData eventData) {
		var delta = (eventData).delta * (600.0f / Screen.height);
		if (MainTouchPressed && eventData.pointerId == MainTouchId) {
			Cursor.MoveCursor(delta * 2);
		}

		if (AsistTouchPressed && eventData.pointerId == AsistTouchId) {
			Cursor.NudgeCursor(delta);
		}
	}

	public void OnPointerUp (PointerEventData eventData) {
		if (eventData.pointerId == MainTouchId) {
			MainTouchPressed = false;
		} else if (eventData.pointerId == AsistTouchId) {
			AsistTouchPressed = false;
			Cursor.ReleaseCursor();
		}
	}
}
