using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
	public CursorController Cursor;

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
		if (MainTouchPressed && eventData.pointerId == MainTouchId) {
			Cursor.MoveCursor(eventData.delta);
		}

		if (AsistTouchPressed && eventData.pointerId == AsistTouchId) {
			Cursor.NudgeCursor(eventData.delta);
		}
	}

	public void OnPointerUp (PointerEventData eventData) {
		//		DebugConsole.Log("OnPointerUp " + eventData.pointerId);
		if (eventData.pointerId == MainTouchId) {
			MainTouchPressed = false;
		} else if (eventData.pointerId == AsistTouchId) {
			AsistTouchPressed = false;
			Cursor.ReleaseCursor();
		}
	}
}
