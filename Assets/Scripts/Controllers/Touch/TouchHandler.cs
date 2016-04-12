using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public delegate void OnPositionUpdate (Vector2 deltaPosition);

	public delegate void OnPositionChanged (Vector2 position);

	public event OnPositionChanged OnMainTouchTapedEvent;
	public event OnPositionChanged OnMainTouchPressedEvent;
	public event OnPositionUpdate OnMainTouchDragedEvent;
	public event OnPositionChanged OnMainTouchReleasedEvent;

	public event OnPositionChanged OnAsistTouchTapedEvent;
	public event OnPositionChanged OnAsistTouchPressedEvent;
	public event OnPositionUpdate OnAsistTouchDragedEvent;
	public event OnPositionChanged OnAsistTouchReleasedEvent;

	public int MainTouchIndex;
	public int AsistTouchIndex;

	public bool MainTouchActive;
	public bool AsistTouchActive;

	public void OnPointerClick (PointerEventData eventData) {
		if (!MainTouchActive) {
			if (OnMainTouchTapedEvent != null) OnMainTouchTapedEvent(eventData.position);
		} else if (!AsistTouchActive && eventData.pointerId != MainTouchIndex) {
			if (OnAsistTouchTapedEvent != null) OnAsistTouchTapedEvent(eventData.position);
		}
	}

	public void OnBeginDrag (PointerEventData eventData) {
		if (!MainTouchActive) {
			MainTouchIndex = eventData.pointerId;
			MainTouchActive = true;
			if (OnMainTouchPressedEvent != null) OnMainTouchPressedEvent(eventData.position);
		} else if (!AsistTouchActive) {
			AsistTouchIndex = eventData.pointerId;
			AsistTouchActive = true;
			if (OnAsistTouchPressedEvent != null) OnAsistTouchPressedEvent(eventData.position);
		}
	}

	public void OnDrag (PointerEventData eventData) {
		if (eventData.pointerId == MainTouchIndex) {
			if (OnMainTouchDragedEvent != null) OnMainTouchDragedEvent(eventData.delta);
		} else if (eventData.pointerId == AsistTouchIndex) {
			if (OnAsistTouchDragedEvent != null) OnAsistTouchDragedEvent(eventData.delta);
		}
	}

	public void OnEndDrag (PointerEventData eventData) {
		if (eventData.pointerId == MainTouchIndex) {
			if (AsistTouchActive) {
				AsistTouchActive = false;
				MainTouchIndex = AsistTouchIndex;
				if (OnAsistTouchReleasedEvent != null) OnAsistTouchReleasedEvent(eventData.position);
			} else {
				MainTouchActive = false;
				if (OnMainTouchReleasedEvent != null) OnMainTouchReleasedEvent(eventData.position);
			}
		} else if (eventData.pointerId == AsistTouchIndex) {
			AsistTouchActive = false;
			if (OnAsistTouchReleasedEvent != null) OnAsistTouchReleasedEvent(eventData.position);
		}
	}
}
