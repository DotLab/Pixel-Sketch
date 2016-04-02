using UnityEngine;
using UnityEngine.UI;

public class UiSpriteSwapable : MonoBehaviour, ISwapable<Sprite> {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	public Image MainUiImage;
	public Image TransitionUiImage;

	float transitionTime;
	Color baseColor;


	public void Swap (Sprite newSprite) {
		if (MainUiImage.sprite == newSprite || TransitionUiImage.sprite == newSprite) {
			return;
		}

		baseColor = MainUiImage.color;
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
		// TODO: Make transition seamless & Use Coroutine instead of Update

		if (TransitionUiImage.sprite == null) {
			return;
		}

		if (transitionTime < TransitionDuration) {
			float easedStep = Easing.EaseInOut(transitionTime / TransitionDuration, TransitionEasingType);
			MainUiImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1.0f - easedStep);
			TransitionUiImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, easedStep); 
		} else {
			MainUiImage.sprite = TransitionUiImage.sprite;
			MainUiImage.color = baseColor;

			TransitionUiImage.sprite = null;
			TransitionUiImage.color = Color.clear;
			return;
		}

		transitionTime += Time.deltaTime;
	}
}
