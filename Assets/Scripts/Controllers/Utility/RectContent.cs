using UnityEngine;

using System.Collections.Generic;

public class RectContent : MonoBehaviour {
	public class PositionComparer : IComparer<RectContent> {
		public int Compare (RectContent obj1, RectContent obj2) {
			var obj1Value =
				(obj1.Controllable ? obj1.TargetPosition.x : obj1.CurrentPosition.x) / obj1.Size.x
				- (int)((obj1.Controllable ? obj1.TargetPosition.y : obj1.CurrentPosition.y) / obj1.Size.y) * 1000;
			var obj2Value =
				(obj2.Controllable ? obj2.TargetPosition.x : obj2.CurrentPosition.x) / obj2.Size.x
				- (int)((obj2.Controllable ? obj2.TargetPosition.y : obj2.CurrentPosition.y) / obj2.Size.y) * 1000;

			return obj1Value.CompareTo(obj2Value);
		}
	}

	public bool Active = true;
	public bool Controllable = true;

	public Vector2 OriginalPosition { get; set; }

	public Vector2 TargetPosition { get; set; }

	public Vector2 CurrentPosition {
		get { return trans.anchoredPosition; }
		set { trans.anchoredPosition = value; }
	}

	public Vector2 Size {
		get { return trans.sizeDelta; }
		set { trans.sizeDelta = value; }
	}

	RectTransform trans;


	public virtual void Awake () {
		trans = GetComponent<RectTransform>();
	}

	public virtual void Init (Transform parent, Vector2 position) {
		trans.SetParent(parent, false);
		trans.anchoredPosition = position;
	}
}
