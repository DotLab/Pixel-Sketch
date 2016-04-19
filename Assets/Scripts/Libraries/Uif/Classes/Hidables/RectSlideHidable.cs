using UnityEngine;
using System.Collections;

namespace Uif {
	[AddComponentMenu("Uif/Hidable/Rect Slide Hidable")]
	public class RectSlideHidable : Hidable {
		public bool LockX;
		public float ShowX;
		public float HideX;

		public bool LockY;
		public float ShowY;
		public float HideY;

		RectTransform trans;

		void Awake () {
			trans = GetComponent<RectTransform>();
		}

		public override void Show () {
			if (!Shown()) {
				StopAllCoroutines();
				StartCoroutine(Slide(trans.anchoredPosition.x, trans.anchoredPosition.y, ShowX, ShowY));
			}
		}

		public override void Hide () {
			if (!Hided()) {
				StopAllCoroutines();
				StartCoroutine(Slide(trans.anchoredPosition.x, trans.anchoredPosition.y, HideX, HideY));
			}
		}

		public override bool Shown () {
			return trans.anchoredPosition == new Vector2(ShowX, ShowY);
		}

		public override bool Hided () {
			return trans.anchoredPosition == new Vector2(HideX, HideY);
		}

		IEnumerator Slide (float startX, float startY, float endX, float endY) {
			float time = 0;

			while (time < TransitionDuration) {
				var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

				trans.anchoredPosition = new Vector2(
					LockX ? trans.anchoredPosition.x : Mathf.Lerp(startX, endX, easedStep),
					LockY ? trans.anchoredPosition.y : Mathf.Lerp(startY, endY, easedStep));
			
				time += Time.deltaTime;
				yield return null;
			}

			trans.anchoredPosition = new Vector2(
				LockX ? trans.anchoredPosition.x : endX,
				LockY ? trans.anchoredPosition.y : endY);
		}
	}
}