using UnityEngine;

using System.Collections;

using Uif;

public class CursorController : MonoBehaviour {
	[System.Serializable]
	public class CursorConfig {
		public CursorType CursorType;
		public Sprite Sprite;
	}

	public delegate void OnCursorPositionChanged (Vector2 position);

	public event OnCursorPositionChanged OnCursorMovedEvent;
	public event OnCursorPositionChanged OnCursorPressedEvent;
	public event OnCursorPositionChanged OnCursorReleasedEvent;

	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	[Space]
	public RectTransform CursorTransform;
	public SpriteSwapable CursorSprite;

	[Space]
	public ColorSwapable CursorColor;
	public Color NormalColor = Color.white;
	public Color PressedColor = Color.cyan;

	[Space]
	public CursorConfig[] CursorConfigs;

	RectTransform trans;
	CursorConfig currentConfig;


	void Awake () {
		trans = GetComponent<RectTransform>();
	}

	void Start () {
		SetCursorType(CursorType.CrossCursor);
	}

	#region Cursor Operations

	public void SetCursorType (CursorType cursorType) {
		var config = GetCursorConfig(cursorType);
		if (config == currentConfig) return;
		else currentConfig = config;

		StopAllCoroutines();
		StartCoroutine(SetPivotHandler(CursorTransform.pivot, config.Sprite.pivot / config.Sprite.rect.height));
		CursorSprite.Swap(config.Sprite);
	}

	public Vector2 GetCursorPosition () {
		return CursorTransform.anchoredPosition;
	}

	public void MoveCursor (Vector2 delta) {
		var targetPosition = CursorTransform.anchoredPosition + delta;

		targetPosition.x = Clump(0, trans.rect.width, targetPosition.x);
		targetPosition.y = Clump(0, trans.rect.height, targetPosition.y);

		CursorTransform.anchoredPosition = targetPosition;

		if (OnCursorMovedEvent != null) OnCursorMovedEvent(GetCursorPosition());
	}

	public void NudgeCursor (Vector2 delta) {
		MoveCursor(delta / 2);
	}

	public void PressCursor () {
		CursorColor.Swap(PressedColor);

		if (OnCursorPressedEvent != null) OnCursorPressedEvent(GetCursorPosition());
	}

	public void ReleaseCursor () {
		CursorColor.Swap(NormalColor);

		if (OnCursorReleasedEvent != null) OnCursorReleasedEvent(GetCursorPosition());
	}

	#endregion

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

	static float Clump (float min, float max, float value) {
		return value < min ? min : value > max ? max : value;
	}
}
