using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Uif {
	[AddComponentMenu("Uif/Swapable/ImageSwapable")]
	[RequireComponent(typeof(Image))]
	public class ImageSwapable : SpriteSwapable {
		public Image MainImage;
		public Image TransitionImage;


		void OnValidate () {
			if (MainImage == null) MainImage = GetComponent<Image>();
			if (TransitionImage == null) TransitionImage = transform.FindChild("Transition").GetComponent<Image>();
		}

		public override void Swap (Sprite newSprite) {
			if ((MainImage.sprite == newSprite && TransitionImage.sprite == null) || TransitionImage.sprite == newSprite)
				return;

			StopAllCoroutines();
			if (TransitionImage.sprite != null)
				StartCoroutine(SwapHandler(TransitionImage.sprite, TransitionImage.color.a, newSprite));
			else
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

				time += Time.deltaTime;
				yield return null;
			}

			MainImage.sprite = dstSprite;
			MainImage.color = SetAlpha(MainImage.color, 1);
			TransitionImage.sprite = null;
			TransitionImage.color = Color.clear;
		}

		static Color SetAlpha (Color color, float alpha) {
			color.a = alpha;
			return color;
		}
	}
}