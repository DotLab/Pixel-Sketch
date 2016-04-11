using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class LayerTabContentFitter : MonoBehaviour {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	[Space]
	public float Spacing = 10;
	public float Padding = 10;

	[Space]
	public List<LayerController> Layers;

	RectTransform trans;

	float originalHeight;
	float targetHeight;

	void Awake () {
		trans = GetComponent<RectTransform>();
	}

	[ContextMenu("Fit")]
	public void Fit () {
		var needFit = false;

		var height = Padding;
		for (int i = 0; i < Layers.Count; i++) {
			Layers[i].TargetPosition = new Vector2(0, -height);
			Layers[i].OriginalPosition = Layers[i].CurrentPosition;
			needFit |= Layers[i].OriginalPosition != Layers[i].TargetPosition;

			if (Layers[i].Active)
				height += Layers[i].Size.y + Spacing;
		}

		targetHeight = height - Spacing + Padding;
		originalHeight = trans.sizeDelta.y;
		needFit |= originalHeight != targetHeight;

		if (needFit) {
			StopAllCoroutines();
			StartCoroutine(FitHandler());
		}
	}

	IEnumerator FitHandler () {
		float time = 0;

		while (time < TransitionDuration) {
			var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

			trans.sizeDelta = new Vector2(
				trans.sizeDelta.x,
				Mathf.Lerp(originalHeight, targetHeight, easedStep));
			for (int i = 0; i < Layers.Count; i++) {
				if (!Layers[i].Controllable) continue;

				Layers[i].CurrentPosition = Vector2.Lerp(
					Layers[i].OriginalPosition,
					Layers[i].TargetPosition, easedStep);
			}

			time += Time.deltaTime;
			yield return null;
		}

		trans.sizeDelta = new Vector2(trans.sizeDelta.x, targetHeight);
		for (int i = 0; i < Layers.Count; i++) {
			if (!Layers[i].Controllable) continue;

			Layers[i].CurrentPosition = Layers[i].TargetPosition;
		}
	}
}
