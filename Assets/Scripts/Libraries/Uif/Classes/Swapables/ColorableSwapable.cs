using UnityEngine;
using System.Collections;

namespace Uif {
	[AddComponentMenu("Uif/Swapable/ColorableSwapable")]
	[RequireComponent(typeof(Colorable))]
	public class ColorableSwapable : ColorSwapable {
		public Colorable Colorable;


		void OnValidate () {
			Colorable = GetComponent<Colorable>();
		}

		public override void Swap (Color newColor) {
			if (Colorable.GetColor() == newColor) return;

			StopAllCoroutines();
			StartCoroutine(SwapHandler(Colorable.GetColor(), newColor));
		}

		public override void SilentSwap (Color newColor) {
			Colorable.SetColor(newColor);
		}

		IEnumerator SwapHandler (Color srcColor, Color dstColor) {
			float time = 0;

			while (time < TransitionDuration) {
				var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

				Colorable.SetColor(Color.Lerp(srcColor, dstColor, easedStep));

				time += Time.deltaTime;
				yield return null;
			}

			Colorable.SetColor(dstColor);
		}
	}
}