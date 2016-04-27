using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionTouchHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
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

	bool active;

	[Header("Main Touch")]
	public int MainTouchIndex;
	public bool MainTouchPressed;
	public Vector2 MainTouchPosition;

	[Header("Asist Touch")]
	public int AsistTouchIndex;
	public bool AsistTouchPressed;
	public Vector2 AsistTouchPosition;

	[Header("Start Param")]
	public float StartDistance;
	public Vector3 StartDirection;
	public Vector2 StartMidPoint;

	[Header("Param Cache")]
	public float StartScale;
	public float StartRotation;
	public Vector2 StartPosition;


	public void OnBeginDrag (PointerEventData eventData) {
		if (!active) return;

		if (!MainTouchPressed) {
			MainTouchPressed = true;
			MainTouchIndex = eventData.pointerId;
			MainTouchPosition = eventData.position;
		} else if (!AsistTouchPressed) {
			AsistTouchPressed = true;
			AsistTouchIndex = eventData.pointerId;
			AsistTouchPosition = eventData.position;

			StartScale = SelectionController.Selection.Scale;
			StartRotation = SelectionController.Selection.Rotation;
			StartPosition = SelectionController.Selection.Position;

			StartDistance = Vector2.Distance(MainTouchPosition, AsistTouchPosition);
			StartDirection = AsistTouchPosition - MainTouchPosition;
			StartMidPoint = (AsistTouchPosition + MainTouchPosition) * 0.5f;
		}
	}

	public void OnDrag (PointerEventData eventData) {
		if (!active) return;

		if (MainTouchPressed && eventData.pointerId == MainTouchIndex) {
			MainTouchPosition = eventData.position;

			if (!AsistTouchPressed)
				SelectionController.Selection.Position += (Vector3)eventData.delta * DrawingScheduler.Ui2Coordinate;
		} else if (AsistTouchPressed && eventData.pointerId == AsistTouchIndex) {
			AsistTouchPosition = eventData.position;
		}

		if (MainTouchPressed && AsistTouchPressed) {
			var currentDistance = Vector2.Distance(MainTouchPosition, AsistTouchPosition);
			var currentDirection = AsistTouchPosition - MainTouchPosition;
			var currentMidPoint = (AsistTouchPosition + MainTouchPosition) * 0.5f;

			SelectionController.Selection.Scale = StartScale * currentDistance / StartDistance;
			SelectionController.Selection.Rotation = StartRotation + (Mathf.Atan2(currentDirection.y, currentDirection.x) - Mathf.Atan2(StartDirection.y, StartDirection.x)) * Mathf.Rad2Deg;
			SelectionController.Selection.Position = StartPosition + (currentMidPoint - StartMidPoint) * DrawingScheduler.Ui2Coordinate;
		}
	}

	public void OnEndDrag (PointerEventData eventData) {
		if (!active) return;

		if (eventData.pointerId == MainTouchIndex) {
			MainTouchPressed = false;
		} else if (eventData.pointerId == AsistTouchIndex) {
			AsistTouchPressed = false;
		}
	}
}
