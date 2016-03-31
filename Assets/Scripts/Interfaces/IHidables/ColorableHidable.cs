using UnityEngine;

public class ColorableHidable : ColorableSwapable, IHidable {
	public Color ShowColor = Color.white;
	public Color HideColor = Color.clear;

	public HidableState StartState;
	public HidableAction StartAction;

	void Start () {
		if (StartState == HidableState.Shown) {
			SilentSwap(ShowColor);
		} else if (StartState == HidableState.Hided) {
			SilentSwap(HideColor);
		}

		if (StartAction == HidableAction.Show) {
			Show();
		} else if (StartAction == HidableAction.Hide) {
			Hide();
		}
	}

	public void Show () {
		if (!Shown()) {
			Swap(ShowColor);
		}
	}

	public void Hide () {
		if (!Hided()) {
			Swap(HideColor);
		}
	}

	public bool Shown () {
		return colorable.GetColor() == ShowColor;
	}

	public bool Hided () {
		return colorable.GetColor() == HideColor;
	}
}
