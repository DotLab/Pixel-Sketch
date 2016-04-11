using UnityEngine;

namespace Uif {
	public abstract class Hidable : MonoBehaviour, IHidable {
		public EasingType TransitionEasingType = EasingType.Cubic;
		public float TransitionDuration = 0.5f;

		//public HidableState StartState;
		public HidableAction StartAction;

		void Start () {
			switch (StartAction) {
			case HidableAction.Show:
				Show();
				break;
			case HidableAction.Hide:
				Hide();
				break;
			}
		}

		public abstract void Hide ();

		public abstract void Show ();

		public abstract bool Hided ();

		public abstract bool Shown ();
	}
}