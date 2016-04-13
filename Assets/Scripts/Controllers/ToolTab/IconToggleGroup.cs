using UnityEngine;
using Uif;

public class IconToggleGroup : MonoBehaviour {
	public delegate void OnIconClicked (int index);

	public event OnIconClicked OnIconClickedEvent;

	public Color NormalColor = Color.white;
	public Color SelectedColor = Color.cyan;

	public int StartSelection = -1;

	public ColorSwapable[] Icons;


	void Start () {
		if (0 <= StartSelection && StartSelection < Icons.Length) {
			for (int i = 0; i < Icons.Length; i++) {
				if (i == StartSelection) Icons[i].Swap(SelectedColor);
				else Icons[i].Swap(NormalColor);
			}
		}
	}

	public void OnClicked (int index) {
		if (index < 0 || index >= Icons.Length) throw new System.NotImplementedException();

		for (int i = 0; i < Icons.Length; i++) {
			if (i == index) Icons[i].Swap(SelectedColor);
			else Icons[i].Swap(NormalColor);
		}

		if (OnIconClickedEvent != null) OnIconClickedEvent(index);
	}
}
