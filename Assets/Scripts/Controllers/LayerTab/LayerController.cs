using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections.Generic;

using Uif;

public class LayerController : RectContent {
	public new class VerticalPositionComparer : RectContent.VerticalPositionComparer, IComparer<LayerController> {
		public int Compare (LayerController obj1, LayerController obj2) {
			return base.Compare(obj1, obj2);
		}
	}

	public delegate void OnStateChanged (LayerController layer);

	public event OnStateChanged OnClickedEvent;
	public event OnStateChanged OnPressedEvent;
	public event OnStateChanged OnDragedEvent;
	public event OnStateChanged OnReleasedEvent;
	public event OnStateChanged OnChangedEvent;

	public ColorSwapable BaseColor;
	public ColorSwapable HideIconColor;
	public ColorSwapable LockIconColor;

	[Space]
	public RawImage ThumbnailImage;
	public ColorSwapable ThumbnailColor;
	public AspectRatioFitter ThumbnailFitter;

	[Space]
	public Color NormalColor = Color.white;
	public Color SelectedColor = Color.cyan;

	[Space]
	public bool Hided;
	public bool Locked;

	public bool Selected {
		get { return Selected; }
		set {
			if (value == selected) return;
			selected = value;

			BaseColor.Swap(selected ? SelectedColor : NormalColor);
		}
	}

	public bool DeleteFlag {
		get { return deleteFlag; }
		set {
			if (value == deleteFlag) return;
			deleteFlag = value;

			if (deleteFlag) {
				BaseColor.Swap(Color.red);
				HideIconColor.Swap(Color.red);
				LockIconColor.Swap(Color.red);
				ThumbnailColor.Swap(Color.red);
			} else {
				BaseColor.Swap(Color.white);
				HideIconColor.Swap(Hided ? SelectedColor : NormalColor);
				LockIconColor.Swap(Locked ? SelectedColor : NormalColor);
				ThumbnailColor.Swap(Color.white);
			}
		}
	}

	bool selected;
	bool deleteFlag;
	Vector2 touchStartPosition;
	Vector2 touchDeltaPosition;

	void OnValidate () {
		if (BaseColor == null) BaseColor = GetComponent<ColorSwapable>();
		if (ThumbnailImage != null) {
			ThumbnailColor = ThumbnailImage.GetComponent<ColorSwapable>();
			ThumbnailFitter = ThumbnailImage.GetComponent<AspectRatioFitter>();
		}
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

	public void SetThumbnail (Texture texture) {
		ThumbnailImage.texture = texture;
		ThumbnailFitter.aspectRatio = (float)texture.width / texture.height;
	}

	public void OnHideToggleClicked () {
		Hided = !Hided;
		HideIconColor.Swap(Hided ? SelectedColor : NormalColor);

		if (OnChangedEvent != null) OnChangedEvent(this);
	}

	public void OnLockToggleClicked () {
		Locked = !Locked;
		LockIconColor.Swap(Locked ? SelectedColor : NormalColor);
	}

	public void OnPointerClick (PointerEventData eventData) {
		if (Controllable && !deleteFlag)
		if (OnClickedEvent != null) OnClickedEvent(this);
	}

	public void OnBeginDrag (PointerEventData eventData) {
		Controllable = false;

		touchStartPosition = CurrentPosition;
		touchDeltaPosition = Vector2.zero;

		if (OnPressedEvent != null) OnPressedEvent(this);
	}

	public void OnDrag (PointerEventData eventData) {
		touchDeltaPosition += eventData.delta * (600.0f / Screen.height);
		CurrentPosition = touchStartPosition + touchDeltaPosition;

		if (OnDragedEvent != null) OnDragedEvent(this);
	}

	public void OnEndDrag (PointerEventData eventData) {
		Controllable = true;

		if (deleteFlag) Delete();

		if (OnReleasedEvent != null) OnReleasedEvent(this);
	}

	public void Delete () {
		BaseColor.Swap(Color.clear);
		HideIconColor.Swap(Color.clear);
		LockIconColor.Swap(Color.clear);

		Destroy(gameObject, 0.5f);
	}
}
