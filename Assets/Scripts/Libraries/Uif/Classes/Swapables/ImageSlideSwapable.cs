using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Uif {
	[AddComponentMenu("Uif/Swapable/ImageSlideSwapable")]
	[RequireComponent(typeof(Image))]
	public class ImageSlideSwapable : SpriteSwapable {
		public Image MainImage;
		public Image TransitionImage;

		public bool LockX;
		public float StartX;
		public float EndX;

		public bool LockY;
		public float StartY;
		public float EndY;

		RectTransform trans;

		void OnValidate () {
			if (MainImage == null) MainImage = GetComponent<Image>();
			if (TransitionImage == null) TransitionImage = transform.FindChild("Transition").GetComponent<Image>();
		}

		void Awake () {
			trans = TransitionImage.GetComponent<RectTransform>();
		}

		public override void Swap (Sprite newSprite) {
			if ((MainImage.sprite == newSprite && TransitionImage.sprite == null) || TransitionImage.sprite == newSprite)
				return;

			StopAllCoroutines();
			StartCoroutine(SwapHandler(MainImage.sprite, MainImage.color.a, newSprite));
		}

		public override void SilentSwap (Sprite newSprite) {
			if ((MainImage.sprite == newSprite && TransitionImage.sprite == null) || TransitionImage.sprite == newSprite)
				return;

			MainImage.sprite = newSprite;
		}

		IEnumerator SwapHandler (Sprite srcSprite, float srcAlpha, Sprite dstSprite) {
			float time = 0;

			MainImage.sprite = srcSprite;
			MainImage.color = SetAlpha(MainImage.color, srcAlpha);
			TransitionImage.sprite = dstSprite;
			TransitionImage.color = SetAlpha(MainImage.color, 0);

			while (time < TransitionDuration) {
				var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

				MainImage.color = SetAlpha(MainImage.color, srcAlpha - srcAlpha * easedStep);
				TransitionImage.color = SetAlpha(MainImage.color, easedStep);
				trans.anchoredPosition = new Vector2(
					LockX ? trans.anchoredPosition.x : Mathf.Lerp(StartX, EndX, easedStep),
					LockY ? trans.anchoredPosition.y : Mathf.Lerp(StartY, EndY, easedStep));

				time += Time.deltaTime;
				yield return null;
			}

			MainImage.sprite = dstSprite;
			MainImage.color = SetAlpha(MainImage.color, 1);
			TransitionImage.sprite = null;
			TransitionImage.color = Color.clear;
			trans.anchoredPosition = new Vector2(
				LockX ? trans.anchoredPosition.x : StartY,
				LockY ? trans.anchoredPosition.y : StartY);
		}

		static Color SetAlpha (Color baseColor, float alpha) {
			baseColor.a = alpha;
			return baseColor;
		}
	}
}