using UnityEngine;

using System.Collections;

using Uif;

public class CursorController : MonoBehaviour {
	[System.Serializable]
	public class CursorConfig {
		public CursorType CursorType;
		public Sprite Sprite;
	}

	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	[Space]
	public RectTransform CursorTransform;
	public SpriteSwapable CursorSprite;

	[Space]
	public CursorConfig[] CursorConfigs;


	void OnValidate () {
		if (CursorTransform == null) CursorTransform = GetComponent<RectTransform>();
		if (CursorSprite == null) CursorSprite = GetComponent<SpriteSwapable>();
	}

	void Start () {
		SetCursorType(CursorType.MoveObjectCursor);
	}

	public void SetCursorType (CursorType cursorType) {
		var config = GetCursorConfig(cursorType);
		StopAllCoroutines();
		StartCoroutine(SetPivotHandler(CursorTransform.pivot, config.Sprite.pivot / config.Sprite.rect.height));
		CursorSprite.Swap(config.Sprite);
	}

	public Vector2 GetCursorPosition () {
		return CursorTransform.anchoredPosition;
	}

	public void MoveCursor (Vector2 deltaPosition) {
		CursorTransform.anchoredPosition += deltaPosition;
	}

	CursorConfig GetCursorConfig (CursorType cursorType) {
		foreach (var cursorConfig in CursorConfigs) {
			if (cursorConfig.CursorType == cursorType) return cursorConfig;
		}

		throw new System.NotImplementedException();
	}

	IEnumerator SetPivotHandler (Vector2 srcPivot, Vector2 dstPivot) {
		float time = 0;

		while (time < TransitionDuration) {
			var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

			CursorTransform.pivot = Vector2.Lerp(srcPivot, dstPivot, easedStep);

			time += Time.deltaTime;
			yield return null;
		}

		CursorTransform.pivot = dstPivot;

	}
}
