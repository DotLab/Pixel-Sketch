using UnityEngine;

public class ColorableSwapable : MonoBehaviour, ISwapable<Color> {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	protected IColorable colorable;

	float transitionTime;
	bool isSwaping;

	Color srcColor;
	Color dstColor;

	void Awake () {
		colorable = GetComponent<IColorable>();
	}

	public void Swap (Color newColor) {
		if (colorable.GetColor() == newColor) {
			return;
		}

		srcColor = colorable.GetColor();
		dstColor = newColor;

		transitionTime = 0;
		isSwaping = true;
	}

	public void SilentSwap (Color newColor) {
		if (colorable.GetColor() == newColor) {
			return;
		}

		colorable.SetColor(newColor);
	}

	void Update () {
		if (!isSwaping) {
			return;
		}

		if (transitionTime < TransitionDuration) {
			float easedStep = Easing.EaseInOut(transitionTime / TransitionDuration, TransitionEasingType);
			colorable.SetColor(Color.Lerp(srcColor, dstColor, easedStep));
		} else {
			colorable.SetColor(dstColor);
			isSwaping = false;
			return;
		}

		transitionTime += Time.deltaTime;
	}
}
