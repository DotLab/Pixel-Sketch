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

	public delegate void OnLayerChange (LayerController layer);

	public event OnLayerChange OnLayerClickedEvent;
	public event OnLayerChange OnLayerPressedEvent;
	public event OnLayerChange OnLayerDragedEvent;
	public event OnLayerChange OnLayerReleasedEvent;

	public event OnLayerChange OnLayerHideStateChangedEvent;
	public event OnLayerChange OnLayerLockStateChangedEvent;


	public ColorSwapable ColorSwapable;

	public ColorSwapable HideIcon;
	public ColorSwapable LockIcon;

	public Color NormalColor = Color.white;
	public Color SelectedColor = Color.cyan;

	public int Index;
	public bool IsLayer = true;
	public bool DeleteFlag;

	public bool Hided;
	public bool Locked;

	Vector2 startPosition;
	Vector2 positionDelta;

	void OnValidate () {
		if (ColorSwapable == null) ColorSwapable = GetComponent<ColorSwapable>();
	}

	#region Layer UI Logic

	public void OnHideButtonClicked () {
		Hided = !Hided;
		HideIcon.Swap(Hided ? SelectedColor : NormalColor);
		if (OnLayerHideStateChangedEvent != null) OnLayerHideStateChangedEvent(this);
	}

	public void OnLockButtonClicked () {
		Locked = !Locked;
		LockIcon.Swap(Locked ? SelectedColor : NormalColor);
		if (OnLayerLockStateChangedEvent != null) OnLayerLockStateChangedEvent(this);
	}

	public override void Init (Transform parent, Vector2 position) {
		base.Init(parent, position);

		ColorSwapable.SilentSwap(Color.clear);
		HideIcon.SilentSwap(Color.clear);
		LockIcon.SilentSwap(Color.clear);

		ColorSwapable.Swap(Color.white);
		HideIcon.Swap(Color.white);
		LockIcon.Swap(Color.white);
	}

	public void Deinit () {
		ColorSwapable.Swap(Color.clear);
		HideIcon.Swap(Color.clear);
		LockIcon.Swap(Color.clear);

		Destroy(gameObject, 0.5f);
	}

	public void SetDeleteFlag () {
		if (DeleteFlag) return;
		DeleteFlag = true;

		ColorSwapable.Swap(Color.red);
		HideIcon.Swap(Color.red);
		LockIcon.Swap(Color.red);
	}

	public void ResetDeleteFlag () {
		if (!DeleteFlag) return;
		DeleteFlag = false;

		ColorSwapable.Swap(Color.white);
		HideIcon.Swap(Hided ? SelectedColor : NormalColor);
		LockIcon.Swap(Locked ? SelectedColor : NormalColor);
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

	#endregion
}
