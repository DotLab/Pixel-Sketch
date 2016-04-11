using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections.Generic;

using Uif;

public class LayerController : RectContent, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public new class PositionComparer : IComparer<LayerController> {
		public int Compare (LayerController obj1, LayerController obj2) {
			var obj1Value = obj1.Controllable ? -obj1.TargetPosition.y : -obj1.CurrentPosition.y;
			var obj2Value = obj2.Controllable ? -obj2.TargetPosition.y : -obj2.CurrentPosition.y;
	
			return obj1Value.CompareTo(obj2Value);
		}
	}

	public delegate void OnLayerStateChange (LayerController layer);

	public event OnLayerStateChange OnLayerClickedEvent;
	public event OnLayerStateChange OnLayerPressedEvent;
	public event OnLayerStateChange OnLayerDragedEvent;
	public event OnLayerStateChange OnLayerReleasedEvent;

	public Colorable Colorable;
	public ColorSwapable ColorSwapable;

	public bool IsLayer = true;
	public bool DeleteFlag;

	Vector2 startPosition;
	Vector2 positionDelta;

	public override void Init (Transform parent, Vector2 position) {
		base.Init(parent, position);

		Colorable.SetColor(Color.clear);
		ColorSwapable.Swap(Color.white);
	}

	public void SetDeleteFlag () {
		if (DeleteFlag) return;
		DeleteFlag = true;

		ColorSwapable.Swap(Color.red);
	}

	public void ResetDeleteFlag () {
		if (!DeleteFlag) return;
		DeleteFlag = false;

		ColorSwapable.Swap(Color.white);
	}

	public void Deinit () {
		ColorSwapable.Swap(Color.clear);

		Destroy(gameObject, 0.5f);
	}

	public void OnPointerClick (PointerEventData eventData) {
		if (OnLayerClickedEvent != null) OnLayerClickedEvent(this);
	}

	public void OnBeginDrag (PointerEventData eventData) {
		Controllable = false;

		startPosition = CurrentPosition;
		positionDelta = Vector2.zero;

		if (OnLayerPressedEvent != null) OnLayerPressedEvent(this);
	}

	public void OnDrag (PointerEventData eventData) {
		positionDelta += eventData.delta * (600.0f / Screen.height);
		CurrentPosition = startPosition + positionDelta;

		if (OnLayerDragedEvent != null) OnLayerDragedEvent(this);
	}

	public void OnEndDrag (PointerEventData eventData) {
		Controllable = true;

		if (OnLayerReleasedEvent != null) OnLayerReleasedEvent(this);
	}
}
