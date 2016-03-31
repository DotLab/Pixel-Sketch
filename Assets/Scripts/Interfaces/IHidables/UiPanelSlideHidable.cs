using UnityEngine;
using System.Collections;

public class UiPanelSlideHidable : MonoBehaviour, IHidable {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	public Vector2 ShowMin;
	public Vector2 ShowMax;

	public Vector2 HideMin;
	public Vector2 HideMax;

	public HidableState StartState;
	public HidableAction StartAction;

	RectTransform trans;

	void Awake () {
		trans = GetComponent<RectTransform>();
	}

	void Start () {
		if (StartState == HidableState.Shown) {
			trans.offsetMin = ShowMin;
			trans.offsetMax = ShowMax;
		} else if (StartState == HidableState.Hided) {
			trans.offsetMin = HideMin;
			trans.offsetMax = HideMax;
		}

		if (StartAction == HidableAction.Show) {
			Show();
		} else if (StartAction == HidableAction.Hide) {
			Hide();
		}
	}

	public void Show () {
		if (!Shown()) {
			StopAllCoroutines();
			StartCoroutine(Slide(trans.offsetMin, trans.offsetMax, ShowMin, ShowMax));
		}
	}

	public void Hide () {
		if (!Hided()) {
			StopAllCoroutines();
			StartCoroutine(Slide(trans.offsetMin, trans.offsetMax, HideMin, HideMax));
		}
	}

	public bool Shown () {
		return trans.offsetMin == ShowMin && trans.offsetMax != ShowMax;
	}

	public bool Hided () {
		return trans.offsetMin == HideMin && trans.offsetMax != HideMax;
	}

	IEnumerator Slide (Vector2 startMin, Vector2 startMax, Vector2 endMin, Vector2 endMax) {
		float time = 0;

		while (time < TransitionDuration) {
			var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);
			trans.offsetMin = Vector2.Lerp(startMin, endMin, easedStep);
			trans.offsetMax = Vector2.Lerp(startMax, endMax, easedStep);
			
			time += Time.deltaTime;
			yield return null;
		}

		trans.offsetMin = endMin;
		trans.offsetMax = endMax;
	}
}
