public interface IHidable {
	void Hide ();

	void Show ();

	bool Hided ();

	bool Shown ();
}

public enum HidableState {
	None,
	Hided,
	Shown
}

public enum HidableAction {
	None,
	Hide,
	Show
}