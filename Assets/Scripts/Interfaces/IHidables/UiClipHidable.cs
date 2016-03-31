using UnityEngine;
using System.Collections;

public class UiClipHidable : MonoBehaviour, IHidable {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	public bool LockX;
	public float ShowX;
	public float HideX;

	public bool LockY;
	public float ShowY;
	public float HideY;

	public HidableState StartState;
	public HidableAction StartAction;

	RectTransform trans;

	Vector2 sizeDelta;

	void Awake () {
		trans = GetComponent<RectTransform>();
		sizeDelta = trans.sizeDelta;
	}

	void Start () {
		if (StartState == HidableState.Shown) {
			trans.sizeDelta = new Vector2(ShowX, ShowY);
		} else if (StartState == HidableState.Hided) {
			trans.sizeDelta = new Vector2(HideX, HideY);
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
			StartCoroutine(Slide(trans.sizeDelta.x, trans.sizeDelta.y, ShowX, ShowY));
		}
	}

	public void Hide () {
		if (!Hided()) {
			StopAllCoroutines();
			StartCoroutine(Slide(trans.sizeDelta.x, trans.sizeDelta.y, HideX, HideY));
		}
	}

	public bool Shown () {
		return trans.sizeDelta == new Vector2(ShowX, ShowY);
	}

	public bool Hided () {
		return trans.sizeDelta == new Vector2(HideX, HideY);
	}

	IEnumerator Slide (float startX, float startY, float endX, float endY) {
		float time = 0;

		while (time < TransitionDuration) {
			var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);
			trans.sizeDelta = new Vector2(
				LockX ? sizeDelta.x : Mathf.Lerp(startX, endX, easedStep),
				LockY ? sizeDelta.y : Mathf.Lerp(startY, endY, easedStep));
			
			time += Time.deltaTime;
			yield return null;
		}

		trans.sizeDelta = new Vector2(endX, endY);
	}
}
