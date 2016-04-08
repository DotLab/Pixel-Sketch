using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections.Generic;

public class LayerController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public class PositionComparer : IComparer<LayerController> {
		public int Compare (LayerController obj1, LayerController obj2) {
			var obj1Value = obj1.Controllable ? -obj1.TargetPosition.y : -obj1.Trans.anchoredPosition.y;
			var obj2Value = obj2.Controllable ? -obj2.TargetPosition.y : -obj2.Trans.anchoredPosition.y;

			return obj1Value.CompareTo(obj2Value);
		}
	}

	public const float DeleteRange = 200;

	public delegate void OnLayerStateChange (LayerController layerController, bool deleted);

	public event OnLayerStateChange OnLayerDragEvent;
	public event OnLayerStateChange OnLayerReleaseEvent;

	public RectTransform Trans { get { return trans; } }

	public bool Controllable { get { return !pressed; } }

	public bool DeleteFlag { get { return deleteFlag; } }


	public Vector2 OriginalPosition;
	public Vector2 TargetPosition;

	public bool IsLayer;

	Vector2 originalPosition;
	Vector2 totalDelta;

	bool pressed;
	bool deleteFlag;

	RectTransform trans;
	ISwapable<Color> colorSwapable;

	void Awake () {
		trans = GetComponent<RectTransform>();
		colorSwapable = GetComponent<ISwapable<Color>>();
	}

	public void OnBeginDrag (PointerEventData eventData) {
		pressed = true;
		deleteFlag = false;

		originalPosition = trans.anchoredPosition;
		totalDelta = Vector2.zero;
	}

	public void OnDrag (PointerEventData eventData) {
		totalDelta += eventData.delta * (600.0f / Screen.height);
		trans.anchoredPosition = originalPosition + totalDelta;

		if (!deleteFlag && totalDelta.magnitude > DeleteRange) {
			deleteFlag = true;
			colorSwapable.Swap(Color.red);
		} else if (deleteFlag && totalDelta.magnitude < DeleteRange) {
			deleteFlag = false;
			colorSwapable.Swap(Color.white);
		}

		if (OnLayerDragEvent != null) OnLayerDragEvent(this, deleteFlag);
	}

	public void OnEndDrag (PointerEventData eventData) {
		pressed = false;

		if (deleteFlag) {
			if (IsLayer) {
				colorSwapable.Swap(Color.clear);
				Destroy(gameObject, 0.5f);
			} else {
				deleteFlag = false;
				colorSwapable.Swap(Color.white);
			}
		}

		if (OnLayerReleaseEvent != null) OnLayerReleaseEvent(this, deleteFlag);
	}
}
