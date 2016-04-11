using UnityEngine;
using System.Collections;

namespace Uif {
	[AddComponentMenu("Uif/Hidable/ColorableHidable")]
	[RequireComponent(typeof(Colorable))]
	public class ColorableHidable : Hidable {
		public Colorable Colorable;

		[Range(0, 1)]
		public float ShowAlpha = 1;
		[Range(0, 1)]
		public float HideAlpha;


		void OnValidate () {
			Colorable = GetComponent<Colorable>();
		}

		public override void Show () {
			if (!Shown()) {
				StopAllCoroutines();
				StartCoroutine(TransitionHandler(Colorable.GetColor(), ShowAlpha));
			}
		}

		public override void Hide () {
			if (!Hided()) {
				StopAllCoroutines();
				StartCoroutine(TransitionHandler(Colorable.GetColor(), HideAlpha));
			}
		}

		public override bool Shown () {
			return Colorable.GetColor().a == ShowAlpha;
		}

		public override bool Hided () {
			return Colorable.GetColor().a == HideAlpha;
		}

		IEnumerator TransitionHandler (Color srcColor, float dstAlpha) {
			float time = 0;

			var srcAlpha = srcColor.a;

			while (time < TransitionDuration) {
				var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

				srcColor.a = srcAlpha + (dstAlpha - srcAlpha) * easedStep;
				Colorable.SetColor(srcColor);

				time += Time.deltaTime;
				yield return null;
			}

			srcColor.a = dstAlpha;
			Colorable.SetColor(srcColor);
		}
	}
}