namespace Uif {
	public interface IHidable {
		void Show ();

		void Hide ();

		bool Shown ();

		bool Hided ();
	}

	public enum HidableState {
		None,
		Shown,
		Hided
	}

	public enum HidableAction {
		None,
		Show,
		Hide
	}
}