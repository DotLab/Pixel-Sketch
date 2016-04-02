using UnityEngine;

public class IconToggleGroup : MonoBehaviour {
	public delegate void OnIconClicked (int index);

	public event OnIconClicked OnIconClickedEvent;

	public Color NormalColor = Color.white;
	public Color SelectedColor = Color.cyan;

	public int StartSelection = -1;

	public GameObject[] Icons;
	ISwapable<Color>[] iconSwapables;

	void Awake () {
		iconSwapables = new ISwapable<Color>[Icons.Length];
		for (int i = 0; i < Icons.Length; i++) {
			iconSwapables[i] = Icons[i].GetComponent<ISwapable<Color>>();
		}
	}

	void Start () {
		if (0 <= StartSelection && StartSelection < Icons.Length) {
			OnClicked(StartSelection);
		}
	}

	public void OnClicked (int index) {
		if (index >= iconSwapables.Length) throw new System.NotImplementedException();

		for (int i = 0; i < iconSwapables.Length; i++) {
			if (i == index) iconSwapables[i].Swap(SelectedColor);
			else iconSwapables[i].Swap(NormalColor);
		}

		if (OnIconClickedEvent != null) OnIconClickedEvent(index);
	}
}
