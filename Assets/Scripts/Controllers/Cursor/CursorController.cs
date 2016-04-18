using UnityEngine;

using System.Collections;

using Uif;

public class CursorController : MonoBehaviour {
	[System.Serializable]
	public class CursorConfig {
		public CursorType CursorType;
		public Sprite Sprite;
	}

	public const float MaxClickInterval = 0.1f;

	public delegate void OnPositionChanged (Vector2 position);

	public event OnPositionChanged OnCursorClickedEvent;
	public event OnPositionChanged OnCursorDraggedEvent;
	public event OnPositionChanged OnCursorMovedEvent;

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

	bool pressed;
	float pressStartTime;


	void Awake () {
		trans = GetComponent<RectTransform>();
	}

	public void SetCursorType (CursorType cursorType) {
		var config = FindCursorConfig(cursorType);
		SetCursorConfig(config);
	}

	CursorConfig FindCursorConfig (CursorType cursorType) {
		foreach (var cursorConfig in CursorConfigs) {
			if (cursorConfig.CursorType == cursorType) return cursorConfig;
		}

		throw new System.NotImplementedException();
	}

	public void SetCursorConfig (CursorConfig cursorConfig) {
		if (cursorConfig == currentConfig) return;
		currentConfig = cursorConfig;

		StopAllCoroutines();
		StartCoroutine(SetPivotHandler(CursorTransform.pivot, cursorConfig.Sprite.pivot / cursorConfig.Sprite.rect.height));
		CursorSprite.Swap(cursorConfig.Sprite);
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

	public void MoveCursor (Vector2 delta) {
		var targetPosition = CursorTransform.anchoredPosition + delta;

		targetPosition.x = Clump(trans.rect.xMin, trans.rect.xMax, targetPosition.x);
		targetPosition.y = Clump(trans.rect.yMin, trans.rect.yMax, targetPosition.y);

		CursorTransform.anchoredPosition = targetPosition;

		if (pressed) {
			if (OnCursorDraggedEvent != null) OnCursorDraggedEvent(GetCursorPosition());
		} else {
			if (OnCursorMovedEvent != null) OnCursorMovedEvent(GetCursorPosition());
		}
	}

	public void NudgeCursor (Vector2 delta) {
		MoveCursor(delta / 2);
	}

	public void PressCursor () {
		CursorColor.Swap(PressedColor);

		pressed = true;
		pressStartTime = Time.time;
	}

	public void ReleaseCursor () {
		CursorColor.Swap(NormalColor);

		pressed = false;
		if (Time.time - pressStartTime < MaxClickInterval)
		if (OnCursorClickedEvent != null) OnCursorClickedEvent(GetCursorPosition());
	}

	public Vector2 GetCursorPosition () {
		return CursorTransform.anchoredPosition;
	}

	static float Clump (float min, float max, float value) {
		return value < min ? min : value > max ? max : value;
	}
}
