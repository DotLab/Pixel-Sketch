using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public abstract class RectContentFitter : MonoBehaviour {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	[Space]
	public bool EaseSize;

	[Space]
	public RectContent[] Header;
	public RectContent[] Footer;

	[Space]
	public float Spacing = 10;
	public float Padding = 10;

	protected Vector2 originalSize;
	protected Vector2 targetSize;

	protected RectTransform trans;


	void Awake () {
		trans = GetComponent<RectTransform>();
	}

	public abstract bool TryFit (RectContent[] contents);

	[ContextMenu("Fit")]
	public void Fit (RectContent[] contents) {
		var fullContents = new List<RectContent>();
		fullContents.AddRange(Header);
		fullContents.AddRange(contents);
		fullContents.AddRange(Footer);

		if (TryFit(fullContents.ToArray())) {
			for (int i = 0; i < contents.Length; i++)
				contents[i].Index = i;
			
			StopAllCoroutines();
			StartCoroutine(FitHandler(fullContents.ToArray()));
		}
	}

	IEnumerator FitHandler (RectContent[] contents) {
		float time = 0;

		while (time < TransitionDuration) {
			var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

			if (EaseSize)
				trans.sizeDelta = Vector2.Lerp(originalSize, targetSize, easedStep);
			for (int i = 0; i < contents.Length; i++) {
				if (!contents[i].Controllable) continue;

				contents[i].CurrentPosition = Vector2.Lerp(
					contents[i].OriginalPosition,
					contents[i].TargetPosition, easedStep);
			}

			time += Time.deltaTime;
			yield return null;
		}

		if (EaseSize)
			trans.sizeDelta = targetSize;
		for (int i = 0; i < contents.Length; i++) {
			if (!contents[i].Controllable) continue;

			contents[i].CurrentPosition = contents[i].TargetPosition;
		}
	}
}
