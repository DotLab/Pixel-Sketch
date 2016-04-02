using UnityEngine;
using System.Collections;

public class GroupColorableHidable : MonoBehaviour, IHidable {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	public LayerMask ColorableLayer;
	public Color HideColor = Color.clear;

	public HidableState StartState;
	public HidableAction StartAction;

	IColorable[] colorables;
	Color[] originalColors;

	void Awake () {
		var qulifiedColorables = new System.Collections.Generic.List<IColorable>();
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild(i).gameObject.layer != ColorableLayer) continue;
			qulifiedColorables.Add(transform.GetChild(i).GetComponent<IColorable>());
		}
		colorables = qulifiedColorables.ToArray();
		originalColors = new Color[colorables.Length];
		for (int i = 0; i < colorables.Length; i++)
			originalColors[i] = colorables[i].GetColor();
	}

	//	void Start () {
	//		if (StartState == HidableState.Shown) {
	//			SilentSwap(ShowColor);
	//		} else if (StartState == HidableState.Hided) {
	//			SilentSwap(HideColor);
	//		}
	//
	//		if (StartAction == HidableAction.Show) {
	//			Show();
	//		} else if (StartAction == HidableAction.Hide) {
	//			Hide();
	//		}
	//	}

	public void Show () {
		if (!Shown()) {
			StopAllCoroutines();
			StartCoroutine(ShowHandler());
		}
	}

	public void Hide () {
		if (!Hided()) {
			StopAllCoroutines();
			StartCoroutine(HideHanler());
		}
	}

	public bool Shown () {
		return colorables[0].GetColor() == originalColors[0];
	}

	public bool Hided () {
		return colorables[0].GetColor() == HideColor;
	}

	IEnumerator ShowHandler () {
		float time = 0;

		while (time < TransitionDuration) {
			var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

			for (int i = 0; i < colorables.Length; i++)
				colorables[i].SetColor(Color.Lerp(HideColor, originalColors[i], easedStep));

			time += Time.deltaTime;
			yield return null;
		}

		foreach (var colorable in colorables)
			colorable.SetColor(HideColor);
	}

	IEnumerator HideHanler () {
		float time = 0;

		while (time < TransitionDuration) {
			var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

			for (int i = 0; i < colorables.Length; i++)
				colorables[i].SetColor(Color.Lerp(originalColors[i], HideColor, easedStep));

			time += Time.deltaTime;
			yield return null;
		}

		foreach (var colorable in colorables)
			colorable.SetColor(HideColor);
	}
}
