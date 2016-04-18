using UnityEngine;

using System.Collections.Generic;

public class RectContent : MonoBehaviour {
	public class GridPositionComparer : IComparer<RectContent> {
		public int Compare (RectContent obj1, RectContent obj2) {
			return GetWeight(obj1, 100).CompareTo(GetWeight(obj2, 100));
		}

		public float GetWeight (RectContent obj, int maxCountY) {
			var weightX = (obj.Controllable ? obj.TargetPosition.x : obj.CurrentPosition.x) / obj.Size.x;
			var weightY = (obj.Controllable ? obj.TargetPosition.y : obj.CurrentPosition.y) / obj.Size.y;
			return weightX - (int)weightY * maxCountY;
		}
	}

	public class VerticalPositionComparer : IComparer<RectContent> {
		public int Compare (RectContent obj1, RectContent obj2) {
			var obj1Weight = obj1.Controllable ? -obj1.TargetPosition.y : -obj1.CurrentPosition.y;
			var obj2Weight = obj2.Controllable ? -obj2.TargetPosition.y : -obj2.CurrentPosition.y;

			return obj1Weight.CompareTo(obj2Weight);
		}
	}

	public class HorizontalPositionComparer : IComparer<RectContent> {
		public int Compare (RectContent obj1, RectContent obj2) {
			var obj1Weight = obj1.Controllable ? obj1.TargetPosition.x : obj1.CurrentPosition.x;
			var obj2Weight = obj2.Controllable ? obj2.TargetPosition.x : obj2.CurrentPosition.x;

			return obj1Weight.CompareTo(obj2Weight);
		}
	}

	public int Index;
	public bool Active = true;
	public bool Controllable = true;

	public Vector2 TargetPosition { get; set; }

	public Vector2 OriginalPosition { get; set; }

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
