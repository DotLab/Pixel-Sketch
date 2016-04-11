using UnityEngine;

[System.Serializable]
public class RectCell {
	public RectTransform Trans;

	/// <summary>
	/// If the UiContent occupate some space.
	/// </summary>
	public bool Active = true;

	/// <summary>
	/// If the UiContent's position should be set.
	/// </summary>
	public bool Controllable = true;

	public Vector2 OriginalPosition;
	public Vector2 TargetPosition;

	public Vector2 CurrentPosition {
		get { return Trans.anchoredPosition; }
		set { Trans.anchoredPosition = value; }
	}

	public Vector2 Size {
		get { return Trans.sizeDelta; }
		set { Trans.sizeDelta = value; }
	}
}