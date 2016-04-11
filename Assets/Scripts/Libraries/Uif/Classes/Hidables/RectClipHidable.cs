using UnityEngine;
using System.Collections;

namespace Uif {
	[AddComponentMenu("Uif/Hidable/RectClipHidable")]
	public class RectClipHidable : Hidable {
		public bool LockWidth;
		public float ShowWidth;
		public float HideHeight;

		public bool LockHeight;
		public float ShowHeight;
		public float HideWidth;

		RectTransform trans;

		void Awake () {
			trans = GetComponent<RectTransform>();
		}

		public override void Show () {
			if (!Shown()) {
				StopAllCoroutines();
				StartCoroutine(Slide(trans.sizeDelta.x, trans.sizeDelta.y, ShowWidth, ShowHeight));
			}
		}

		public override void Hide () {
			if (!Hided()) {
				StopAllCoroutines();
				StartCoroutine(Slide(trans.sizeDelta.x, trans.sizeDelta.y, HideHeight, HideWidth));
			}
		}

		public override bool Shown () {
			return trans.sizeDelta == new Vector2(ShowWidth, ShowHeight);
		}

		public override bool Hided () {
			return trans.sizeDelta == new Vector2(HideHeight, HideWidth);
		}

		IEnumerator Slide (float startWidth, float startHeigth, float endWidth, float endHeight) {
			float time = 0;

			while (time < TransitionDuration) {
				var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

				trans.sizeDelta = new Vector2(
					LockWidth ? trans.sizeDelta.x : Mathf.Lerp(startWidth, endWidth, easedStep),
					LockHeight ? trans.sizeDelta.y : Mathf.Lerp(startHeigth, endHeight, easedStep));
			
				time += Time.deltaTime;
				yield return null;
			}
			
			trans.sizeDelta = new Vector2(
				LockWidth ? trans.sizeDelta.x : endWidth,
				LockHeight ? trans.sizeDelta.y : endHeight);
		}
	}
}