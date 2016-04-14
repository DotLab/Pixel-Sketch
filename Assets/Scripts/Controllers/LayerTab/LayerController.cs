using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections.Generic;

using Uif;

public class LayerController : RectContent {
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

	public ColorSwapable BaseColor;
	public ColorSwapable HideIconColor;
	public ColorSwapable LockIconColor;
	public ColorSwapable ThumbnailColor;
	RawImage thumbnailImage;
	AspectRatioFitter thumbnailFitter;

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
		if (BaseColor == null) BaseColor = GetComponent<ColorSwapable>();
	}

	public override void Awake () {
		base.Awake();

		if (IsLayer) {
			thumbnailImage = ThumbnailColor.GetComponent<RawImage>();
			thumbnailFitter = ThumbnailColor.GetComponent<AspectRatioFitter>();
		}
	}

	public void SetThumbnail (Texture texture) {
		if (thumbnailImage.texture != null) DestroyImmediate(thumbnailImage.texture);
		thumbnailImage.texture = texture;
		thumbnailFitter.aspectRatio = (float)texture.width / texture.height;
	}

	#region Layer UI Logic

	public void OnLayerSelected () {
		BaseColor.Swap(SelectedColor);
	}

	public void OnLayerDeselected () {
		BaseColor.Swap(NormalColor);
	}

	public void OnHideButtonClicked () {
		Hided = !Hided;
		HideIconColor.Swap(Hided ? SelectedColor : NormalColor);
		if (OnLayerHideStateChangedEvent != null) OnLayerHideStateChangedEvent(this);
	}

	public void OnLockButtonClicked () {
		Locked = !Locked;
		LockIconColor.Swap(Locked ? SelectedColor : NormalColor);
		if (OnLayerLockStateChangedEvent != null) OnLayerLockStateChangedEvent(this);
	}

	public override void Init (Transform parent, Vector2 position) {
		base.Init(parent, position);

		BaseColor.SilentSwap(Color.clear);
		HideIconColor.SilentSwap(Color.clear);
		LockIconColor.SilentSwap(Color.clear);
		ThumbnailColor.SilentSwap(Color.clear);

		BaseColor.Swap(Color.white);
		HideIconColor.Swap(Color.white);
		LockIconColor.Swap(Color.white);
		ThumbnailColor.Swap(Color.white);
	}

	public void Deinit () {
		BaseColor.Swap(Color.clear);
		HideIconColor.Swap(Color.clear);
		LockIconColor.Swap(Color.clear);
		ThumbnailColor.Swap(Color.clear);

		Destroy(gameObject, 0.5f);
	}

	public void SetDeleteFlag () {
		if (DeleteFlag) return;
		DeleteFlag = true;

		BaseColor.Swap(Color.red);
		HideIconColor.Swap(Color.red);
		LockIconColor.Swap(Color.red);
		ThumbnailColor.Swap(Color.red);
	}

	public void ResetDeleteFlag () {
		if (!DeleteFlag) return;
		DeleteFlag = false;

		BaseColor.Swap(Color.white);
		HideIconColor.Swap(Hided ? SelectedColor : NormalColor);
		LockIconColor.Swap(Locked ? SelectedColor : NormalColor);
		ThumbnailColor.Swap(Color.white);
	}

	public void OnPointerClick (BaseEventData eventData) {
		if (OnLayerClickedEvent != null) OnLayerClickedEvent(this);
	}

	public void OnBeginDrag (BaseEventData eventData) {
		Controllable = false;

		startPosition = CurrentPosition;
		positionDelta = Vector2.zero;

		if (OnLayerPressedEvent != null) OnLayerPressedEvent(this);
	}

	public void OnDrag (BaseEventData eventData) {
		positionDelta += ((PointerEventData)eventData).delta * (600.0f / Screen.height);
		CurrentPosition = startPosition + positionDelta;

		if (OnLayerDragedEvent != null) OnLayerDragedEvent(this);
	}

	public void OnEndDrag (BaseEventData eventData) {
		Controllable = true;

		if (OnLayerReleasedEvent != null) OnLayerReleasedEvent(this);
	}

	#endregion
}
