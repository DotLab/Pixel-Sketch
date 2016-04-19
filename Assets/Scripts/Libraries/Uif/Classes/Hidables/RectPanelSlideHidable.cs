using UnityEngine;
using System.Collections;

namespace Uif {
	[AddComponentMenu("Uif/Hidable/Rect Panel Slide Hidable")]
	public class RectPanelSlideHidable : Hidable {
		[Header("Lower Left : Upper Right")]
		public Vector2 ShowMin;
		public Vector2 ShowMax;

		[Header("Lower Left : Upper Right")]
		public Vector2 HideMin;
		public Vector2 HideMax;

		RectTransform trans;

		[ContextMenu("RecordShowOffset")]
		public void RecordShowOffset () {
			if (trans == null) trans = GetComponent<RectTransform>();
			ShowMin = trans.offsetMin;
			ShowMax = trans.offsetMax;
		}

		[ContextMenu("RecordHideOffset")]
		public void RecordHideOffset () {
			if (trans == null) trans = GetComponent<RectTransform>();
			HideMin = trans.offsetMin;
			HideMax = trans.offsetMax;
		}

		void Awake () {
			trans = GetComponent<RectTransform>();
		}

		public override void Show () {
			if (!Shown()) {
				StopAllCoroutines();
				StartCoroutine(Slide(trans.offsetMin, trans.offsetMax, ShowMin, ShowMax));
			}
		}

		public override void Hide () {
			if (!Hided()) {
				StopAllCoroutines();
				StartCoroutine(Slide(trans.offsetMin, trans.offsetMax, HideMin, HideMax));
			}
		}

		public override bool Shown () {
			return trans.offsetMin == ShowMin && trans.offsetMax != ShowMax;
		}

		public override bool Hided () {
			return trans.offsetMin == HideMin && trans.offsetMax != HideMax;
		}

		IEnumerator Slide (Vector2 startMin, Vector2 startMax, Vector2 endMin, Vector2 endMax) {
			float time = 0;

			while (time < TransitionDuration) {
				var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

				trans.offsetMin = Vector2.Lerp(startMin, endMin, easedStep);
				trans.offsetMax = Vector2.Lerp(startMax, endMax, easedStep);
			
				time += Time.deltaTime;
				yield return null;
			}

			trans.offsetMin = endMin;
			trans.offsetMax = endMax;
		}
	}
}