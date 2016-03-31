using UnityEngine;
using UnityEngine.UI;

public class UiSlideSpriteSwapable : MonoBehaviour, ISwapable<Sprite> {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	public Image MainUiImage;
	public Image TransitionUiImage;

	public bool LockX;
	public float StartX;
	public float EndX;

	public bool LockY;
	public float StartY;
	public float EndY;

	float transitionTime;

	RectTransform tranTrans;

	Vector2 anchoredPosition;

	void Awake () {
		tranTrans = MainUiImage.GetComponent<RectTransform>();

		anchoredPosition = tranTrans.anchoredPosition;
	}

	public void Swap (Sprite newSprite) {
		if (MainUiImage.sprite == newSprite || TransitionUiImage.sprite == newSprite) {
			return;
		}

		TransitionUiImage.sprite = newSprite;
		transitionTime = 0;
	}

	public void SilentSwap (Sprite newSprite) {
		if (MainUiImage.sprite == newSprite || TransitionUiImage.sprite == newSprite) {
			return;
		}

		MainUiImage.sprite = newSprite;
	}

	void Update () {
		if (TransitionUiImage.sprite == null) {
			return;
		}

		if (transitionTime < TransitionDuration) {
			float easedStep = Easing.EaseInOut(transitionTime / TransitionDuration, TransitionEasingType);
			tranTrans.anchoredPosition = new Vector2(
				LockX ? anchoredPosition.x : Mathf.Lerp(StartX, EndX, easedStep),
				LockY ? anchoredPosition.y : Mathf.Lerp(StartY, EndY, easedStep));

			MainUiImage.color = new Color(1, 1, 1, 1.0f - easedStep);
		} else {
			MainUiImage.sprite = TransitionUiImage.sprite;
			MainUiImage.color = new Color(1, 1, 1);

			tranTrans.anchoredPosition = new Vector2(StartX, StartY);
			TransitionUiImage.sprite = null;
			return;
		}

		transitionTime += Time.deltaTime;
	}
}
