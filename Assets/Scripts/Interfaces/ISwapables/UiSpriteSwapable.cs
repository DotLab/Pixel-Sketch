using UnityEngine;
using UnityEngine.UI;

public class UiSpriteSwapable : MonoBehaviour, ISwapable<Sprite> {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	public Image MainUiImage;
	public Image TransitionUiImage;

	float transitionTime;


	public void Swap (Sprite newSprite) {
		if (MainUiImage.sprite == newSprite || TransitionUiImage.sprite == newSprite) {
			return;
		}

		TransitionUiImage.sprite = newSprite;
		TransitionUiImage.color = Color.clear;
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
			MainUiImage.color = new Color(1, 1, 1, 1.0f - easedStep);
			TransitionUiImage.color = new Color(1, 1, 1, easedStep); 
		} else {
			MainUiImage.sprite = TransitionUiImage.sprite;
			MainUiImage.color = Color.white;

			TransitionUiImage.sprite = null;
			TransitionUiImage.color = Color.clear;
			return;
		}

		transitionTime += Time.deltaTime;
	}
}
